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
    float originSpeed;
    float dashSpeed = 3f;
    float dashDuration = 1.5f;
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

        Debug.Log("AutoAttack");
    }

    /// <summary>
    /// 스킬
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }


    // 대시
    public override void Dash(Slime slime)
    {
        if (isDash)
        {
            slime.isDash = false;
            return;
        }

        StartCoroutine(IncrementSpeed(slime));                  // 이속 증가

        StartCoroutine(DashTimeCount());        // 대시 쿨타임 카운트

        slime.isDash = false;
    }
    #endregion

}
