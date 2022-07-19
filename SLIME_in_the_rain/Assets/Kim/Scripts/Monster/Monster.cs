/**
 * @brief ���� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-07-12
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
    idleBattle,
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

    private Collider monsterCollider;

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

    [HideInInspector]
    public int projectileAtk;

    protected bool canAttack = true;


    // ���� �� ��� �ð�
    protected float randAtkTime;
    protected float minAtkTime = 0.3f;
    protected float maxAtkTime = 1.5f;

    // ����
    protected bool isHit = false;

    protected bool isStun = false;

    protected bool isDie = false;

    protected bool doDamage; // �����ӿ��� �������� ��������?

    protected bool noDamage = false;        // �������� ���� �ʿ䰡 ������?

    string animName;

    // ĳ��
    private StatManager statManager;
    protected ObjectPoolingManager objectPoolingManager;
    protected MonsterManager monsterManager;
    protected Slime slime;
    private DamageText damageText;

    protected WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    
    #endregion
    
    #region ����Ƽ �Լ�
    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        monsterCollider = GetComponent<Collider>();
    }

    protected virtual void OnEnable()
    {
        isDie = false;
        monsterCollider.isTrigger = false;

        PlayAnim(EMonsterAnim.idle);
    }

   void Start()
    {
        statManager = StatManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        monsterManager = MonsterManager.Instance;
        slime = Slime.Instance;

        nav.speed = stats.moveSpeed;
        nav.stoppingDistance = stats.attackRange;
    }

    private void Update()
    {
        if (isDie) PlayAnim(EMonsterAnim.die);
    }
    #endregion

    #region �ڷ�ƾ

    // ������ �������� ����
    protected virtual IEnumerator Chase()
    {
        while (target && isChasing && !isStun)
        {
            if(!isHit)
            {
                // ������ ���� ���� �ȿ� �������� �ִٸ� ���� ����
                atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
                if (atkRangeColliders.Length > 0 && !isAttacking && canAttack)
                {
                    StartCoroutine(Attack());
                }
                else if (atkRangeColliders.Length <= 0)
                {
                    if(!canAttack) canAttack = true;

                    // �������� �Ѿƴٴ�
                    nav.SetDestination(target.position);

                    if (!doDamage) IsAttacking = false;
                    PlayAnim(EMonsterAnim.run);
                }

                yield return null;
            }

            yield return null;
        }
    }

    // ���� 
    protected virtual IEnumerator Attack()
    {
        canAttack = false;

        nav.SetDestination(transform.position);
        transform.LookAt(target);

        IsAttacking = true;

        // ���� ����� �������� ���� (TODO : Ȯ��)
        randAttack = Random.Range(0, attackTypeCount);
        anim.SetInteger("attack", randAttack);

        PlayAnim(EMonsterAnim.attack);

        // ������ �ð����� ���
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
    }

    // �ִϸ��̼��� ����Ǿ����� Ȯ�� �� Idle�� ���� ����
    public IEnumerator CheckAnimEnd(string state)
    {
        animName = "Base Layer." + state;

        if (state == "3")       // ���� ������ ��
        {
            animName = "Base Layer." + "Attack " + anim.GetInteger("attack");
        }

        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    if (currentAnim.Equals(EMonsterAnim.attack))    // ������ ������ �� ���� ���¸� ����(-1)���� ����
                    {
                        anim.SetInteger("attack", -1);
                        doDamage = false;

                        canAttack = true;
                    }
                    else if (currentAnim.Equals(EMonsterAnim.hit))
                    {
                        isHit = false;
                    }

                    if (isDie) PlayAnim(EMonsterAnim.die);
                    else PlayAnim(EMonsterAnim.idleBattle);

                    break;
                }
                else 
                {
                    if (currentAnim.Equals(EMonsterAnim.attack) && !doDamage && !noDamage)
                    {
                        yield return new WaitForSeconds(0.1f);
                        
                        DamageSlime(randAttack);     // ���� �ִϸ��̼� ���� �� �������� ������ �Ե���
                    }

                    if (isDie) PlayAnim(EMonsterAnim.die);
                }
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
            isHit = true;
           
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
            isHit = true;
           
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
    protected virtual void Die()
    {
        isDie = true;
        monsterCollider.isTrigger = true;

        // ������ ����ٴϱ⸦ ����
        isChasing = false;
        if (isAttacking) IsAttacking = false;

        nav.SetDestination(this.transform.position);

        target = null;

        HideHPBar();

        StartCoroutine(DieCoroutine());
    }

    // �������� ����
    bool HaveDamage(float damage)
    {
        float result = stats.HP - damage;

        if (result <= 0)             // ����
        {
            stats.HP = 0;
            ShowDamage(damage);
            Die();
            return false;
        }
        else
        {
            stats.HP = result;
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
    public virtual void DamageSlime(int atkType)
    {
        if (!target && doDamage) return;

        if(!doDamage)
        {
            Debug.Log("damaged");

            doDamage = true;
            slime.Damaged(stats, atkType);
        }
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

            if (!slime) slime = Slime.Instance;

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

        anim.SetInteger("animation", state);

        if (isDie)
        {
            return;
        }

        if (!(animState.Equals(EMonsterAnim.attack)))
        {
            anim.SetInteger("attack", -1);          // ���� ���¸� ����(-1)���� ����
        }

        //anim.SetInteger("animation", state);

        //// �ݺ��ؾ��ϴ� �ִϸ��̼��� �ƴ϶��(ex.����), �ִϸ��̼��� ���� �� ���¸� Idle�� ����
        if (state >= (int)EMonsterAnim.attack && state <= (int)EMonsterAnim.hit)
        {
            StartCoroutine(CheckAnimEnd(state.ToString()));
        }

        //if (state == (int)EMonsterAnim.attack)
        //{
        //    StartCoroutine(CheckAnimEnd(state.ToString()));
        //}
    }
    #endregion
}