/**
 * @brief 몬스터
 * @author 김미성
 * @date 22-07-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

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
    #region 변수
    private Animator anim;

    [SerializeField]
    private Stats stats;

    // 슬라임 감지
    private float angleRange = 90f;
    Vector3 direction;
    float dotValue = 0f;
    private LayerMask slimeLayer = 9;
    private Transform target;

    // 공격
    private bool isAttacking = false;
    int randAttack;

    private bool isStun = false;

    // 캐싱
    StatManager statManager;
    #endregion

    #region 유니티 함수

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        statManager = StatManager.Instance;

        PlayAnim(EMonsterAnim.idle);

        StartCoroutine(DetectSlime());
    }
    #endregion

    #region 코루틴

    // 슬라임 감지 코루틴
    IEnumerator DetectSlime()
    {
        while (true)
        {
            if (!isAttacking)           // 공격 중이 아닐 때
            {
                // 원 안에 들어온 슬라임 콜라이더를 구하여 공격
                Collider[] colliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);

                if (colliders.Length > 0)
                {
                    dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));             // 각도에 대한 코사인값
                    direction = colliders[0].transform.position - transform.position;      // 몬스터에서 슬라임을 보는 벡터

                    if (direction.magnitude < stats.attackRange)         // 탐지한 오브젝트와 부채꼴의 중심점의 거리를 비교 
                    {
                        // 탐지한 오브젝트가 각도안에 들어왔으면 공격 시작
                        if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
                        {
                            target = colliders[0].transform;
                            Debug.Log("Detect slime");
                        }
                    }
                }

            }
            yield return null;
        }
    }

    // 애니메이션이 종료되었는지 확인 후 Idle로 상태 변경
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

    // 스턴 코루틴
    IEnumerator DoStun(float time)
    {
        isStun = true;
        PlayAnim(EMonsterAnim.stun);

        yield return new WaitForSeconds(time);

        isStun = false;
        PlayAnim(EMonsterAnim.idle);
    }
    #endregion

    #region 함수
    #region 데미지
    // 슬라임의 평타에 데미지를 입음
    public void AutoAtkDamaged()
    {
        PlayAnim(EMonsterAnim.hit);
        StartCoroutine(CheckAnimEnd("GetHit"));

        stats.HP -= statManager.GetAutoAtkDamage();

        Debug.Log(name + " 평타 " + statManager.GetAutoAtkDamage());
    }

    // 슬라임의스킬에 데미지를 입음
    public void SkillDamaged()
    {
        PlayAnim(EMonsterAnim.hit);
        StartCoroutine(CheckAnimEnd("GetHit"));

        stats.HP -= statManager.GetSkillDamage();

        Debug.Log(name + " 스킬 " + statManager.GetSkillDamage());
    }

    // 스턴
    public void Stun(float stunTime)
    {
        StartCoroutine(DoStun(stunTime));
    }
    #endregion

    // 공격
    void Attack()
    {
        if (!target) return;

        randAttack = Random.Range((int)EMonsterAnim.attack1, (int)EMonsterAnim.attack2 + 1);
        PlayAnim((EMonsterAnim)randAttack);
        if (randAttack == (int)EMonsterAnim.attack1) StartCoroutine(CheckAnimEnd("Attack01"));
        else StartCoroutine(CheckAnimEnd("Attack02"));
    }

    // 애니메이션 플레이
    void PlayAnim(EMonsterAnim animState)
    {
        anim.SetInteger("animation", (int)animState);
    }
    #endregion

    // 유니티 에디터에 부채꼴을 그려줄 메소드
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0f, 0f, 1f, 0.2f);
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, stats.attackRange);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, stats.attackRange);
    }
}
