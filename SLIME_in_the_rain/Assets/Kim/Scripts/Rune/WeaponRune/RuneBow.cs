/**
 * @brief Ȱ ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneBow : RuneWeapon, IAttackRune
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.bow;
        weaponType2 = EWeaponType.bow;
    }
    #endregion

    #region �Լ�


    //protected override bool UseWeaponRune()
    //{
    //    if (base.UseWeaponRune())
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    #endregion
}
