/**
 * @brief 
 * @author ��̼�
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    #region ����
    private bool isPenetrate = false;       // ���� ȭ������?
    public bool IsPenetrate { set { isPenetrate = value; } }
    #endregion

    private void OnEnable()
    {
        isPenetrate = false;
    }

    #region �Լ�
    // �������� ����
    protected override void DoDamage(Collider other)
    {
        if (!isPenetrate)
        {
            ObjectPoolingManager.Instance.Set(this.gameObject, flag);           // ����ȭ���� �ƴϸ� �����
        }
        
        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
        }
    }
    #endregion
}
