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

        // ����ü ���� �� ���콺 ������ �ٶ�
        GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, Vector3.zero);
        arrow.transform.LookAt(targetPos);      // ȭ�� ���� �� ���콺 ������ �ٶ�

        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;
    }

    // ��ų
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
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

    /// ���
    //public override void Dash(Slime slime)
    //{
    //    if (isDash)
    //    {
    //        slime.isDash = false;
    //        return;
    //    }

    //    Transform slimePos = slime.transform;

    //    slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // ������ ������ �����̵�(����)

    //    slime.isDash = false;

    //    StartCoroutine(DashTimeCount());
    //}
    #endregion
}
