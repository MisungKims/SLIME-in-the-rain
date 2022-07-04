/**
 * @brief �� ��ų ����ü ������Ʈ
 * @author ��̼�
 * @date 22-06-28
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : StaffProjectile
{
    #region �Լ�
    // �������� ����
    protected override void DoDamage(Collider other, bool isSkill)
    {
        Debug.Log(other.name);

        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            if (isSkill) damagedObject.SkillDamaged();
            else damagedObject.AutoAtkDamaged();
        }
    }
    #endregion
}
