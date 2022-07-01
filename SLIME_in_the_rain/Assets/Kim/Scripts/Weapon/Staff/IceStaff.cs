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
    private void Awake()
    {
        weaponType = EWeaponType.iceStaff;
        projectileFlag = EProjectileFlag.ice;
        skillProjectileFlag = EProjectileFlag.iceSkill;
        dashCoolTime = 5f;
    }
    #endregion

    #region 함수
    // 투사체 생성
    public override void GetProjectile(EProjectileFlag flag, Vector3 targetPos)
    {
        // 투사체 생성 뒤 마우스 방향을 바라봄
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, projectilePos.position, Vector3.zero).GetComponent<StaffProjectile>();

        MissileRune(projectile);        // 유도 투사체 룬을 가지고 있다면 사용
        StunRune(flag, projectile);     // 스턴 2배 룬을 가지고 있다면 사용

        LookAtPos(projectile, targetPos);
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
