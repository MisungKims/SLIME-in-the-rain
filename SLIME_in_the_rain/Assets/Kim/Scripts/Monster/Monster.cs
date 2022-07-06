/**
 * @brief 몬스터 스크립트
 * @author 김미성
 * @date 22-07-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor;  // OnDrawGizmos

// 몬스터의 애니메이션 상태
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
    #region 변수
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


    // 추적
    private bool takeDamage;            // 데미지를 입었는지?
    private bool isCounting;            // 추적 카운팅을 시작했는지?

    private float originCountTime = 10f;    // 기본 카운팅 시간
    private float countTime;                // 카운팅해야하는 시간


    // 공격
    Collider[] atkRangeColliders;       // 공격 범위 감지 콜라이더

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

    private int randAttack;      // 공격 방법

    // 공격 후 대기 시간
    private float randAtkTime;          
    private float minAtkTime = 1f;
    private float maxAtkTime = 3f;

    // 스턴
    protected bool isStun = false;

    // 체력바
    [SerializeField]
    private Slider hpBar;

    // 캐싱
    protected StatManager statManager;
    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    private Slime slime;

    #endregion

    #region 유니티 함수

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        hpBar.maxValue = stats.maxHP;
    }

    protected virtual void Start()
    {
        statManager = StatManager.Instance;
        slime = Slime.Instance;

        nav.speed = stats.moveSpeed;

        PlayAnim(EMonsterAnim.idle);
    }
    #endregion

    #region 코루틴

    // 감지된 슬라임을 쫓음
    IEnumerator Chase()
    {
        while (target && isChasing)
        {
            // 몬스터의 공격 범위 안에 슬라임이 있다면 공격 시작
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

            // 슬라임을 쫓아다님
            nav.SetDestination(target.position);

            yield return null;
        }
    }

    // 공격 
    IEnumerator Attack()
    {
        while (isAttacking)
        {
            // 공격 방식을 랜덤으로 실행
            PlayAnim(EMonsterAnim.idle);

            yield return new WaitForSeconds(0.1f);

            randAttack = Random.Range(0, attackTypeCount);
            anim.SetInteger("attack", randAttack);

            PlayAnim(EMonsterAnim.attack);

            DamageSlime(randAttack);

            // 랜덤한 시간동안 대기
            randAtkTime = Random.Range(minAtkTime, maxAtkTime);
            yield return new WaitForSeconds(randAtkTime);
        }
    }

    // 슬라임 추적을 시작하고 시간이 지나도 공격을 못하면 추적 중지
    IEnumerator ChaseTimeCount()
    {
        isCounting = true;
        takeDamage = false;
        countTime = originCountTime;

        for (int i = 0; i < countTime; i++)
        {
            if (takeDamage)                      // 카운트 세는 도중 데미지를 입었다면, 카운트 시간을 증가시킴
            {
                countTime += originCountTime;
                takeDamage = false;
            }

            if(isAttacking)                     // 공격을 시도했을 때에도 추적 중지 카운트 시간을 증가
            {
                countTime += originCountTime;
            }

            yield return waitFor1s;
        }

        if (isChasing && !isAttacking)
        {
            isCounting = false;
            StopChase();
        }
    }

    // 애니메이션이 종료되었는지 확인 후 Idle로 상태 변경
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

    // 스턴 코루틴
    IEnumerator DoStun(float time)
    {
        isStun = true;
        PlayAnim(EMonsterAnim.stun);

        yield return new WaitForSeconds(time);

        isStun = false;
        HaveDamage();
        // PlayAnim(EMonsterAnim.idle);
    }
    #endregion

    #region 함수
    #region 데미지
    // 슬라임의 평타에 데미지를 입음
    public virtual void AutoAtkDamaged()
    {
        PlayAnim(EMonsterAnim.hit);

        float damage = statManager.GetAutoAtkDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        HaveDamage();

        Debug.Log(name + " 평타 " + statManager.GetAutoAtkDamage());
    }

    // 슬라임의 스킬에 데미지를 입음
    public virtual void SkillDamaged()
    {
        PlayAnim(EMonsterAnim.hit);

        float damage = statManager.GetSkillDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        HaveDamage();

        Debug.Log(name + " 스킬 " + statManager.GetSkillDamage());
    }

    // 스턴
    public virtual void Stun(float stunTime)
    {
        float damage = statManager.GetSkillDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        StartCoroutine(DoStun(stunTime));
    }

    // 데미지 피격 수치 UI로 보여줌
    protected void ShowDamage(float damage)
    {
        DamageText damageText = ObjectPoolingManager.Instance.Get(EObjectFlag.damageText, transform.position).GetComponent<DamageText>();
        damageText.Damage = (int)damage;

        SetHPBar();     // 체력바 설정
    }
    #endregion

    #region 공격
    // 슬라임에게 데미지를 입힘
    void DamageSlime(int atkType)
    {
        if (!target) return;

        IDamage damagedObject = target.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.AutoAtkDamaged();
        }
    }

    // 데미지를 입었을 때 슬라임 추적 시작
    void HaveDamage()
    {
        takeDamage = true;

        StartChase(slime.transform);

        if (!isCounting)
        {
            StartCoroutine(ChaseTimeCount());       // 추적 타임 카운트 시작
        }
    }

    // 체력바 활성화
    void SetHPBar()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
        }

        hpBar.value = stats.HP;
    }

    // 추적 시작
    protected void StartChase(Transform targetTransform)
    {
        if (!isChasing)
        {
            isChasing = true;
            target = targetTransform;
            StartCoroutine(Chase());
        }
    }

    // 추적 정지
    protected void StopChase()
    {
        if (isChasing && !isCounting)
        {
            isChasing = false;
            if (isAttacking) IsAttacking = false;
            target = null;
            PlayAnim(EMonsterAnim.idle);
        }
    }
    #endregion

    // 애니메이션 플레이
    protected void PlayAnim(EMonsterAnim animState)
    {
        
        int state = (int)animState;
        currentAnim = animState;

        anim.SetInteger("animation", state);

        if (!(animState.Equals(EMonsterAnim.attack)))
        {
            anim.SetInteger("attack", -1);
        }


        //반복해야하는 애니메이션이 아니라면, 애니메이션이 끝난 후 상태를 Idle로 변경
        if (state >= (int)EMonsterAnim.attack && state <= (int)EMonsterAnim.hit)
        {
            Debug.Log(animState + " " + anim.GetInteger("attack"));

            StartCoroutine(CheckAnimEnd(state.ToString()));
        }
    }
    #endregion
}