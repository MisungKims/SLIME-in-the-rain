using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Projectile
{
    // µ•πÃ¡ˆ∏¶ ¿‘»˚
    protected override void DoDamage(Collider other, bool isSkill)
    {

            HideProjectile(other);

        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            if (isSkill) damagedObject.SkillDamaged();
            else damagedObject.AutoAtkDamaged();

            // »Ì«˜ ∑È
            if (other.gameObject.layer == 8)
            {
                RuneManager.Instance.UseAttackRune(other.gameObject);
            }
        }
    }
}
