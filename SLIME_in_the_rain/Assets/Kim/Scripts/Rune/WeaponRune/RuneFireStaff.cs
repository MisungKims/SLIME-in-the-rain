/**
 * @brief �� ������ ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneFireStaff : RuneWeapon
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.fireStaff;
        weaponType2 = EWeaponType.fireStaff;
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
