/**
 * @brief ���� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneVampire : Rune, IAttackRune
{
    #region �Լ�
    public void Attack()
    {
        // TODO : ���� 20%
        float amount = 0f;    //  (monsterStat.HP * percent) * 0.01f;
        statManager.AddHP(amount);
    }
    #endregion
}
