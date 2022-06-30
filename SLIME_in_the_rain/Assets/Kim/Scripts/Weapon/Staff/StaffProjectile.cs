using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : Projectile
{
    #region ����
    private bool isUseRune = false;
    public bool IsUseRune { set { isUseRune = value; } }
    Transform targetPos;
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        isUseRune = false;
    }
    #endregion

    #region �Լ�
    protected override void Move()
    {
        // Ÿ������
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
