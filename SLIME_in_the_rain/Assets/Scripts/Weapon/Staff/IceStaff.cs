using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaff : Staff
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.iceStaff;
        dashCoolTime = 5f;
    }
    #endregion
}
