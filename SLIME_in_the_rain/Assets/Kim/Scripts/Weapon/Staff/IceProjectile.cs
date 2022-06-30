/**
 * @brief 얼음 스킬 투사체 오브젝트
 * @author 김미성
 * @date 22-06-28
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : StaffProjectile
{
    #region 변수
    private float stunTime = 1f;
    public float StunTime { get { return stunTime; } set { stunTime = value; } }
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        stunTime = 1f;
    }
    #endregion

    #region 함수
    // 데미지를 입힘
    protected override void DoDamage(Collider other)
    {
        Debug.Log(other.name);
        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
            damagedObject.Stun(stunTime);       // 스턴
        }
    }
    #endregion
}
