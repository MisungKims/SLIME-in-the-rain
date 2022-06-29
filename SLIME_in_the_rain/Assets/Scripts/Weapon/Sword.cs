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
    float dashSpeed = 2.5f;
    float dashDuration = 2.5f;

    // 평타
    private float maxDistance = 1.1f;

    // 스킬
    private float detectRadius = 1.5f;
    private float distance = 2f;
    private float angleRange = 90f;
    Vector3 direction;
    float dotValue = 0f;
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

    // 평타
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        DoDamage();
    }

    // 스킬
    protected override void Skill(Vector3 targetPos)
    {
        base.Skill(targetPos);

        DoSkillDamage();
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

        // 슬라임의 위치에서 공격 거리만큼 ray를 쏨
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, maxDistance))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Damage(hit.transform);          // 데미지를 입힘
            }
        }
    }

    // 부채꼴 범위 안의 적을 판별하여 데미지를 입힘
    void DoSkillDamage()
    {
        Transform slimeTransform = slime.transform;

        // 원 안에 들어온 적들을 구함
        Collider[] colliders = Physics.OverlapSphere(slimeTransform.position, detectRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("DamagedObject"))
            {
                // 그 적들 중 부채꼴 범위 안에 있는 적들에게 데미지를 입힘

                dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));     // 스킬 각도에 대한 코사인값
                direction = colliders[i].transform.position - slimeTransform.position;      // 슬라임에서 타겟을 보는 벡터

                if (direction.magnitude < distance)         // 탐지한 오브젝트와 부채꼴의 중심점의 거리를 비교 
                {
                    // 탐지한 오브젝트가 스킬 각도안에 들어왔으면 데미지
                    if (Vector3.Dot(direction.normalized, slimeTransform.forward) > dotValue)
                    {
                        Damage(colliders[i].transform);
                    }
                }
            }
        }
    }

    // 데미지를 입힘
    void Damage(Transform hitObj)
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
