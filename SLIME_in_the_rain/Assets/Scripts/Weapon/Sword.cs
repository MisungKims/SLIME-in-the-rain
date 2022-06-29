/**
 * @brief 양손검 스크립트
 * @author 김미성
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    #region 변수


    // 대시
    float originSpeed;
    float dashSpeed = 3f;
    float dashDuration = 1.5f;

    // 공격
    private float maxDistance = 1.1f;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.sword;
        angle = Vector3.zero;
        dashCoolTime = 1f;
    }
    #endregion

    #region 코루틴
    // 일정 시간동안 이속이 증가
    IEnumerator IncrementSpeed(Slime slime)
    {
        StatManager statManager = slime.statManager;

        originSpeed = statManager.myStats.moveSpeed;
        statManager.myStats.moveSpeed += dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        statManager.myStats.moveSpeed = originSpeed;
    }
    #endregion

    #region 함수

    /// <summary>
    /// 평타
    /// </summary>
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        DoDamage();
    }

    /// <summary>
    /// 스킬
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            StartCoroutine(IncrementSpeed(slime));                  // 이속 증가
            slime.isDash = false;
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
    #endregion

}
