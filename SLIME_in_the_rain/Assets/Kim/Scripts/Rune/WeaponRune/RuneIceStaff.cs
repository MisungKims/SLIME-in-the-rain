/**
 * @brief ���� ������ ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneIceStaff : RuneWeapon, ISkillRune
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.iceStaff;
        weaponType2 = EWeaponType.iceStaff;
    }
    #endregion

    #region �Լ�
    public void Skill()
    {

    }
    #endregion
}
