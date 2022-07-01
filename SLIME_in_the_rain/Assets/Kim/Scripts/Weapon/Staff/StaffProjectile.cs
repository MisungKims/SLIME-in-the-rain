using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : Projectile
{
    #region 변수
    private bool isUseRune = false;
    public bool IsUseRune { set { isUseRune = value; } }

    private Transform target;
    public Transform Target { set { target = value; } }
    #endregion

    #region 유니티 함수
    protected override void OnEnable()
    {
        base.OnEnable();
        isUseRune = false;
    }
    #endregion

    #region 함수
    protected override void Move()
    {
        if (isUseRune && target != null)          // 유도 룬 사용 시 타겟을 향해
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, 0.1f);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
    #endregion
}
