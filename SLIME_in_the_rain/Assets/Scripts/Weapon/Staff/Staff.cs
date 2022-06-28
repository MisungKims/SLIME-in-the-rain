/**
 * @brief 불 지팡이 스크립트
 * @author 김미성
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    #region 변수
    [SerializeField]
    private Transform projectilePos;        // 생성될 투사체의 위치

    Vector3 lookRot;

    //////// 대시
    float dashDistance = 400f;   // 대시할 거리
    float currentDashTime;      // 현재 대시 지속시간
    #endregion

    #region 유니티 함수

    #endregion

    #region 코루틴

    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        // 투사체 생성 뒤 마우스 방향을 바라봄
        GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, Vector3.zero);
        arrow.transform.LookAt(targetPos);      // 화살 생성 뒤 마우스 방향을 바라봄

        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;
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

        if (canDash)
        {
            Transform slimePos = slime.transform;

            slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // 정해진 곳으로 순간이동(점멸)

            slime.isDash = false;
        }
        return canDash;
    }

    /// 대시
    //public override void Dash(Slime slime)
    //{
    //    if (isDash)
    //    {
    //        slime.isDash = false;
    //        return;
    //    }

    //    Transform slimePos = slime.transform;

    //    slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // 정해진 곳으로 순간이동(점멸)

    //    slime.isDash = false;

    //    StartCoroutine(DashTimeCount());
    //}
    #endregion
}
