using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
{
    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.fireStaff;
        projectileFlag = EObjectFlag.fire;
        skillProjectileFlag = EObjectFlag.fireSkill;
        dashCoolTime = 5f;
    }
    #endregion
}