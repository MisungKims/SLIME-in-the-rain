using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaff : Staff
{
    #region 변수
    // 룬
    //private bool isHaveRune2 = false;
    //public bool IsHaveRune2 { set { isHaveRune = value; } }
    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        weaponType = EWeaponType.iceStaff;
        projectileFlag = EProjectileFlag.ice;
        skillProjectileFlag = EProjectileFlag.iceSkill;
        dashCoolTime = 5f;
    }

    protected override void Start()
    {
        base.Start();

        UIseting("얼음지팡이", "파란색", "얼음공격"); //내용 정보 셋팅 //jeon 추가
    }

    #endregion

    #region 함수
    // 투사체 생성
    public override void GetProjectile(EProjectileFlag flag, Vector3 targetPos, bool isSkill)
    {
        // 투사체 생성 뒤 마우스 방향을 바라봄
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, projectilePos.position, Vector3.zero).GetComponent<StaffProjectile>();
        projectile.isSkill = isSkill;
        projectile.transform.forward = targetPos;

        MissileRune(projectile);        // 유도 투사체 룬을 가지고 있다면 사용
        StunRune(flag, projectile);     // 스턴 2배 룬을 가지고 있다면 사용

       // LookAtPos(projectile, targetPos);
    }

    // 얼음 지팡이 룬이 있다면 스턴 시간 2배
    void StunRune(EProjectileFlag flag, StaffProjectile projectile)
    {
        if (weaponRuneInfos[1].isActive)
        {
            if (flag.Equals(EProjectileFlag.iceSkill))
            {
                IceProjectile ice = projectile.GetComponent<IceProjectile>();
                if (ice != null)
                {
                    ice.StunTime *= 2f;
                }
            }
        }
    }
    #endregion
}
