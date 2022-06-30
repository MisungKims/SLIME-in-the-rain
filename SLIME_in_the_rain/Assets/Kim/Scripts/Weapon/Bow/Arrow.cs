/**
 * @brief 
 * @author 김미성
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    #region 변수
    private bool isPenetrate = false;       // 관통 화살인지?
    public bool IsPenetrate { set { isPenetrate = value; } }
    #endregion

    private void OnEnable()
    {
        isPenetrate = false;
    }

    #region 함수
    // 데미지를 입힘
    protected override void DoDamage(Collider other)
    {
        if (!isPenetrate)
        {
            ObjectPoolingManager.Instance.Set(this.gameObject, flag);           // 관통화살이 아니면 사라짐
        }
        
        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
        }
    }
    #endregion
}
