using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
{
    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        weaponType = EWeaponType.fireStaff;
        projectileFlag = EProjectileFlag.fire;
        skillProjectileFlag = EProjectileFlag.fireSkill;
        maxDashCoolTime = 5f;
    }

    private void Start()
    {
        UIseting("��������", "������", "ȭ�����"); //���� ���� ���� //jeon �߰�
    }
    #endregion
}