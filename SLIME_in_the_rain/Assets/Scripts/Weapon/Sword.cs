/**
 * @brief ��հ� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    #region ����

    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.sword;
    }
    #endregion

    #region �Լ�

    /// <summary>
    /// ��Ÿ
    /// </summary>
    public override void AutoAttack()
    {
        Debug.Log("AutoAttack");
    }

    /// <summary>
    /// ��ų
    /// </summary>
    public override void Skill()
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
