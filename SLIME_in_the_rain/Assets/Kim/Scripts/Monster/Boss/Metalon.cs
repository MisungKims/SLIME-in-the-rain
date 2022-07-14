/**
 * @brief Metalon 보스
 * @author 김미성
 * @date 22-07-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metalon : Boss
{
    #region 변수
    private float chaseCount;
    private float maxCount = 10f;

    [SerializeField]
    private GameObject[] spiders = new GameObject[3];
    [SerializeField]
    private Transform[] spawnSpiderPos = new Transform[3];
    #endregion

    #region 코루틴
    // 슬라임을 추적
    protected override IEnumerator Chase()
    {
        while (target && isChasing && !isStun)
        {
            if (!isHit)
            {
                // 몬스터의 공격 범위 안에 슬라임이 있다면 단거리 공격 시작
                atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
                if (atkRangeColliders.Length > 0)
                {
                    if (!isAttacking) StartCoroutine(Attack());

                    chaseCount = 0;
                }
                else if (atkRangeColliders.Length <= 0)         // 공격 범위에 슬라임이 없다면 6초? 후에 원거리 공격
                {
                    IsAttacking = false;
                    PlayAnim(EMonsterAnim.run);

                    chaseCount += Time.deltaTime;

                    // 3초가 지나면 투사체 발사
                    if (chaseCount >= maxCount)
                    {
                        yield return StartCoroutine(LongAttack());
                    }
                }

                if (!isAttacking) nav.SetDestination(target.position);
            }

            yield return null;
        }
    }

    // 원거리 공격 (거미 새끼 생성) 코루틴
    private IEnumerator LongAttack()
    {
        PlayAnim(EMonsterAnim.idleBattle);

        chaseCount = 0;
        IsAttacking = true;
        nav.SetDestination(transform.position);
        transform.LookAt(target);

        yield return new WaitForSeconds(0.1f);

        SpawnSpider();

        yield return new WaitForSeconds(2f);

        IsAttacking = false;
    }
    #endregion

    #region 함수

    // 거미 새끼 생성
    private void SpawnSpider()
    {
        for (int i = 0; i < spiders.Length; i++)
        {
            spiders[i].SetActive(true);
            spiders[i].transform.position = spawnSpiderPos[i].position;
        }
            
    }

    #endregion
}


