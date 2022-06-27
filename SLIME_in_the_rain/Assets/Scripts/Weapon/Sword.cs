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

    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.sword;
        angle = Vector3.zero;
    }
    #endregion

    #region 함수

    /// <summary>
    /// 평타
    /// </summary>
    public override void AutoAttack(Vector3 targetPos)
    {
        Debug.Log("AutoAttack");
    }

    /// <summary>
    /// 스킬
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }

    /// <summary>
    /// 대시
    /// </summary>
    public override void Dash(Slime slime)
    {
        Debug.Log("Dash");
    }
    #endregion

}
