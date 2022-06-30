/**
 * @brief 활 전용 룬
 * @author 김미성
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneBow : RuneWeapon
{
    #region 유니티 함수
    private void Awake()
    {
        weaponType1 = EWeaponType.bow;
        weaponType2 = EWeaponType.bow;
    }
    #endregion

    #region 함수
    public override bool Use(Weapon weapon)
    {
        if (base.Use(weapon))
        {
            weapon.stats.attackPower += statManager.GetIncrementStat("AtkPower", 50);       // 데미지 50% 증가

            return true;
        }
        else return false;
    }
    #endregion
}
