/**
 * @brief 활 스크립트
 * @author 김미성
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    #region 변수
    Vector3 lookRot;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
        dashCoolTime = 2f;
    }
    #endregion

    #region 코루틴

    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);         // 평타 애니메이션 재생

        GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, Vector3.zero);
        arrow.transform.LookAt(targetPos);      // 화살 생성 뒤 마우스 방향을 바라봄

        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;
    }

    // 스킬
    public override void Skill(Vector3 targetPos)
    {
        // 부채꼴로 화살을 발사

        float angle = 45;           // 각도
        float interval = 10f;       // 간격

        for (float y = 180 - angle; y <= 180 + angle; y += interval)
        {
            GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow);

            arrow.transform.position = this.transform.position;

            arrow.transform.LookAt(targetPos);                  // 마우스 클릭 위치로 바라보게 한 다음  

            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.y += y + 180;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;     // 각도를 조절해 부채꼴처럼 보이도록 함
        }
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash) slime.Dash();           // 일반 대시

       return canDash;
    }

    // 대시
    //public override bool Dash(Slime slime)
    //{
    //    if (isDash)
    //    {
    //        slime.isDash = false;
    //        return;
    //    }

    //    slime.Dash();           // 일반 대시

    //    StartCoroutine(DashTimeCount());        // 대시 쿨타임 카운트
    //}
    #endregion
}
