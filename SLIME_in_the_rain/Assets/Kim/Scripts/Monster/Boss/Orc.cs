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
    private float stunTime = 2f;        // 슬라임이 스턴할 시간

    //// 감지된 슬라임을 쫓음
    //protected override IEnumerator Chase()
    //{
    //    while (target && isChasing && !isStun)
    //    {
    //        // 몬스터의 공격 범위 안에 슬라임이 있다면 공격 시작
    //        atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
    //        if (atkRangeColliders.Length > 0 && !isAttacking)
    //        {
    //            StartCoroutine(Attack());
    //        }
    //        else if (atkRangeColliders.Length <= 0)
    //        {
    //            // 슬라임을 쫓아다님
    //            nav.SetDestination(target.position);

    //            if(!doDamaged) IsAttacking = false;
    //            PlayAnim(EMonsterAnim.run);
    //        }

    //        yield return null;
    //    }
    //}

    public override void DamageSlime(int atkType)
    {
        base.DamageSlime(atkType);
        Debug.Log(atkType);

       if(atkType == 1) slime.Stun(stunTime);
    }
}
