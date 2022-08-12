/**
 * @brief 일반 몬스터
 * @author 김미성
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GeneralMonster : Monster
{
    #region 변수

    // 이동
    private Vector3 randPos;
    private Vector3 offset;
    private float distance;
    private float randTime;
    private bool isStop = false;
    private int mapRange;

    // 추적
    private bool takeDamage;            // 데미지를 입었는지?
    private bool isCounting;            // 추적 카운팅을 시작했는지?

    private float originCountTime = 30f;    // 기본 카운팅 시간
    private float countTime;                // 카운팅해야하는 시간
    protected float addCountAmount;         // 카운팅 시간 증가량

    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);

    // 체력바
    private GameObject hpBarObject;
    private Slider hpBar;
    private Vector3 hpBarPos = new Vector3(0, -0.65f, 0);

    private Camera mainCam;
    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        addCountAmount = 10f;
        mainCam = Camera.main;

        if(DungeonManager.Instance) mapRange = DungeonManager.Instance.mapRange;
        else mapRange = 8;
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
                nav.SetDestination(transform.position);
                PlayAnim(EMonsterAnim.idle);

                // 랜덤한 위치로 이동
                if (RandomPosition.GetRandomNavPoint(Vector3.zero, mapRange, out randPos))
                {
                    nav.SetDestination(randPos);
                    PlayAnim(EMonsterAnim.walk);
                    
                    isStop = false;
                    while (!isStop)
                    {
                        offset = transform.position - randPos;
                        distance = offset.sqrMagnitude;         // 몬스터와 랜덤한 위치 사이의 거리

                        if (distance < 1f)
                        {
                            nav.SetDestination(transform.position);
                            PlayAnim(EMonsterAnim.idle);

                            randTime = Random.Range(2f, 6f);
                            yield return new WaitForSeconds(randTime);

                            isStop = true;
                        }

                        yield return null;
                    }
                    
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
        while (hpBarObject)
        {
            hpBarObject.transform.position = mainCam.WorldToScreenPoint(transform.position + hpBarPos);

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
            hpBarObject = uiPoolingManager.Get(EUIFlag.hpBar);
            hpBar = hpBarObject.transform.GetChild(0).GetComponent<Slider>();
            hpBar.maxValue = stats.maxHP;

            StartCoroutine(SetHPBarPos());
        }

        hpBar.value = stats.HP;
    }

    // 체력바 비활성화
    public override void HideHPBar()
    {
        if (!hpBar) return;

        uiPoolingManager.Set(hpBarObject, EUIFlag.hpBar);
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
