/**
 * @brief ������ ����ü
 * @author ��̼�
 * @date 22-07-19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : Projectile
{
    [HideInInspector]
    public Monster monster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slime"))
        {
            StartCoroutine(CameraShake.StartShake(0.1f, 0.08f));

            DoDamage(other, isSkill);
        }
    }

    // �������� ����
    protected override void DoDamage(Collider other, bool isSkill)
    {
        ObjectPoolingManager.Instance.Set(this.gameObject, flag);

        if (monster)
        {
            monster.DamageSlime(monster.projectileAtk);
        }
    }
}
