using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : Projectile
{
    #region ����
    protected bool isUseRune = false;
    public bool IsUseRune { set { isUseRune = value; } }

    protected Transform target;
    public Transform Target { set { target = value; } }

    #endregion

    #region ����Ƽ �Լ�

    //private void Awake()
    //{
    //    statManager = StatManager.Instance;
    //}

    protected override void OnEnable()
    {
        removeTime = StatManager.Instance.myStats.attackRange * 0.4f;
        transform.position = Vector3.down * 5f;
        isUseRune = false;

        base.OnEnable();
    }
    #endregion

    #region �Լ�
    protected override void Move()
    {
        if (isUseRune && target != null)          // ���� �� ��� �� Ÿ���� ����
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        else
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    #endregion
}
