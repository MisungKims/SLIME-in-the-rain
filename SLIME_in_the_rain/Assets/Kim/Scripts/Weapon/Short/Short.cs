using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region 변수
    // 룬
    private bool isHaveRune2 = false;
    public bool IsHaveRune2 { set { isHaveRune = value; } }
    #endregion

    #region 함수
    protected override void UseRune()
    {
        if (!isHaveRune)
        {
            RuneManager.Instance.UseWeaponRune(this);       // 발동되지 않은 무기룬을 가지고 있다면 무기룬 발동
        }

        if (!isHaveRune2)
        {
            RuneManager.Instance.UseWeaponRune(this);       // 발동되지 않은 무기룬을 가지고 있다면 무기룬 발동
        }
    }

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
