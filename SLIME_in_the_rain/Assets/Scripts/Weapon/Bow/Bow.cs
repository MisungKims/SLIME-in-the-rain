/**
 * @brief Ȱ ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    #region ����
    Vector3 lookRot;
    Vector3 bowPos;
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
        bowPos = transform.position;

        base.AutoAttack(targetPos);

        // ȭ�� ���� �� ���콺 ������ �ٶ�
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, bowPos, Vector3.zero).transform.LookAt(targetPos);
    }

    // ��ų
    public override void Skill(Vector3 targetPos)
    {
        // ��ä�÷� ȭ���� �߻�

        float angle = 45;           // ����
        float interval = 10f;       // ����

        for (float y = 180 - angle; y <= 180 + angle; y += interval)
        {
            GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow);

            arrow.transform.position = this.transform.position;

            arrow.transform.LookAt(targetPos);                  // ���콺 Ŭ�� ��ġ�� �ٶ󺸰� �� ����  

            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.y += y + 180;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;     // ������ ������ ��ä��ó�� ���̵��� ��
        }
    }

    // ���
    public override void Dash(Slime slime)
    {
        if (isDash)
        {
            slime.isDash = false;
            return;
        }

        slime.Dash();           // �Ϲ� ���

        StartCoroutine(DashTimeCount());        // ��� ��Ÿ�� ī��Ʈ
    }
    #endregion
}
