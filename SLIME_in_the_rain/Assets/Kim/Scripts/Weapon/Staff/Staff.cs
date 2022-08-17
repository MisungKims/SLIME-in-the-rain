/**
 * @brief �� ������ ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    #region ����
    [SerializeField]
    protected Transform projectilePos;        // ������ ����ü�� ��ġ

    protected Vector3 lookRot;

    protected EProjectileFlag projectileFlag;     // ������ ����ü�� flag
    protected EProjectileFlag skillProjectileFlag;     // ������ ��ų ����ü�� flag

    //////// ���
    private Vector3 dashPos;
    private Vector3 offset;
    private float distance;
    private float dashDistance = 400f;   // ����� �Ÿ�
    #endregion

    #region �Լ�
    // ��Ÿ
    protected override void AutoAttack()
    {
        base.AutoAttack();

        GetProjectile(projectileFlag, targetPos, false);
    }

    // ��ų
    protected override void Skill()
    {
        base.Skill();

        GetProjectile(skillProjectileFlag, this.targetPos, true);
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            slime.DashTime = slime.originDashTime;
            Transform slimePos = slime.transform;

            // ��� �� �����ϴ� ��ġ�� ���� �ִٸ�, �� �տ��� ��ø� ���ߵ���
            // ���ٸ� ������ ������ �����̵�(����)

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

    // ����ü ����
    public virtual void GetProjectile(EProjectileFlag flag, Vector3 targetPos, bool isSkill)
    {
        // ����ü ���� �� ���콺 ������ �ٶ�
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, projectilePos.position, Vector3.zero).GetComponent<StaffProjectile>();
        projectile.isSkill = isSkill;
        projectile.transform.forward = targetPos;

        //LookAtPos(projectile, targetPos);
        MissileRune(projectile);        // ���� ����ü ���� ������ �ִٸ� ���
    }

    protected void LookAtPos(StaffProjectile projectile, Vector3 targetPos)
    {
        projectile.transform.LookAt(targetPos);      // ȭ�� ���� �� ���콺 ������ �ٶ�

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }

    // ���� ����ü ���� ������ �ִٸ� ����� �� �ֵ���
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
