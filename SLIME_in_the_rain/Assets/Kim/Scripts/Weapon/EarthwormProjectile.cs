using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthwormProjectile : Projectile
{
    public Earthworm earthworm;

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

       if(earthworm) earthworm.DamageSlime(1);
    }
}
