/**
 * @brief Ȱ ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    #region ����
    Vector3 lookRot;

    private float dashDistance = 2f;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
        dashCoolTime = 2f;
    }
    #endregion

    #region �ڷ�ƾ

    #endregion

    #region �Լ�
    // ��Ÿ
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);         // ��Ÿ �ִϸ��̼� ���

        //Arrow arrow = ObjectPoolingManager.Instance.Get(EProjectileFlag.arrow, transform.position, Vector3.zero).GetComponent<Arrow>();
        //if (weaponRuneInfos[0].isActive) arrow.IsPenetrate = true;       // ���� ������ �ִٸ� ���� ȭ��

        //arrow.transform.LookAt(targetPos);      // ȭ�� ���� �� ���콺 ������ �ٶ�

        Arrow arrow = GetProjectile(targetPos);
        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        arrow.transform.eulerAngles = lookRot;
    }

    // ��ų
    protected override void Skill(Vector3 targetPos)
    {
        base.Skill(targetPos);

        // ��ä�÷� ȭ���� �߻�

        float angle = 45;           // ����
        float interval = 10f;       // ����

        for (float y = 180 - angle; y <= 180 + angle; y += interval)
        {
            //Arrow arrow = ObjectPoolingManager.Instance.Get(EProjectileFlag.arrow).GetComponent<Arrow>();
            //if (weaponRuneInfos[0].isActive) arrow.IsPenetrate = true;       // ���� ������ �ִٸ� ���� ȭ��

            //arrow.transform.position = this.transform.position;
            //arrow.transform.LookAt(targetPos);                  // ���콺 Ŭ�� ��ġ�� �ٶ󺸰� �� ����  

            Arrow arrow = GetProjectile(targetPos);
            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.y += y + 180;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;     // ������ ������ ��ä��ó�� ���̵��� ��
        }
    }

    // ����ü(ȭ��) ����
    Arrow GetProjectile(Vector3 targetPos)
    {
        Arrow arrow = ObjectPoolingManager.Instance.Get(EProjectileFlag.arrow, transform.position, Vector3.zero).GetComponent<Arrow>();
        if (weaponRuneInfos[0].isActive) arrow.IsPenetrate = true;       // ���� ������ �ִٸ� ���� ȭ��

        arrow.transform.LookAt(targetPos);      // ȭ�� ���� �� ���콺 ������ �ٶ�

        return arrow;
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            slime.DashDistance = dashDistance;
            slime.Dash();           // �Ϲ� ���
        }

       return canDash;
    }
    #endregion
}
