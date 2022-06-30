/**
 * @brief ���� ������ ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneIceStaff : RuneWeapon
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.iceStaff;
        weaponType2 = EWeaponType.iceStaff;
    }
    #endregion

    #region �Լ�
    public override bool Use(Weapon weapon)
    {
        if (UseWeaponRune(weapon))          // ���� �ߵ��� �� �ִ��� �Ǻ�
        {
            IceStaff iceStaff = weapon.GetComponent<IceStaff>();
            if (iceStaff != null)
            {
                iceStaff.IsHaveRune2 = true;       // ���Ⱑ ���� Ư���� ����� �� �ֵ���
                return true;
            }
        }

        return false;
    }
    #endregion
}
