/**
 * @details 데미지를 입어야 반응하는 방목형 몬스터
 * @author 김미성
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

public class GrazingMonster : Monster
{
    #region 변수
    // 추적
    private bool takeDamage;            // 데미지를 입었는지?
    private bool isCounting;            // 추적 카운팅을 시작했는지?

    private float originCountTime = 10f;    // 기본 카운팅 시간
    private float countTime;                // 카운팅해야하는 시간

    // 캐싱
    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    private Slime slime;
    #endregion

    #region 유니티 함수
    protected override void Start()
    {
        slime = Slime.Instance;

        base.Start();
    }
    #endregion

    #region 코루틴
    // 슬라임 추적을 시작하고 시간이 지나도 공격을 못하면 추적 중지
    IEnumerator ChaseTimeCount()
    {
        isCounting = true;
        takeDamage = false;
        countTime = originCountTime;

        while (isChasing)
        {
            for (int i = 0; i < countTime; i++)
            {
                if(takeDamage)                      // 카운트 세는 도중 데미지를 입었다면, 카운트 시간을 증가시킴
                {
                    countTime += originCountTime;
                    takeDamage = false;
                }

                yield return waitFor1s;
            }
            
            if (isChasing && !isAttacking) StopChase();
        }

        isCounting = false;
    }

    IEnumerator DoStun(float time)
    {
        isStun = true;
        PlayAnim(EMonsterAnim.stun);

        yield return new WaitForSeconds(time);

        isStun = false;
        Chase();
    }
    #endregion

    #region 함수
    // 슬라임을 쫓기 시작
    void Chase()
    {
        takeDamage = true;

        StartChase(slime.transform);

        if(!isCounting)
        {
            StartCoroutine(ChaseTimeCount());
        }
    }

    public override void AutoAtkDamaged()
    {
        base.AutoAtkDamaged();

        Chase();
    }

    public override void SkillDamaged()
    {
        base.SkillDamaged();

        Chase();
    }

    public override void Stun(float stunTime)
    {
        float damage = statManager.GetSkillDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        StartCoroutine(DoStun(stunTime));
    }
    #endregion
}

