using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
{
    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.fireStaff;
        projectileFlag = EProjectileFlag.fire;
        skillProjectileFlag = EProjectileFlag.fireSkill;
        dashCoolTime = 5f;
    }

    protected override void Start()
    {
        base.Start();
        UIseting("��������", "������", "ȭ�����"); //���� ���� ���� //jeon �߰�
    }
    #endregion
}