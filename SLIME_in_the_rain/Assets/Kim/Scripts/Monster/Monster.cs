/**
 * @brief ���� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-07-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor;  // OnDrawGizmos

// ������ �ִϸ��̼� ����
public enum EMonsterAnim
{
    idle,
    walk,
    run,
    attack,
    hit,
    stun,
    die
}

public class Monster : MonoBehaviour, IDamage
{
    #region ����
    [SerializeField]
    private int attackTypeCount;

    private Animator anim;
    private NavMeshAgent nav;

    [SerializeField]
    protected Stats stats;

    [SerializeField]
    protected LayerMask slimeLayer = 9;
    protected Transform target;

    private EMonsterAnim currentAnim;


    // ����
    private bool takeDamage;            // �������� �Ծ�����?
    private bool isCounting;            // ���� ī������ �����ߴ���?

    private float originCountTime = 10f;    // �⺻ ī���� �ð�
    private float countTime;                // ī�����ؾ��ϴ� �ð�


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

    // ü�¹�
    private Slider hpBar;
    private Vector3 hpBarPos;

    protected bool isDie;

    // ĳ��
    private StatManager statManager;
    private ObjectPoolingManager objectPoolingManager;
    private Slime slime;

    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    private WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    
    #endregion

    #region ����Ƽ �Լ�

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        isDie = false;

        PlayAnim(EMonsterAnim.idle);
    }

   void Start()
    {
        statManager = StatManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        slime = Slime.Instance;

        nav.speed = stats.moveSpeed;
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
            PlayAnim(EMonsterAnim.idle);

            yield return new WaitForSeconds(0.1f);

            randAttack = Random.Range(0, attackTypeCount);
            anim.SetInteger("attack", randAttack);

            PlayAnim(EMonsterAnim.attack);

            DamageSlime(randAttack);

            // ������ �ð����� ���
            randAtkTime = Random.Range(minAtkTime, maxAtkTime);
            yield return new WaitForSeconds(randAtkTime);
        }
    }

    // ������ ������ �����ϰ� �ð��� ������ ������ ���ϸ� ���� ����
    IEnumerator ChaseTimeCount()
    {
        isCounting = true;
        takeDamage = false;
        countTime = originCountTime;

        for (int i = 0; i < countTime; i++)
        {
            if (takeDamage)                      // ī��Ʈ ���� ���� �������� �Ծ��ٸ�, ī��Ʈ �ð��� ������Ŵ
            {
                countTime += 6f;
                takeDamage = false;
            }

            if(isAttacking)                     // ������ �õ����� ������ ���� ���� ī��Ʈ �ð��� ����
            {
                countTime += 6f;
            }

            yield return waitFor1s;
        }

        if (isChasing && !isAttacking)
        {
            isCounting = false;
            StopChase();
        }
    }

    // �ִϸ��̼��� ����Ǿ����� Ȯ�� �� Idle�� ���� ����
    public IEnumerator CheckAnimEnd(string state)
    {
        string name = "Base Layer." + state;

        if (state == "3")
        {
            name = "Attack " + anim.GetInteger("attack");
        }

        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(name) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if(currentAnim.Equals(EMonsterAnim.attack)) anim.SetInteger("attack", -1);

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
        TryStartChase();
        // PlayAnim(EMonsterAnim.idle);
    }
    
    // ü�¹��� ��ġ�� �����ϴ� �ڷ�ƾ
    IEnumerator SetHPBarPos()
    {
        while (hpBar)
        {
            hpBarPos = transform.position;
            hpBarPos.y += 1.5f;

            hpBar.transform.position = hpBarPos;

            yield return null;
        }
    }

    // 3�� �� ������Ʈ ��Ȱ��ȭ
    IEnumerator DieCoroutine()
    {
        yield return waitFor3s;

        this.gameObject.SetActive(false);
    }
    #endregion

    #region �Լ�
    #region ������
    // �������� ��Ÿ�� �������� ����
    public virtual void AutoAtkDamaged()
    {
        if (isDie) return;

        PlayAnim(EMonsterAnim.hit);

        HaveDamage(statManager.GetAutoAtkDamage());
    }

    // �������� ��ų�� �������� ����
    public virtual void SkillDamaged()
    {
        if (isDie) return;

        PlayAnim(EMonsterAnim.hit);

        HaveDamage(statManager.GetSkillDamage());
    }

    // ����
    public virtual void Stun(float stunTime)
    {
        if (isDie) return;

        if (!HaveDamage(statManager.GetSkillDamage()))       // ���� �ʾ��� ��
        {
            StartCoroutine(DoStun(stunTime));               // ���� �ڷ�ƾ ����
        }
    }

    // ����
    void Die()
    {
        isDie = true;

        // ������ ����ٴϱ⸦ ����
        isChasing = false;
        if (isAttacking) IsAttacking = false;

        nav.SetDestination(this.transform.position);

        target = null;

        HideHPBar();

        PlayAnim(EMonsterAnim.die);

        StartCoroutine(DieCoroutine());
    }

    // �������� ����
    bool HaveDamage(float damage)
    {
        ShowDamage(damage);         // ������ ��ġ ������

        if (stats.HP - damage < 0)
        {
            stats.HP = 0;
            Die();
            return true;
        }
        else
        {
            stats.HP -= damage;
            TryStartChase();               // ������ ����ٴϱ� ����
            return false;
        }
    }

    // ������ �ǰ� ��ġ UI�� ������
    void ShowDamage(float damage)
    {
        if (isDie) return;

        DamageText damageText = ObjectPoolingManager.Instance.Get(EObjectFlag.damageText, transform.position).GetComponent<DamageText>();
        damageText.Damage = (int)damage;

        ShowHPBar();     // ü�¹� ����
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
            damagedObject.AutoAtkDamaged();
        }
    }

    // ������ ���� �õ�
    void TryStartChase()
    {
        takeDamage = true;

        StartChase(slime.transform);

        if (!isCounting)                // ���� Ÿ�� ī��Ʈ�� ���� ���� ���� ��
        {
            StartCoroutine(ChaseTimeCount());       // ���� Ÿ�� ī��Ʈ ����
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
        if (isChasing && !isCounting)
        {
            isChasing = false;
            if (isAttacking) IsAttacking = false;

            nav.SetDestination(this.transform.position);

            target = null;

            HideHPBar();

            PlayAnim(EMonsterAnim.idle);
        }
    }
    #endregion

    // ü�¹� Ȱ��ȭ
    protected virtual void ShowHPBar()
    {
        if (!hpBar)
        {
            hpBar = objectPoolingManager.Get(EObjectFlag.hpBar).GetComponent<Slider>();
            hpBar.maxValue = stats.maxHP;

            StartCoroutine(SetHPBarPos());
        }

        hpBar.value = stats.HP;
    }

    // ü�¹ٸ� ����
    protected virtual void HideHPBar()
    {
        if (!hpBar) return;

        objectPoolingManager.Set(hpBar.gameObject, EObjectFlag.hpBar);
        hpBar = null;
    }

    // �ִϸ��̼� �÷���
    protected void PlayAnim(EMonsterAnim animState)
    {
        
        int state = (int)animState;
        currentAnim = animState;

        anim.SetInteger("animation", state);

        if (!(animState.Equals(EMonsterAnim.attack)))
        {
            anim.SetInteger("attack", -1);
        }


        //�ݺ��ؾ��ϴ� �ִϸ��̼��� �ƴ϶��, �ִϸ��̼��� ���� �� ���¸� Idle�� ����
        if (state >= (int)EMonsterAnim.attack && state <= (int)EMonsterAnim.hit)
        {
           // Debug.Log(animState + " " + anim.GetInteger("attack"));

            StartCoroutine(CheckAnimEnd(state.ToString()));
        }
    }
    #endregion
}