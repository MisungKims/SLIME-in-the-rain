/**
 * @brief 단검 스크립트
 * @author 김미성
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    #region 변수
    private float maxDistance = 0.8f;
    private float maxDashAttackDistance = 2f;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
    }
    #endregion

    #region 함수

    // 평타
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        DoDamage();
    }

    // 스킬
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        // 돌진 베기
        if (canDash)
        {
            DoDashDamage();
            slime.Dash();           // 일반 대시

            PlayAnim(AnimState.autoAttack);
            StartCoroutine(CheckAnimEnd("AutoAttack"));

            
        }

        return canDash;
    }

    // 오브젝트를 공격하면 데미지를 입힘
    void DoDamage()
    {
        Transform slimeTransform = slime.transform;

        // 슬라임의 위치에서 공격 범위만큼 ray를 쏨
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, maxDistance))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Debug.Log(hit.transform.name);

                // 데미지를 입힘
                IDamage damagedObject = hit.transform.GetComponent<IDamage>();
                if (damagedObject != null)
                {
                    damagedObject.Damaged();
                }
            }
        }
    }

    void DoDashDamage()
    {
        Transform slimeTransform = slime.transform;
        RaycastHit[] hits = Physics.RaycastAll(slimeTransform.position, slimeTransform.forward, maxDashAttackDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("DamagedObject"))
            {
                Debug.Log(hits[i].transform.name);

                // 데미지를 입힘
                IDamage damagedObject = hits[i].transform.GetComponent<IDamage>();
                if (damagedObject != null)
                {
                    damagedObject.Damaged();
                }
            }
        }
    }
    #endregion
}
