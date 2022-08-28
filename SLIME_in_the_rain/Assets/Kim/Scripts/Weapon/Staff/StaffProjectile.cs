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

   // private Vector3 scale;
   // private StatManager statManager;
    #endregion

    #region ����Ƽ �Լ�

    //private void Awake()
    //{
    //    statManager = StatManager.Instance;
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.position = Vector3.down * 5f;
        isUseRune = false;
    }
    #endregion

    #region �Լ�
    protected override void Move()
    {
        if (isUseRune && target != null)          // ���� �� ��� �� Ÿ���� ����
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.1f);
        else
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    #endregion
}
