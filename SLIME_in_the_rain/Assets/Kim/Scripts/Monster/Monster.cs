/**
 * @brief ���� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-07-12
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
    attack,
    hit,
    jumpHit,
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
    public NavMeshAgent nav;
   // private Rigidbody rb;

    [SerializeField]
    protected Stats stats;
    public Stats Stats { get { return stats; } }

    [SerializeField]
    private float multiplyChaseSpeedValue = 1.3f;
    protected float chaseSpeed;

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

   // private bool isInGround = true;

    protected bool isInRange = false;

    // ���� �� ��� �ð�
    protected float randAtkTime;
    protected float minAtkTime = 0.3f;
    protected float maxAtkTime = 1.5f;

    // ����
    protected bool isJumpHit = false;
    protected bool isHit = false;

    protected bool isStun = false;

    public bool isDie = false;

    protected bool doDamage; // �����ӿ��� �������� ��������?

    protected bool noDamage = false;        // �������� ���� �ʿ䰡 ������?

    string animName;

    //// ����
    //private float airTime;
    //private float maxAirTime = 3f;
    //private float jumpPower = 5f;
    //protected bool canAir = true;

    // �̴ϸ�
    [SerializeField]
    private MinimapWorldObject minimapObj;

    // ĳ��
    private StatManager statManager;
    protected ObjectPoolingManager objectPoolingManager;
    protected UIObjectPoolingManager uiPoolingManager;
    protected DungeonManager dungeonManager;
    protected Slime slime;
    private DamageText damageText;
    private Camera cam;
    protected WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    
    #endregion
    
    #region ����Ƽ �Լ�
    protected virtual void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        monsterCollider = GetComponent<Collider>();
        cam = Camera.main;

        chaseSpeed = stats.moveSpeed * multiplyChaseSpeedValue;
    }

    protected virtual void OnEnable()
    {
        isDie = false;
        //if(canAir) monsterCollider.isTrigger = false;

        stats.HP = stats.maxHP;

        StartCoroutine(Animation());
    }

   void Start()
    {
        statManager = StatManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
        uiPoolingManager = UIObjectPoolingManager.Instance;
        dungeonManager = DungeonManager.Instance;
        slime = Slime.Instance;

        nav.speed = stats.moveSpeed;
        nav.stoppingDistance = stats.attackRange;
    }

    #endregion

    #region �ڷ�ƾ
    private IEnumerator Animation()
    {
        while (true)
        {
            if (isDie)
            {
                PlayAnim(EMonsterAnim.die);
            }
            else if (isJumpHit) PlayAnim(EMonsterAnim.jumpHit);
            else if(!isAttacking)
            {
                if (isHit)
                {
                    if (isStun)
                    {
                        PlayAnim(EMonsterAnim.stun);
                    }
                    else
                    {
                        PlayAnim(EMonsterAnim.hit);
                    }
                }
                else if (isChasing)
                {
                    if (nav.velocity.Equals(Vector3.zero))
                    {
                        PlayAnim(EMonsterAnim.idle);
                    }
                    else
                    {
                        PlayAnim(EMonsterAnim.run);
                    }
                }
                else if (isInRange)
                {
                    if (nav.velocity.Equals(Vector3.zero))
                    {
                        PlayAnim(EMonsterAnim.idleBattle);
                    }
                    else
                    {
                        PlayAnim(EMonsterAnim.run);
                    }
                }
                else
                {
                    if (nav.velocity.Equals(Vector3.zero))
                    {
                        PlayAnim(EMonsterAnim.idle);
                    }
                    else
                    {
                        PlayAnim(EMonsterAnim.walk);
                    }
                }
            }
            
            yield return null;
        }
    }

    // �������� ���ݿ� ���� ����
    public void JumpHit()
    {
        nav.SetDestination(transform.position);
        isChasing = false;
        isJumpHit = true;
    }



    // ������ �������� ����
    protected virtual IEnumerator Chase()
    {
        while (CanChase())
        {
            nav.speed = chaseSpeed;
            if (!isHit)
            {
                // ������ ���� ���� �ȿ� �������� �ִٸ� ���� ����
                atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
                if (atkRangeColliders.Length > 0 && !isAttacking && canAttack)
                {
                    isInRange = true;
                    StartCoroutine(Attack());
                }
                else if (atkRangeColliders.Length <= 0 && !isAttacking)
                {
                    isInRange = false;
                    if (!canAttack) canAttack = true;

                    // �������� �Ѿƴٴ�
                    if(nav.enabled) nav.SetDestination(target.position);

                    if (!doDamage) IsAttacking = false;         // �������� ������ ���� �� ������ �� ������
                }

                yield return null;
            }

            yield return null;
        }

        nav.speed = stats.moveSpeed;
        isChasing = false;
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

        // ���� �ִϸ��̼��� ���� �� ���� ���
        while (!canAttack)
        {
            yield return null;
        }

        // ������ �ð����� ���
        // ��� �� ���� ������ ����� �ٷ� �Ѿư�
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        while (randAtkTime > 0 && isInRange)
        {
            randAtkTime -= Time.deltaTime;

            yield return null;
        }

        IsAttacking = false;
        canAttack = true;
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
                    }
                    else if (currentAnim.Equals(EMonsterAnim.hit))
                    {
                        isHit = false;
                        TryStartChase();
                    }
                    else if (currentAnim.Equals(EMonsterAnim.jumpHit))
                    {
                        isJumpHit = false;
                        TryStartChase();
                    }

                    canAttack = true;
                    PlayAnim(EMonsterAnim.idleBattle);
                    break;
                }
                else 
                {
                    if (currentAnim.Equals(EMonsterAnim.attack) && !doDamage && !noDamage)
                    {
                        yield return new WaitForSeconds(0.1f);
                        
                        DamageSlime(randAttack);     // ���� �ִϸ��̼� ���� �� �������� ������ �Ե���
                    }
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
    protected bool CanChase()
    {
        return target && isChasing && !isStun && !isDie && !isHit && !isJumpHit && !slime.isStealth && !slime.isDie;
    }

    #region ������
    // ī�޶� ���
    public void CameraShaking(float duration, float magnitude)
    {
        StartCoroutine(CameraShake.StartShake(duration, magnitude));
    }

    // �������� ��Ÿ�� �������� ����
    public virtual void AutoAtkDamaged()
    {
        if (isDie) return;

        CameraShaking(0.1f, 0.08f);

        if (HaveDamage(statManager.GetAutoAtkDamage()))
        {
            isHit = true;
           
            TryStartChase();               // ������ ����ٴϱ� ����
        }
    }

    // �������� ��ų�� �������� ����
    public virtual void SkillDamaged()
    {
        if (isDie) return;

        if (!isJumpHit) CameraShaking(0.1f, 0.2f);

        if (HaveDamage(statManager.GetSkillDamage()))
        {
            isHit = true;
           
            TryStartChase();               // ������ ����ٴϱ� ����
        }
    }

    // ����
    public virtual void Stun(float stunTime)
    {
        if (isDie) return;

        CameraShaking(0.1f, 0.23f);

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

        Minimap.Instance.RemoveMinimapIcon(minimapObj);     // �̴ϸʿ��� ����

        if (DungeonManager.Instance) DungeonManager.Instance.DieMonster(this);

        objectPoolingManager.Get(EObjectFlag.gelatin, transform.position);

        StartCoroutine(DieCoroutine());
    }

    public void monsterDrop(int _round, int _range1, int _range2, Vector3 _pos)//������ ��� -> ���� ��, ������Ʈ ������� ���������� ��µǰ� , ������Ʈ Ǯ�� �̶� ���� ����ϸ� �ɵ�
    {
        int count = Random.Range(0, _round + 1);
        float ranRAddPos = Random.Range(0, 0.1f);
        float ranFAddPos = Random.Range(0, 0.1f);
        for (int i = 0; i < count; i++)
        {
            Item item = ItemDatabase.Instance.AllitemDB[Random.Range(_range1, _range2)];
            Vector3 itemPos = _pos + (Vector3.right * ranRAddPos) + (Vector3.forward * ranFAddPos);
            objectPoolingManager.GetFieldItem(item, itemPos);
        }
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

        damageText = uiPoolingManager.Get(EUIFlag.damageText, cam.WorldToScreenPoint(transform.position)).GetComponent<DamageText>();
        damageText.Damage = damage;

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
            nav.SetDestination(transform.position);
            return;
        }

        if (!(animState.Equals(EMonsterAnim.attack)))
        {
            anim.SetInteger("attack", -1);          // ���� ���¸� ����(-1)���� ����
        }

        //// �ݺ��ؾ��ϴ� �ִϸ��̼��� �ƴ϶��(ex.����), �ִϸ��̼��� ���� �� ���¸� Idle�� ����
        if (state >= (int)EMonsterAnim.attack && state <= (int)EMonsterAnim.jumpHit)
        {
            StartCoroutine(CheckAnimEnd(state.ToString()));
        }
    }

    #endregion
}