/**
 * @brief 몬스터 스크립트
 * @author 김미성
 * @date 22-07-12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;  // OnDrawGizmos

// 몬스터의 애니메이션 상태
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
    #region 변수
    [SerializeField]
    protected int attackTypeCount;

    protected Animator anim;
    public NavMeshAgent nav;
    private Rigidbody rb;

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

    // 공격
    protected Collider[] atkRangeColliders;       // 공격 범위 감지 콜라이더

    protected bool isChasing = false;   // 추적 중인지?

    protected bool isAttacking = false; // 공격 중인지?
    public bool IsAttacking
    {
        set
        {
            isAttacking = value;
            if (!isChasing) isAttacking = false;
        }
    }

    protected int randAttack;      // 공격 방법

    [HideInInspector]
    public int projectileAtk;

    protected bool canAttack = true;

    private bool isInGround = true;

    protected bool isInRange = false;

    // 공격 후 대기 시간
    protected float randAtkTime;
    protected float minAtkTime = 0.3f;
    protected float maxAtkTime = 1.5f;

    // 스턴
    protected bool isHit = false;

    protected bool isStun = false;

    public bool isDie = false;

    protected bool doDamage; // 슬라임에게 데미지를 입혔는지?

    protected bool noDamage = false;        // 데미지를 입힐 필요가 없는지?

    string animName;

    // 공중
    private float airTime;
    private float maxAirTime = 3f;
    private float jumpPower = 5f;
    protected bool canAir = true;

    // 미니맵
    [SerializeField]
    private MinimapWorldObject minimapObj;

    // 캐싱
    private StatManager statManager;
    protected ObjectPoolingManager objectPoolingManager;
    protected UIObjectPoolingManager uiPoolingManager;
    protected DungeonManager dungeonManager;
    protected Slime slime;
    private DamageText damageText;
    private Camera cam;
    protected WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    
    #endregion
    
    #region 유니티 함수
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        monsterCollider = GetComponent<Collider>();
        cam = Camera.main;

        chaseSpeed = stats.moveSpeed * multiplyChaseSpeedValue;
    }

    protected virtual void OnEnable()
    {
        isDie = false;
        if(canAir) monsterCollider.isTrigger = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Land"))
        {
            isInGround = true;
        }
    }
    #endregion

    #region 코루틴
    private IEnumerator Animation()
    {
        while (true)
        {
            if (isDie)
            {
                PlayAnim(EMonsterAnim.die);
            }
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

    // 슬라임의 공격에 의해 점프
    public IEnumerator Jump()
    {
        if(canAir)
        {
            isChasing = false;
            isHit = true;
            nav.enabled = false;

            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            isInGround = false;

            yield return StartCoroutine(IsInGround());      // 땅에 닿을 때 까지 기다림

            isHit = false;
            nav.enabled = true;
            TryStartChase();
        }
    }

   
    // 공중에 떠있을 동안 실행
    private IEnumerator IsInGround()
    {
        airTime = maxAirTime;
        while (!isInGround && airTime > 0)
        {
            airTime -= Time.deltaTime;

            yield return null;
        }
    }    

    // 감지된 슬라임을 쫓음
    protected virtual IEnumerator Chase()
    {
        while (target && isChasing && !isStun && !slime.isStealth)
        {
            nav.speed = chaseSpeed;
            if (!isHit)
            {
                // 몬스터의 공격 범위 안에 슬라임이 있다면 공격 시작
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

                    // 슬라임을 쫓아다님
                    if(nav.enabled) nav.SetDestination(target.position);

                    if (!doDamage) IsAttacking = false;         // 데미지를 입히는 중일 때 공격할 수 없도록
                }

                yield return null;
            }

            yield return null;
        }

        nav.speed = stats.moveSpeed;
        isChasing = false;
    }

    // 공격 
    protected virtual IEnumerator Attack()
    {
        canAttack = false;

        nav.SetDestination(transform.position);
        transform.LookAt(target);

        IsAttacking = true;

        // 공격 방식을 랜덤으로 실행 (TODO : 확률)
        randAttack = Random.Range(0, attackTypeCount);
        anim.SetInteger("attack", randAttack);

        PlayAnim(EMonsterAnim.attack);

        // 공격 애니메이션이 끝날 때 까지 대기
        while (!canAttack)
        {
            yield return null;
        }

        // 랜덤한 시간동안 대기
        // 대기 중 공격 범위를 벗어나면 바로 쫓아감
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        while (randAtkTime > 0 && isInRange)
        {
            randAtkTime -= Time.deltaTime;

            yield return null;
        }

        IsAttacking = false;
        canAttack = true;
    }

    // 애니메이션이 종료되었는지 확인 후 Idle로 상태 변경
    public IEnumerator CheckAnimEnd(string state)
    {
        animName = "Base Layer." + state;

        if (state == "3")       // 공격 상태일 때
        {
            animName = "Base Layer." + "Attack " + anim.GetInteger("attack");
        }

        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    if (currentAnim.Equals(EMonsterAnim.attack))    // 공격이 끝났을 때 공격 상태를 없음(-1)으로 변경
                    {
                        anim.SetInteger("attack", -1);
                        doDamage = false;
                    }
                    else if (currentAnim.Equals(EMonsterAnim.hit) && isInGround)
                    {
                        isHit = false;
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
                        
                        DamageSlime(randAttack);     // 공격 애니메이션 실행 시 슬라임이 데미지 입도록
                    }
                }
            }

            yield return null;
        }
    }

    // 스턴 코루틴
    IEnumerator DoStun(float time)
    {
        isStun = true;
        if (isChasing) isChasing = false;

        nav.SetDestination(transform.position);
        
        yield return new WaitForSeconds(time);

        isStun = false;
        TryStartChase();
    }


    // 3초 뒤 오브젝트 비활성화
    protected virtual IEnumerator DieCoroutine()
    {
        yield return waitFor3s;

        this.gameObject.SetActive(false);
    }

    #endregion

    #region 함수
    #region 데미지
    // 슬라임의 평타에 데미지를 입음
    public virtual void AutoAtkDamaged()
    {
        if (isDie) return;

        StartCoroutine(CameraShake.StartShake(0.1f, 0.08f));

        if (HaveDamage(statManager.GetAutoAtkDamage()))
        {
            isHit = true;
           
            TryStartChase();               // 슬라임 따라다니기 시작
        }
    }

    // 슬라임의 스킬에 데미지를 입음
    public virtual void SkillDamaged()
    {
        if (isDie) return;

        StartCoroutine(CameraShake.StartShake(0.1f, 0.2f));

        if (HaveDamage(statManager.GetSkillDamage()))
        {
            isHit = true;
           
            TryStartChase();               // 슬라임 따라다니기 시작
        }
    }

    // 스턴
    public virtual void Stun(float stunTime)
    {
        if (isDie) return;

        StartCoroutine(CameraShake.StartShake(0.1f, 0.23f));

        if (HaveDamage(statManager.GetSkillDamage()))       // 죽지 않았을 때
        {
            if(!isStun) StartCoroutine(DoStun(stunTime));               // 스턴 코루틴 실행
        }
    }

    // 죽음
    protected virtual void Die()
    {
        isDie = true;
        monsterCollider.isTrigger = true;

        // 슬라임 따라다니기를 중지
        isChasing = false;
        if (isAttacking) IsAttacking = false;

        nav.SetDestination(this.transform.position);

        target = null;

        HideHPBar();

        Minimap.Instance.RemoveMinimapIcon(minimapObj);     // 미니맵에서 제거

        if (DungeonManager.Instance) DungeonManager.Instance.DieMonster(this);

        objectPoolingManager.Get(EObjectFlag.gelatin, transform.position);

        StartCoroutine(DieCoroutine());
    }

    public void monsterDrop(int _round, int _range1, int _range2, Vector3 _pos)//아이템 드롭 -> 추후 몹, 오브젝트 잡았을때 랜덤값으로 출력되게 , 오브젝트 풀링 이랑 같이 사용하면 될듯
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

    // 데미지를 입음
    bool HaveDamage(float damage)
    {
        float result = stats.HP - damage;
       
        if (result <= 0)             // 죽음
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

    // 데미지 피격 수치 UI로 보여줌
    void ShowDamage(float damage)
    {
        if (isDie) return;

        damageText = uiPoolingManager.Get(EUIFlag.damageText, cam.WorldToScreenPoint(transform.position)).GetComponent<DamageText>();
        damageText.Damage = damage;

        ShowHPBar();     // 체력바 설정
    }
    #endregion

    #region 공격
    // 슬라임에게 데미지를 입힘
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

    // 슬라임 추적 시도
    protected virtual void TryStartChase()
    {
        StartChase();
    }

    // 추적 시작
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
 
    public abstract void ShowHPBar();       // 체력바 활성화
    public abstract void HideHPBar();       // 체력바 비활성화


    // 애니메이션 플레이
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
            anim.SetInteger("attack", -1);          // 공격 상태를 없음(-1)으로 변경
        }

        //// 반복해야하는 애니메이션이 아니라면(ex.공격), 애니메이션이 끝난 후 상태를 Idle로 변경
        if (state >= (int)EMonsterAnim.attack && state <= (int)EMonsterAnim.hit)
        {
            StartCoroutine(CheckAnimEnd(state.ToString()));
        }
    }

    #endregion
}