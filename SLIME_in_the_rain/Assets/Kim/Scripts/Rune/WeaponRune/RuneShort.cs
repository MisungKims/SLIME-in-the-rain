/**
 * @brief �ܰŸ� ���� ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneShort : RuneWeapon
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.dagger;
        weaponType2 = EWeaponType.sword;
    }
    #endregion

    #region �Լ�
    protected override bool UseWeaponRune()
    {
        if (base.UseWeaponRune())
        {
            return true;
        }

        return false;
    }
    #endregion
}
