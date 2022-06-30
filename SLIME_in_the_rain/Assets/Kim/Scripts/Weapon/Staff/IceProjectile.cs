/**
 * @brief ���� ��ų ����ü ������Ʈ
 * @author ��̼�
 * @date 22-06-28
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : StaffProjectile
{
    #region ����
    private float stunTime = 1f;
    public float StunTime { get { return stunTime; } set { stunTime = value; } }
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        stunTime = 1f;
    }
    #endregion

    #region �Լ�
    // �������� ����
    protected override void DoDamage(Collider other)
    {
        Debug.Log(other.name);
        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
            damagedObject.Stun(stunTime);       // ����
        }
    }
    #endregion
}
