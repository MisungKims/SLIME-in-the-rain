/**
 * @brief Ȱ ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneBow : RuneWeapon
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType1 = EWeaponType.bow;
        weaponType2 = EWeaponType.bow;
    }
    #endregion

    #region �Լ�
    public override bool Use(Weapon weapon)
    {
        if (base.Use(weapon))
        {
            weapon.stats.attackPower += statManager.GetIncrementStat("AtkPower", 50);       // ������ 50% ����

            return true;
        }
        else return false;
    }
    #endregion
}
