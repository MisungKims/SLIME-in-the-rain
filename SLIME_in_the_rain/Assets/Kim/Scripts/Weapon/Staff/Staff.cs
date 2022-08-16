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
    float originDashDistance = 400f;   // ����� �Ÿ�
    float dashDistance = 400f;   // ����� �Ÿ�
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
            dashDistance = originDashDistance;

            Transform slimePos = slime.transform;

            if (Physics.Raycast(slimePos.position + Vector3.up * dashDistance, slime.transform.forward, out RaycastHit hit, 0.7f))
            {
                if (hit.transform.gameObject.layer == 11)
                {
                    
                }
            }

            slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // ������ ������ �����̵�(����)

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
