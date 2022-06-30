/**
 * @brief 지팡이 무기 전용 룬
 * @author 김미성
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStaff : RuneWeapon
{
    #region 유니티 함수
    private void Awake()
    {
        weaponType1 = EWeaponType.iceStaff;
        weaponType2 = EWeaponType.fireStaff;
    }
    #endregion

    #region 함수

    #endregion
}
