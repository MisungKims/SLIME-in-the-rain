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

    //// ������ �������� ����
    //protected override IEnumerator Chase()
    //{
    //    while (target && isChasing && !isStun)
    //    {
    //        // ������ ���� ���� �ȿ� �������� �ִٸ� ���� ����
    //        atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
    //        if (atkRangeColliders.Length > 0 && !isAttacking)
    //        {
    //            StartCoroutine(Attack());
    //        }
    //        else if (atkRangeColliders.Length <= 0)
    //        {
    //            // �������� �Ѿƴٴ�
    //            nav.SetDestination(target.position);

    //            if(!doDamaged) IsAttacking = false;
    //            PlayAnim(EMonsterAnim.run);
    //        }

    //        yield return null;
    //    }
    //}

    public override void DamageSlime(int atkType)
    {
        base.DamageSlime(atkType);
        Debug.Log(atkType);

       if(atkType == 1) slime.Stun(stunTime);
    }
}
