/**
 * @brief Metalon 보스
 * @author 김미성
 * @date 22-07-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metalon : Boss
{
    #region 변수
    private float chaseCount;
    private float maxCount = 5f;

    private float spawnBabyTime = 5f;
    private bool canSpawnBaby = true;


    [SerializeField]
    private Monster[] spiders = new Monster[3];
    [SerializeField]
    private Transform[] spawnSpiderPos = new Transform[3];
    #endregion

    #region 코루틴
    protected override IEnumerator Attack()
    {
        canAttack = false;

        nav.SetDestination(transform.position);
        transform.LookAt(target);

        IsAttacking = true;

        // 공격 방식을 랜덤으로 실행 (TODO : 확률)
        randAttack = Random.Range(0, attackTypeCount);
        if(randAttack == 2 && canSpawnBaby)
        {
            anim.SetInteger("attack", 0);
            PlayAnim(EMonsterAnim.attack);

            yield return StartCoroutine(LongAttack());
        }
        else
        {
            PlayAnim(EMonsterAnim.attack);
            if(randAttack == 2) anim.SetInteger("attack", 0);
            else anim.SetInteger("attack", randAttack);
        }
        
        // 랜덤한 시간동안 대기
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
        canAttack = true;
    }

    // 새끼 거미가 전부 죽어야 다시 소환할 수 있음
    IEnumerator BabyTimeCount()
    {
        canSpawnBaby = false;
        bool isAllDie = false;

        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!spiders[i].isDie)
                {
                    isAllDie = false;
                    break;
                }
                else isAllDie = true;
            }

            if (isAllDie) break;

            yield return null;
        }
        
       yield return new WaitForSeconds(spawnBabyTime);

        canSpawnBaby = true;
    }

    //// 슬라임을 추적
    //protected override IEnumerator Chase()
    //{
    //    while (target && isChasing && !isStun)
    //    {
    //        if (!isHit)
    //        {
    //            // 몬스터의 공격 범위 안에 슬라임이 있다면 단거리 공격 시작
    //            atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
    //            if (atkRangeColliders.Length > 0)
    //            {
    //                if (!isAttacking && canAttack) StartCoroutine(Attack());

    //                chaseCount = 0;
    //            }
    //            else if (atkRangeColliders.Length <= 0)         // 공격 범위에 슬라임이 없다면 6초? 후에 원거리 공격
    //            {
    //                if (!canAttack) canAttack = true;

    //                IsAttacking = false;
    //                PlayAnim(EMonsterAnim.run);

    //                chaseCount += Time.deltaTime;

    //                // 3초가 지나면 투사체 발사
    //                if (chaseCount >= maxCount)
    //                {
    //                    yield return StartCoroutine(LongAttack());
    //                }
    //            }

    //            if (!isAttacking) nav.SetDestination(target.position);
    //        }

    //        yield return null;
    //    }
    //}

    // 원거리 공격 (거미 새끼 생성) 코루틴
    private IEnumerator LongAttack()
    {
        canAttack = false;

        PlayAnim(EMonsterAnim.idleBattle);

        chaseCount = 0;
        IsAttacking = true;
        nav.SetDestination(transform.position);
        transform.LookAt(target);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(BabyTimeCount());
        SpawnSpider();

        yield return new WaitForSeconds(2f);

        IsAttacking = false;
        canAttack = true;
    }
    #endregion

    #region 함수

    // 거미 새끼 생성
    private void SpawnSpider()
    {
        for (int i = 0; i < spiders.Length; i++)
        {
            spiders[i].gameObject.SetActive(true);
            spiders[i].transform.position = spawnSpiderPos[i].position;
        }
    }
    #endregion
}


