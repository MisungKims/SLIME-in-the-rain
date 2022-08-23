using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaff : Staff
{
    #region ����
    // ��
    //private bool isHaveRune2 = false;
    //public bool IsHaveRune2 { set { isHaveRune = value; } }
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        weaponType = EWeaponType.iceStaff;
        projectileFlag = EProjectileFlag.ice;
        skillProjectileFlag = EProjectileFlag.iceSkill;
        maxDashCoolTime = 5f;
    }

    protected override void Start()
    {
        base.Start();

        UIseting("����������", "�Ķ���", "��������"); //���� ���� ���� //jeon �߰�
    }

    #endregion

    #region �Լ�
    // ����ü ����
    public override void GetProjectile(EProjectileFlag flag, Vector3 targetPos, bool isSkill)
    {
        // ����ü ���� �� ���콺 ������ �ٶ�
        StaffProjectile projectile = ObjectPoolingManager.Instance.Get(flag, projectilePos.position, Vector3.zero).GetComponent<StaffProjectile>();
        projectile.isSkill = isSkill;
        projectile.transform.forward = targetPos;

        MissileRune(projectile);        // ���� ����ü ���� ������ �ִٸ� ���
        StunRune(flag, projectile);     // ���� 2�� ���� ������ �ִٸ� ���

       // LookAtPos(projectile, targetPos);
    }

    // ���� ������ ���� �ִٸ� ���� �ð� 2��
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