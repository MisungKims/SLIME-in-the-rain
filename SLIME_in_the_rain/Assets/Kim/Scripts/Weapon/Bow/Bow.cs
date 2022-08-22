/**
 * @brief 활 스크립트
 * @author 김미성
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    #region 변수
    Vector3 lookRot;

    private float addDashDistance = 3f;
    private float addDashTime = 0.05f;
    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
        maxDashCoolTime = 2f;
    }

    protected override void Start()
    {
        base.Start();

        UIseting("활", "노란색", "애쉬w"); //내용 정보 셋팅 //jeon 추가
    }

    #endregion

    #region 코루틴

    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack()
    {
        base.AutoAttack();         // 평타 애니메이션 재생

        Arrow arrow = GetProjectile(this.targetPos);

        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        arrow.transform.eulerAngles = lookRot;
    }

    // 스킬
    protected override void Skill()
    {
        base.Skill();

        // 부채꼴로 화살을 발사

        float angle = 45;           // 각도
        float interval = 3f;       // 간격

        for (float y = 180 - angle; y <= 180 + angle; y += interval)
        {
            Arrow arrow = GetProjectile(this.targetPos);
            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.y += y + 180;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;     // 각도를 조절해 부채꼴처럼 보이도록 함
        }
    }

    // 투사체(화살) 생성
    Arrow GetProjectile(Vector3 targetPos)
    {
        Arrow arrow = ObjectPoolingManager.Instance.Get(EProjectileFlag.arrow, transform.position, Vector3.zero).GetComponent<Arrow>();
        if (weaponRuneInfos[0].isActive) arrow.IsPenetrate = true;       // 룬을 가지고 있다면 관통 화살

        arrow.transform.forward = this.targetPos;        // 화살 생성 뒤 마우스 방향을 바라봄

        return arrow;
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            slime.DashTime = slime.originDashTime + addDashTime;
            slime.DashDistance = slime.originDashDistance + addDashDistance;
            slime.Dash();           // 일반 대시
        }

       return canDash;
    }
    #endregion
}
