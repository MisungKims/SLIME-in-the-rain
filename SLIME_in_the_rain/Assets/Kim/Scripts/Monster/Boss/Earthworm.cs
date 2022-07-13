/**
 * @brief 지렁이 보스
 * @author 김미성
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthworm : Boss
{
    #region 변수
    private float chaseCount;
    private float maxCount = 3f;

    Vector3 lookRot;

    [SerializeField]
    private Transform projectilePos;
    #endregion

    //#region 유니티 함수
    //protected override void Awake()
    //{
    //    base.Awake();

    //    minAtkTime = 0.5f;
    //    maxAtkTime = 1.5f;
    //}
    //#endregion

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
                    if (!isAttacking) StartCoroutine(ShortAttack());

                    chaseCount = 0;
                }
                else if (atkRangeColliders.Length <= 0)         // 공격 범위에 슬라임이 없다면 3초? 후에 원거리 공격
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

    // 단거리 공격 코루틴
    private IEnumerator ShortAttack()
    {
        nav.SetDestination(target.position);
        transform.LookAt(target);

        chaseCount = 0;
        IsAttacking = true;

        anim.SetInteger("attack", 0);

        PlayAnim(EMonsterAnim.attack);

        // 랜덤한 시간동안 대기
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
    }

    // 원거리 공격 (투사체 발사) 코루틴
    private IEnumerator LongAttack()
    {
        chaseCount = 0;
        IsAttacking = true;
        nav.SetDestination(transform.position);
        transform.LookAt(target);

        // 애니메이션 실행
        anim.SetInteger("attack", 1);
        PlayAnim(EMonsterAnim.attack);

        yield return new WaitForSeconds(0.5f);

        // 투사체 발사
        GetProjectile();

        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }
    #endregion

    #region 함수
    private void GetProjectile()
    {
        // 투사체 발사
        EarthwormProjectile projectile = ObjectPoolingManager.Instance.Get(EProjectileFlag.earthworm).GetComponent<EarthwormProjectile>();
        projectile.earthworm = this;

        projectile.transform.position = projectilePos.position;
        projectile.transform.LookAt(target);

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }
    #endregion
}
