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
    private Transform projectilePos;
    #endregion

    #region 유니티 함수

    #endregion

    #region 함수

    /// <summary>
    /// 평타
    /// </summary>
    public override void AutoAttack(Vector3 targetPos)
    {
        // 투사체 생성 뒤 마우스 방향을 바라봄
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, projectilePos.position, Vector3.zero).transform.LookAt(targetPos);
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
    public override void Dash()
    {
        Debug.Log("Dash");
    }
    #endregion
}
