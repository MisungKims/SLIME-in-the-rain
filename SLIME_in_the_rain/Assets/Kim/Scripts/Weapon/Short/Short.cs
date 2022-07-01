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
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        DoDamage();
    }

    // 오브젝트를 공격하면 데미지를 입힘
    protected void DoDamage()
    {
        Transform slimeTransform = slime.transform;

        // 슬라임의 위치에서 공격 거리만큼 ray를 쏨
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, slime.Stat.attackRange))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Damage(hit.transform);          // 데미지를 입힘
            }
        }

        // 검기 발사 룬을 가지고 있을 때 검기 발사
        Missile(slimeTransform.forward);
    }

    // 검기 발사
    protected void Missile(Vector3 targetPos)
    {
        if (weaponRuneInfos[1].isActive)       
        {
            GameObject arrow = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero);
            arrow.transform.LookAt(targetPos);      // 검 생성 뒤 마우스 방향을 바라봄

            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;
        }
    }

    // 데미지를 입힘
    protected void Damage(Transform hitObj)
    {
        Debug.Log(hitObj.name);

        IDamage damagedObject = hitObj.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
        }
    }
    #endregion
}
