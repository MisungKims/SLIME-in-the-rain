using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : Projectile
{
    #region 변수
    private bool isUseRune = false;
    public bool IsUseRune { set { isUseRune = value; } }
    Transform targetPos;
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        isUseRune = false;
    }
    #endregion

    #region 함수
    protected override void Move()
    {
        // 타겟으로
        if (isUseRune)
        {

        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
    #endregion
}
