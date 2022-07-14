/**
 * @brief Metalon ����
 * @author ��̼�
 * @date 22-07-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metalon : Boss
{
    #region ����
    private float chaseCount;
    private float maxCount = 10f;

    [SerializeField]
    private GameObject[] spiders = new GameObject[3];
    [SerializeField]
    private Transform[] spawnSpiderPos = new Transform[3];
    #endregion

    #region �ڷ�ƾ
    // �������� ����
    protected override IEnumerator Chase()
    {
        while (target && isChasing && !isStun)
        {
            if (!isHit)
            {
                // ������ ���� ���� �ȿ� �������� �ִٸ� �ܰŸ� ���� ����
                atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
                if (atkRangeColliders.Length > 0)
                {
                    if (!isAttacking) StartCoroutine(Attack());

                    chaseCount = 0;
                }
                else if (atkRangeColliders.Length <= 0)         // ���� ������ �������� ���ٸ� 6��? �Ŀ� ���Ÿ� ����
                {
                    IsAttacking = false;
                    PlayAnim(EMonsterAnim.run);

                    chaseCount += Time.deltaTime;

                    // 3�ʰ� ������ ����ü �߻�
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

    // ���Ÿ� ���� (�Ź� ���� ����) �ڷ�ƾ
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

    #region �Լ�

    // �Ź� ���� ����
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


