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

    //////// ���
    float dashDistance = 300f;   // ����� �Ÿ�
    float dashTime = 1f;        // ��� ���� ���� �ð�
    float currentDashTime;      // ���� ��� ���ӽð�
    #endregion

    #region ����Ƽ �Լ�

    #endregion

    #region �Լ�

    /// <summary>
    /// ��Ÿ
    /// </summary>
    public override void AutoAttack(Vector3 targetPos)
    {
        // ����ü ���� �� ���콺 ������ �ٶ�
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, projectilePos.position, Vector3.zero).transform.LookAt(targetPos);
    }

    /// <summary>
    /// ��ų
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }

    /// <summary>
    /// ���
    /// </summary>
    public override void Dash(Slime slime)
    {
        Transform slimePos = slime.transform;

        currentDashTime = dashTime;
        if (currentDashTime >= 0)
        {
            slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;

            currentDashTime -= Time.deltaTime;
        }

        slime.isDash = false;
    }
    #endregion
}
