/**
 * @brief 불 스킬 투사체 오브젝트
 * @author 김미성
 * @date 22-06-28
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile
{
    #region 함수
    // 데미지를 입힘
    protected override void DoDamage(Collider other)
    {
        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
            damagedObject.Stun();       // 스턴
        }
    }
    #endregion
}
