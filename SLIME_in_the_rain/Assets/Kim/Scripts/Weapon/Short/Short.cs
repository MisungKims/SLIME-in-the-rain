using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region 변수
    Vector3 lookRot;

    protected EProjectileFlag flag;         // 어떤 종류의 투사체인지?

    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack()
    {
        base.AutoAttack();

        DoDamage(false);

        // 검기 발사 룬을 가지고 있을 때 검기 발사
        Missile(targetPos, false);
    }

    // 오브젝트를 공격하면 데미지를 입힘
    protected void DoDamage(bool isSkill)
    {
        Transform slimeTransform = slime.transform;

        // 슬라임의 위치에서 공격 거리만큼 ray를 쏨
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, slime.Stat.attackRange))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            Debug.Log(hit.transform.name);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Damage(hit.transform, isSkill);          // 데미지를 입힘
            }
        }
    }

    // 검기 발사
    protected void Missile(Vector3 targetPos, bool isSkill)
    {
        if (weaponRuneInfos[1].isActive)       
        {
            GameObject projectile = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero);
            projectile.GetComponent<Projectile>().isSkill = isSkill;

            projectile.transform.LookAt(targetPos);      // 검 생성 뒤 마우스 방향을 바라봄

            lookRot = projectile.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.z = 0;

            projectile.transform.eulerAngles = lookRot;
        }
    }

    // 데미지를 입힘
    protected void Damage(Transform hitObj, bool isSkill)
    {
        Debug.Log(hitObj.name);

        IDamage damagedObject = hitObj.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            if (isSkill) damagedObject.SkillDamaged();
            else damagedObject.AutoAtkDamaged();

            if (hitObj.gameObject.layer == 8)
            {
                RuneManager.Instance.UseAttackRune(hitObj.gameObject);
            }
        }
    }
    #endregion
}
