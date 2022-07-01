using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
{
    #region 유니티 함수
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
        UIseting("불지팡이", "빨간색", "화염방사"); //내용 정보 셋팅 //jeon 추가
    }
    #endregion
}