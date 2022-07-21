/**
 * @brief Metalon ����
 * @author ��̼�
 * @date 22-07-21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metalon : Boss
{
    #region ����
    private float chaseCount;
    private float maxCount = 5f;

    private float spawnBabyTime = 5f;
    private bool canSpawnBaby = true;


    [SerializeField]
    private Monster[] spiders = new Monster[3];
    [SerializeField]
    private Transform[] spawnSpiderPos = new Transform[3];
    #endregion

    #region �ڷ�ƾ
    protected override IEnumerator Attack()
    {
        canAttack = false;

        nav.SetDestination(transform.position);
        transform.LookAt(target);

        IsAttacking = true;

        // ���� ����� �������� ���� (TODO : Ȯ��)
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
        
        // ������ �ð����� ���
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
        canAttack = true;
    }

    // ���� �Ź̰� ���� �׾�� �ٽ� ��ȯ�� �� ����
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

    //// �������� ����
    //protected override IEnumerator Chase()
    //{
    //    while (target && isChasing && !isStun)
    //    {
    //        if (!isHit)
    //        {
    //            // ������ ���� ���� �ȿ� �������� �ִٸ� �ܰŸ� ���� ����
    //            atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
    //            if (atkRangeColliders.Length > 0)
    //            {
    //                if (!isAttacking && canAttack) StartCoroutine(Attack());

    //                chaseCount = 0;
    //            }
    //            else if (atkRangeColliders.Length <= 0)         // ���� ������ �������� ���ٸ� 6��? �Ŀ� ���Ÿ� ����
    //            {
    //                if (!canAttack) canAttack = true;

    //                IsAttacking = false;
    //                PlayAnim(EMonsterAnim.run);

    //                chaseCount += Time.deltaTime;

    //                // 3�ʰ� ������ ����ü �߻�
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

    // ���Ÿ� ���� (�Ź� ���� ����) �ڷ�ƾ
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

    #region �Լ�

    // �Ź� ���� ����
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


