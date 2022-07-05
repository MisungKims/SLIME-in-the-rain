/**
 * @brief ���� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;  // OnDrawGizmos

// ������ �ִϸ��̼� ����
public enum EMonsterAnim
{
    idle,
    walk,
    run,
    attack1,
    attack2,
    hit,
    stun,
    die
}

public class Monster : MonoBehaviour, IDamage
{
    #region ����
    private Animator anim;
    private NavMeshAgent nav;

    [SerializeField]
    protected Stats stats;

    [SerializeField]
    protected LayerMask slimeLayer = 9;
    protected Transform target;


    // ����
    Collider[] atkRangeColliders;       // ���� ���� ���� �ݶ��̴�

    protected bool isChasing = false;   // ���� ������?

    protected bool isAttacking = false; // ���� ������?
    public bool IsAttacking
    {
        set
        {
            isAttacking = value;
            if (!isChasing) isAttacking = false;
        }
    }

    private int randAttack;      // ���� ���

    // ���� �� ��� �ð�
    private float randAtkTime;          
    private float minAtkTime = 1f;
    private float maxAtkTime = 3f;

    // ����
    protected bool isStun = false;

    // ĳ��
    protected StatManager statManager;
    #endregion

    #region ����Ƽ �Լ�

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        statManager = StatManager.Instance;

        nav.speed = stats.moveSpeed;

        PlayAnim(EMonsterAnim.idle);
    }
    #endregion

    #region �ڷ�ƾ

    // ������ �������� ����
    IEnumerator Chase()
    {
        while (target && isChasing)
        {
            // ������ ���� ���� �ȿ� �������� �ִٸ� ���� ����
            atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
            if (atkRangeColliders.Length > 0 && !isAttacking)
            {
                IsAttacking = true;
                StartCoroutine(Attack());
            }
            else if(atkRangeColliders.Length <= 0)
            {
                IsAttacking = false;
                PlayAnim(EMonsterAnim.run);
            }

            // �������� �Ѿƴٴ�
            nav.SetDestination(target.position);

            yield return null;
        }
    }

    // ���� 
    IEnumerator Attack()
    {
        while (isAttacking)
        {
            // ���� ����� �������� ����
            randAttack = Random.Range((int)EMonsterAnim.attack1, (int)EMonsterAnim.attack2 + 1);
            DamageSlime(randAttack);

            PlayAnim((EMonsterAnim)randAttack);

            // ������ �ð����� ���
            randAtkTime = Random.Range(minAtkTime, maxAtkTime);
            yield return new WaitForSeconds(randAtkTime);
        }
    }


    // �ִϸ��̼��� ����Ǿ����� Ȯ�� �� Idle�� ���� ����
    public IEnumerator CheckAnimEnd(string state)
    {
        string name = "Base Layer." + state;
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(name) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                PlayAnim(EMonsterAnim.idle);
                break;
            }
            yield return null;
        }
    }

    // ���� �ڷ�ƾ
    IEnumerator DoStun(float time)
    {
        isStun = true;
        PlayAnim(EMonsterAnim.stun);

        yield return new WaitForSeconds(time);

        isStun = false;
        PlayAnim(EMonsterAnim.idle);
    }
    #endregion

    #region �Լ�
    #region ������
    // �������� ��Ÿ�� �������� ����
    public virtual void AutoAtkDamaged()
    {
        PlayAnim(EMonsterAnim.hit);

        float damage = statManager.GetAutoAtkDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        Debug.Log(name + " ��Ÿ " + statManager.GetAutoAtkDamage());
    }

    // �������ǽ�ų�� �������� ����
    public virtual void SkillDamaged()
    {
        PlayAnim(EMonsterAnim.hit);

        float damage = statManager.GetSkillDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        Debug.Log(name + " ��ų " + statManager.GetSkillDamage());
    }

    // ����
    public virtual void Stun(float stunTime)
    {
        float damage = statManager.GetSkillDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        StartCoroutine(DoStun(stunTime));
    }

    // ������ �ǰ� ��ġ UI�� ������
    protected void ShowDamage(float damage)
    {
        DamageText damageText = ObjectPoolingManager.Instance.Get(EObjectFlag.damageText, transform.position).GetComponent<DamageText>();
        damageText.Damage = (int)damage;
    }
    #endregion

    #region ����
    // �����ӿ��� �������� ����
    void DamageSlime(int atkType)
    {
        if (!target) return;

        IDamage damagedObject = target.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            // Attack1���� Attack2�� �������� �� ũ����
            if (atkType == (int)EMonsterAnim.attack1) damagedObject.AutoAtkDamaged();
            else damagedObject.SkillDamaged();
        }
    }

    // ���� ����
    protected void StartChase(Transform targetTransform)
    {
        if (!isChasing)
        {
            isChasing = true;
            target = targetTransform;
            StartCoroutine(Chase());
        }
    }

    // ���� ����
    protected void StopChase()
    {
        if (isChasing)
        {
            isChasing = false;
            target = null;
            PlayAnim(EMonsterAnim.idle);
        }
    }
    #endregion

    // �ִϸ��̼� �÷���
    protected void PlayAnim(EMonsterAnim animState)
    {
        int state = (int)animState;

        anim.SetInteger("animation", state);

        // �ݺ��ؾ��ϴ� �ִϸ��̼��� �ƴ϶��, �ִϸ��̼��� ���� �� ���¸� Idle�� ����
        if (state >= (int)EMonsterAnim.attack1 && state <= (int)EMonsterAnim.hit)
        {
            StartCoroutine(CheckAnimEnd(state.ToString()));
        }
    }
    #endregion
}
