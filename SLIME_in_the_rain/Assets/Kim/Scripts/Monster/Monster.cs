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

public abstract class Monster : MonoBehaviour, IDamage
{
    #region ����
    [SerializeField]
    protected int attackTypeCount;

    protected Animator anim;
    protected NavMeshAgent nav;

    [SerializeField]
    protected Stats stats;
    public Stats Stats { get { return stats; } }

    [SerializeField]
    protected LayerMask slimeLayer = 9;
    protected Transform target;

    private EMonsterAnim currentAnim;


    // ����
    protected Collider[] atkRangeColliders;       // ���� ���� ���� �ݶ��̴�

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

    protected int randAttack;      // ���� ���

    // ���� �� ��� �ð�
    protected float randAtkTime;
    protected float minAtkTime = 1f;
    protected float maxAtkTime = 3f;

    // ����
    protected bool isStun = false;

    protected bool isDie;

    // ĳ��
    private StatManager statManager;
    protected ObjectPoolingManager objectPoolingManager;
    protected MonsterManager monsterManager;
    private Slime slime;
    private DamageText damageText;

    private WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    
    #endregion

    #region ����Ƽ �Լ�
    protected virtual void Awake()
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
        monsterManager = MonsterManager.Instance;
        slime = Slime.Instance;

        nav.speed = stats.moveSpeed;
    }
    #endregion

    #region �ڷ�ƾ

    // ������ �������� ����
    protected virtual IEnumerator Chase()
    {
        while (target && isChasing && !isStun)
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
    private IEnumerator Attack()
    {
        while (isAttacking)
        {
            // ���� ����� �������� ����
            PlayAnim(EMonsterAnim.idle);

            yield return new WaitForSeconds(0.1f);

            randAttack = Random.Range(0, attackTypeCount);
            anim.SetInteger("attack", randAttack);

            Debug.Log(randAttack);

            PlayAnim(EMonsterAnim.attack);

            DamageSlime(randAttack);

            // ������ �ð����� ���
            randAtkTime = Random.Range(minAtkTime, maxAtkTime);
            yield return new WaitForSeconds(randAtkTime);
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
        if (isChasing) isChasing = false;

        PlayAnim(EMonsterAnim.stun);
        nav.SetDestination(transform.position);
        
        yield return new WaitForSeconds(time);

        isStun = false;
        TryStartChase();
    }


    // 3�� �� ������Ʈ ��Ȱ��ȭ
    protected virtual IEnumerator DieCoroutine()
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

        if (HaveDamage(statManager.GetAutoAtkDamage()))
        {
            if (!isStun) PlayAnim(EMonsterAnim.hit);
            TryStartChase();               // ������ ����ٴϱ� ����
        }
    }

    // �������� ��ų�� �������� ����
    public virtual void SkillDamaged()
    {
        if (isDie) return;

        if (HaveDamage(statManager.GetSkillDamage()))
        {
            if (!isStun) PlayAnim(EMonsterAnim.hit);
            TryStartChase();               // ������ ����ٴϱ� ����
        }
    }

    // ����
    public virtual void Stun(float stunTime)
    {
        if (isDie) return;

        if (HaveDamage(statManager.GetSkillDamage()))       // ���� �ʾ��� ��
        {
            if(!isStun) StartCoroutine(DoStun(stunTime));               // ���� �ڷ�ƾ ����
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
        if (stats.HP - damage <= 0)
        {
            stats.HP = 0;
            ShowDamage(damage);
            Die();
            return false;
        }
        else
        {
            stats.HP -= damage;
            ShowDamage(damage); 
            return true;
        }
    }

    // ������ �ǰ� ��ġ UI�� ������
    void ShowDamage(float damage)
    {
        if (isDie) return;

        damageText = objectPoolingManager.Get(EObjectFlag.damageText, transform.position).GetComponent<DamageText>();
        damageText.Damage = (int)damage;

        ShowHPBar();     // ü�¹� ����
    }
    #endregion

    #region ����
    // �����ӿ��� �������� ����
    public void DamageSlime(int atkType)
    {
        if (!target) return;

        slime.Damaged(stats, atkType);
    }

    // ������ ���� �õ�
    protected virtual void TryStartChase()
    {
        StartChase();
    }

    // ���� ����
    private void StartChase()
    {
        if (!isChasing)
        {
            isChasing = true;
            target = slime.transform;
            StartCoroutine(Chase());
        }
    }
    #endregion

    public abstract void ShowHPBar();       // ü�¹� Ȱ��ȭ
    public abstract void HideHPBar();       // ü�¹� ��Ȱ��ȭ


    // �ִϸ��̼� �÷���
    protected void PlayAnim(EMonsterAnim animState)
    {
        
        int state = (int)animState;
        currentAnim = animState;

        if (!(animState.Equals(EMonsterAnim.attack)))
        {
            anim.SetInteger("attack", -1);
        }


        anim.SetInteger("animation", state);


        //�ݺ��ؾ��ϴ� �ִϸ��̼��� �ƴ϶��, �ִϸ��̼��� ���� �� ���¸� Idle�� ����
        if (state >= (int)EMonsterAnim.attack && state <= (int)EMonsterAnim.hit)
        {
           // Debug.Log(animState + " " + anim.GetInteger("attack"));

            StartCoroutine(CheckAnimEnd(state.ToString()));
        }
    }
    #endregion
}