/**
 * @brief �ܰŸ� ���� ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneMissileShort : RuneWeapon
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.dagger;
        weaponType2 = EWeaponType.sword;
    }
    #endregion

    #region �Լ�
    public override bool Use(Weapon weapon)
    {
        if (UseWeaponRune(weapon))          // ���� �ߵ��� �� �ִ��� �Ǻ�
        {
            Short shortWeapon = weapon.GetComponent<Short>();
            if (shortWeapon != null)
            {
                shortWeapon.IsHaveRune2 = true;       // ���Ⱑ ���� Ư���� ����� �� �ֵ���
                return true;
            }
        }

        return false;
    }
    #endregion
}
