using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : Projectile
{
    #region ����
    private bool isUseRune = false;
    public bool IsUseRune { set { isUseRune = value; } }

    private Transform target;
    public Transform Target { set { target = value; } }
    #endregion

    #region ����Ƽ �Լ�
    protected override void OnEnable()
    {
        base.OnEnable();
        isUseRune = false;
    }
    #endregion

    #region �Լ�
    protected override void Move()
    {
        if (isUseRune && target != null)          // ���� �� ��� �� Ÿ���� ����
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
