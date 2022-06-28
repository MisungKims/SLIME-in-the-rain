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
    private Transform projectilePos;        // ������ ����ü�� ��ġ

    Vector3 lookRot;

    protected EObjectFlag projectileFlag;     // ������ ����ü�� flag
    protected EObjectFlag skillProjectileFlag;     // ������ ����ü�� flag

    //////// ���
    float dashDistance = 400f;   // ����� �Ÿ�
    float currentDashTime;      // ���� ��� ���ӽð�
    #endregion

    #region ����Ƽ �Լ�

    #endregion

    #region �ڷ�ƾ

    #endregion

    #region �Լ�
    // ��Ÿ
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        GetProjectile(projectileFlag, targetPos);
    }

    // ��ų
    protected override void Skill(Vector3 targetPos)
    {
        base.Skill(targetPos);

        GetProjectile(skillProjectileFlag, targetPos);
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            Transform slimePos = slime.transform;

            slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // ������ ������ �����̵�(����)

            slime.isDash = false;
        }
        return canDash;
    }

    // ����ü ����
    public void GetProjectile(EObjectFlag flag, Vector3 targetPos)
    {
        // ����ü ���� �� ���콺 ������ �ٶ�
        GameObject arrow = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero);
        arrow.transform.LookAt(targetPos);      // ȭ�� ���� �� ���콺 ������ �ٶ�

        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        arrow.transform.eulerAngles = lookRot;
    }
    #endregion
}
