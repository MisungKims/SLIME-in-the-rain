/**
 * @brief ��ũ ����
 * @author ��̼�
 * @date 22-07-12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Boss
{
    private float stunTime = 2f;        // �������� ������ �ð�

    // �����ӿ��� �������� ����
    public override void DamageSlime(int atkType)
    {
        base.DamageSlime(atkType);
        Debug.Log(atkType);

       if(atkType == 1) slime.Stun(stunTime);       // �ι�° ������ ����
    }
}
