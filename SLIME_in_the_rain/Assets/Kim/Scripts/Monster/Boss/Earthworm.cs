/**
 * @brief ������ ����
 * @author ��̼�
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthworm : Boss
{
    private float chaseCount;
    private float maxCount = 3f;

    Vector3 lookRot;

    [SerializeField]
    private Transform projectilePos;

    protected override IEnumerator Chase()
    {
        while (target && isChasing && !isStun)
        {
            // ������ ���� ���� �ȿ� �������� �ִٸ� �ܰŸ� ���� ����
            atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
            if (atkRangeColliders.Length > 0)
            {
                if(!isAttacking) StartCoroutine(ShortAttack());

                nav.SetDestination(target.position);
                chaseCount = 0;
            }
            else if (atkRangeColliders.Length <= 0)         // ���� ������ �������� ���ٸ� 3��? �Ŀ� ���Ÿ� ����
            {
                IsAttacking = false;
                PlayAnim(EMonsterAnim.run);

                chaseCount += Time.deltaTime;

                // �������� �Ѿƴٴ�
                nav.SetDestination(target.position);

               // 3�ʰ� ������ ����ü �߻�
                if (chaseCount >= maxCount)
                {
                    yield return StartCoroutine(LongAttack());
                }
            }

            yield return null;
        }
    }

    private IEnumerator ShortAttack()
    {
        chaseCount = 0;
        IsAttacking = true;
        
        while (isAttacking)
        {
            anim.SetInteger("attack", 0);

            PlayAnim(EMonsterAnim.attack);

            yield return new WaitForSeconds(0.5f);
            
            DamageSlime(0);

            // ������ �ð����� ���
            randAtkTime = Random.Range(minAtkTime, maxAtkTime);
            yield return new WaitForSeconds(randAtkTime);
        }
    }

    private IEnumerator LongAttack()
    {
        chaseCount = 0;
        IsAttacking = true;
        nav.SetDestination(target.position);
        nav.SetDestination(transform.position);
        transform.LookAt(target);

        // �ִϸ��̼� ����
        anim.SetInteger("attack", 1);
        PlayAnim(EMonsterAnim.attack);

        yield return new WaitForSeconds(0.5f);

        // ����ü �߻�
        GetProjectile();

        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }

    private void GetProjectile()
    {
        // ����ü �߻�
        EarthwormProjectile projectile = ObjectPoolingManager.Instance.Get(EProjectileFlag.earthworm).GetComponent< EarthwormProjectile>();
        projectile.earthworm = this;

        projectile.transform.position = projectilePos.position;
        projectile.transform.LookAt(target);

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }
}
