/**
 * @brief 일반 몬스터
 * @author 김미성
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMonster : Monster
{
    #region 변수

    // 이동
    private Vector3 randPos;
    private Vector3 offset;
    private float distance;
    private float randTime;

    private Vector3 prePos;
    private bool stop;

    // 추적
    private bool takeDamage;            // 데미지를 입었는지?
    private bool isCounting;            // 추적 카운팅을 시작했는지?

    private float originCountTime = 30f;    // 기본 카운팅 시간
    private float countTime;                // 카운팅해야하는 시간
    protected float addCountAmount;         // 카운팅 시간 증가량

    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);

    // 체력바
    private Slider hpBar;
    private Vector3 hpBarPos;

    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        addCountAmount = 10f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(Move());
    }
    #endregion

    #region 코루틴
    // 몬스터가 던전을 돌아다님
    IEnumerator Move()
    {
        while (true)
        {
            if(!isChasing && !isStun && !isDie && !isHit)
            {
                // 일정시간 가만히
                nav.SetDestination(transform.position);
                PlayAnim(EMonsterAnim.idle);

                randTime = Random.Range(2f, 6f);
                yield return new WaitForSeconds(randTime);
                

                // 랜덤한 위치로 이동
                randPos = monsterManager.GetRandomPosition();
                offset = transform.position - randPos;
                distance = offset.sqrMagnitude;         // 몬스터와 랜덤한 위치 사이의 거리

                nav.SetDestination(randPos);
                PlayAnim(EMonsterAnim.walk);

                prePos = transform.position;

                randTime = Random.Range(2f, 4f);            // 일정 시간동안 걷기
                while (randTime >= 0f)
                {
                    randTime -= Time.deltaTime;

                    if (distance < 0.1f)
                    {
                        nav.SetDestination(transform.position);
                        randTime = 0f;
                    }
                    prePos = transform.position;
                    yield return null;
                }
            }

            yield return null;
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
                countTime += addCountAmount;
                takeDamage = false;
            }

            yield return waitFor1s;
        }

        if (isChasing)
        {
            isCounting = false;
            StopChase();
        }
    }


    // 체력바의 위치를 조절하는 코루틴
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
    #endregion

    #region 함수
    // 체력바 활성화
    public override void ShowHPBar()
    {
        if (!hpBar)
        {
            hpBar = uiPoolingManager.Get(EUIFlag.hpBar).GetComponent<Slider>();
            hpBar.maxValue = stats.maxHP;

            StartCoroutine(SetHPBarPos());
        }

        hpBar.value = stats.HP;
    }

    // 체력바 비활성화
    public override void HideHPBar()
    {
        if (!hpBar) return;

        uiPoolingManager.Set(hpBar.gameObject, EUIFlag.hpBar);
        hpBar = null;
    }

    // 슬라임 추적 시도
    protected override void TryStartChase()
    {
        takeDamage = true;

        base.TryStartChase();

        if (!isCounting)                // 추적 타임 카운트가 돌고 있지 않을 때
        {
            StartCoroutine(ChaseTimeCount());       // 추적 타임 카운트 시작
        }
    }


    // 추적 정지
    private void StopChase()
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
}
