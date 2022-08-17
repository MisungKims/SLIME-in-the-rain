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
    private Vector3 dashPos;
    private Vector3 offset;
    private float distance;
    private float dashDistance = 400f;   // 대시할 거리
    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack()
    {
        base.AutoAttack();

        GetProjectile(projectileFlag, targetPos, false);
    }

    // 스킬
    protected override void Skill()
    {
        base.Skill();

        GetProjectile(skillProjectileFlag, this.targetPos, true);
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            slime.DashTime = slime.originDashTime;
            Transform slimePos = slime.transform;

            // 대시 후 도착하는 위치에 벽이 있다면, 벽 앞에서 대시를 멈추도록
            // 없다면 정해진 곳으로 순간이동(점멸)

            dashPos = slimePos.position + slimePos.forward * dashDistance * Time.deltaTime;
            distance = Vector3.Distance(slimePos.position, dashPos);

#if UNITY_EDITOR
            Debug.DrawRay(slimePos.position + Vector3.up * 0.1f, slime.transform.forward * distance, Color.blue, 0.3f);
#endif
            if (Physics.Raycast(slimePos.position + Vector3.up * 0.1f, slime.transform.forward, out RaycastHit hit, distance))
            {
                if (hit.transform.gameObject.layer == 11) slimePos.position = hit.point - slimePos.forward * 0.5f;
                else slimePos.position = dashPos;
            }
            else slimePos.position = dashPos;

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
