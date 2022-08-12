/**
 * @brief 오크 보스
 * @author 김미성
 * @date 22-07-12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Boss
{
    private float chaseCount;
    private float maxCount = 3f;

    private float stunTime = 2f;        // 슬라임이 스턴할 시간

    protected override void Awake()
    {
        base.Awake();

        bossName = "오크";
        SetHPBar();
    }

    // 슬라임에게 데미지를 입힘
    public override void DamageSlime(int atkType)
    {
        base.DamageSlime(atkType);

       if(atkType == 1) slime.Stun(stunTime);       // 두번째 공격은 스턴
    }

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
                    if (!isAttacking && canAttack) StartCoroutine(ShortAttack());

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
        canAttack = false;

        nav.SetDestination(target.position);
        transform.LookAt(target);

        chaseCount = 0;
        IsAttacking = true;

        randAttack = Random.Range(0, 2);
        anim.SetInteger("attack", randAttack);

        PlayAnim(EMonsterAnim.attack);

        // 랜덤한 시간동안 대기
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
        canAttack = true;
    }

    // 원거리 공격 코루틴 (돌면서 슬라임에게 가까이 옴)
    private IEnumerator LongAttack()
    {
        canAttack = false;

        nav.SetDestination(target.position);
        chaseCount = 0;
        IsAttacking = true;
        nav.speed *= 4;

        // 애니메이션 실행
        randAttack = 2;
        anim.SetInteger("attack", 1);
        PlayAnim(EMonsterAnim.attack);

        while (!canAttack)      // 애니메이션이 끝날 때 까지
        {
            nav.SetDestination(target.position);

            yield return null;
        }

        nav.speed *= 0.25f;
        IsAttacking = false;
    }
}
