/**
 * @brief �ܰ� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    #region ����

    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
    }
    #endregion

    #region �Լ�

    /// <summary>
    /// ��Ÿ
    /// </summary>
    public override void AutoAttack(Vector3 targetPos)
    {
        Debug.Log("AutoAttack");
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
        Debug.Log("Dash");
    }
    #endregion
}
