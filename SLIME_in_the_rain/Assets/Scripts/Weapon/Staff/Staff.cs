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

    Quaternion from = Quaternion.Euler(new Vector3(90, 0, 0));
    Quaternion to = Quaternion.Euler(new Vector3(0, 0, 0));

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
    public override void AutoAttack(Vector3 targetPos)
    {
        // 투사체 생성 뒤 마우스 방향을 바라봄
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, projectilePos.position, Vector3.zero).transform.LookAt(targetPos);
    }

    // 스킬
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }


    /// 대시
    public override void Dash(Slime slime)
    {
        if (isDash)
        {
            slime.isDash = false;
            return;
        }

        Transform slimePos = slime.transform;

        slimePos.position += slimePos.forward * dashDistance * Time.deltaTime;      // 정해진 곳으로 순간이동(점멸)

        slime.isDash = false;

        StartCoroutine(DashTimeCount());
    }
    #endregion
}
