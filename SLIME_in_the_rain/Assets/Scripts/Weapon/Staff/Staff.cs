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
    private Transform projectilePos;
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
    public override void Dash()
    {
        Debug.Log("Dash");
    }
    #endregion
}
