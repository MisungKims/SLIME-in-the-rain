using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.fireStaff;
        projectileFlag = EObjectFlag.fire;
        dashCoolTime = 5f;
    }
    #endregion
}
