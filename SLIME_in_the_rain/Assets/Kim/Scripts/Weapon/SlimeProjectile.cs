using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : Projectile
{
    Monster monster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slime"))
        {
            DoDamage(other, isSkill);
        }
    }

    // 데미지를 입힘
    protected override void DoDamage(Collider other, bool isSkill)
    {
        ObjectPoolingManager.Instance.Set(this.gameObject, flag);

        monster.DamageSlime(1);
    }
}
