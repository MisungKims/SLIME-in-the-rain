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

    Quaternion from = Quaternion.Euler(new Vector3(90, 0, 0));
    Quaternion to = Quaternion.Euler(new Vector3(0, 0, 0));

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
    public override void AutoAttack(Vector3 targetPos)
    {
        // ����ü ���� �� ���콺 ������ �ٶ�
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, projectilePos.position, Vector3.zero).transform.LookAt(targetPos);
    }

    // ��ų
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }


    /// ���
    public override void Dash(Slime slime)
    {
        if (isDash)
        {
            slime.isDash = false;
            return;
        }

        Transform slimePos = slime.transform;

        slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // ������ ������ �����̵�(����)

        slime.isDash = false;

        StartCoroutine(DashTimeCount());
    }
    #endregion
}
