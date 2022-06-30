using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaff : Staff
{
    #region ����
    // ��
    private bool isHaveRune2 = false;
    public bool IsHaveRune2 { set { isHaveRune = value; } }
    #endregion


    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.iceStaff;
        projectileFlag = EProjectileFlag.ice;
        skillProjectileFlag = EProjectileFlag.iceSkill;
        dashCoolTime = 5f;
    }
    #endregion

    #region �Լ�
    protected override void UseRune()
    {
        if (!isHaveRune)
        {
            RuneManager.Instance.UseWeaponRune(this);       // �ߵ����� ���� ������� ������ �ִٸ� ����� �ߵ�
        }

        if (!isHaveRune2)
        {
            RuneManager.Instance.UseWeaponRune(this);       // �ߵ����� ���� ������� ������ �ִٸ� ����� �ߵ�
        }
    }

    // ����ü ����
    public override void GetProjectile(EProjectileFlag flag, Vector3 targetPos)
    {
        // ����ü ���� �� ���콺 ������ �ٶ�
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero).GetComponent<StaffProjectile>();

        if (isHaveRune)
        {
            projectile.IsUseRune = true;            // ������ ���� ������ �ִٸ� ����� �� �ֵ��� (����)
            projectile.Target = slime.target;
        }
        if (isHaveRune2)        // ���� ������ �� (���� �ð� 2��)
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

        projectile.transform.LookAt(targetPos);      // ȭ�� ���� �� ���콺 ������ �ٶ�

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }
    #endregion
}
