/**
 * @brief ���� ���� ��
 * @author ��̼�
 * @date 22-06-30
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneRange : Rune, IPassiveRune
{
    #region �Լ�
    public void Passive()
    {
        // ���� 2��
        statManager.AddAttackRange(statManager.GetIncrementStat("AtkRange", 200));

        // ���� ȿ�� ����
        statManager.AddAttackSpeed(statManager.GetIncrementStat("AtkSpeed", 50) * -1);
    }
    #endregion
}
