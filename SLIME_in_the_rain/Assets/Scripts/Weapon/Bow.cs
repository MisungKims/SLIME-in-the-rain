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

    #endregion

    #region ����Ƽ �Լ�

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
