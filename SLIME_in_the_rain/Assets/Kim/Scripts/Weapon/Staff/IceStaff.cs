using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaff : Staff
{
    #region 변수
    // 룬
    private bool isHaveRune2 = false;
    public bool IsHaveRune2 { set { isHaveRune = value; } }
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
    protected override void UseRune()
    {
        if (!isHaveRune)
        {
            RuneManager.Instance.UseWeaponRune(this);       // 발동되지 않은 무기룬을 가지고 있다면 무기룬 발동
        }

        if (!isHaveRune2)
        {
            RuneManager.Instance.UseWeaponRune(this);       // 발동되지 않은 무기룬을 가지고 있다면 무기룬 발동
        }
    }

    // 투사체 생성
    public override void GetProjectile(EProjectileFlag flag, Vector3 targetPos)
    {
        // 투사체 생성 뒤 마우스 방향을 바라봄
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero).GetComponent<StaffProjectile>();

        if (isHaveRune)
        {
            projectile.IsUseRune = true;            // 지팡이 룬을 가지고 있다면 사용할 수 있도록 (유도)
            projectile.Target = slime.target;
        }
        if (isHaveRune2)        // 얼음 지팡이 룬 (스턴 시간 2배)
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

        projectile.transform.LookAt(targetPos);      // 화살 생성 뒤 마우스 방향을 바라봄

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }
    #endregion
}
