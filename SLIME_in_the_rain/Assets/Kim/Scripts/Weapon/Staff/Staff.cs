/**
 * @brief 불 지팡이 스크립트
 * @author 김미성
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    #region 변수
    [SerializeField]
    protected Transform projectilePos;        // 생성될 투사체의 위치

    protected Vector3 lookRot;

    protected EProjectileFlag projectileFlag;     // 생성할 투사체의 flag
    protected EProjectileFlag skillProjectileFlag;     // 생성할 스킬 투사체의 flag

    //////// 대시
    float dashDistance = 400f;   // 대시할 거리
    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        GetProjectile(projectileFlag, targetPos, false);
    }

    // 스킬
    protected override void Skill(Vector3 targetPos)
    {
        base.Skill(targetPos);

        GetProjectile(skillProjectileFlag, targetPos, true);
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            Transform slimePos = slime.transform;

            slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // 정해진 곳으로 순간이동(점멸)

            slime.isDash = false;
        }
        return canDash;
    }

    // 투사체 생성
    public virtual void GetProjectile(EProjectileFlag flag, Vector3 targetPos, bool isSkill)
    {
        // 투사체 생성 뒤 마우스 방향을 바라봄
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, projectilePos.position, Vector3.zero).GetComponent<StaffProjectile>();
        projectile.isSkill = isSkill;
        projectile.transform.forward = targetPos;

        //LookAtPos(projectile, targetPos);
        MissileRune(projectile);        // 유도 투사체 룬을 가지고 있다면 사용
    }

    protected void LookAtPos(StaffProjectile projectile, Vector3 targetPos)
    {
        projectile.transform.LookAt(targetPos);      // 화살 생성 뒤 마우스 방향을 바라봄

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }

    // 유도 투사체 룬을 가지고 있다면 사용할 수 있도록
    protected void MissileRune(StaffProjectile projectile)
    {
        if (weaponRuneInfos[0].isActive)
        {
            projectile.IsUseRune = true;
            projectile.Target = slime.target;
        }
    }
    #endregion
}
