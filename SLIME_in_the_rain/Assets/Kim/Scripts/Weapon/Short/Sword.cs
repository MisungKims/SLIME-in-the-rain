/**
 * @brief 양손검 스크립트
 * @author 김미성
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Short
{
    #region 변수
    // 대시
    float originSpeed;
    float dashSpeed = 2.5f;
    float dashDuration = 2.5f;

    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        weaponType = EWeaponType.sword;
        angle = Vector3.zero;
        maxDashCoolTime = 1f;
        flag = EProjectileFlag.sword;
    }

    protected override void Start()
    {
        base.Start();

        UIseting("양손검", "초록색", "힘껏베기"); //내용 정보 셋팅 //jeon 추가
    }

    #endregion

    #region 코루틴
    // 일정 시간동안 이속이 증가
    IEnumerator IncrementSpeed(Slime slime)
    {
      //  StatManager statManager = slime.statManager;

        originSpeed = statManager.myStats.moveSpeed;
        statManager.myStats.moveSpeed += dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        statManager.myStats.moveSpeed = originSpeed;
    }
    #endregion

    #region 함수
    // 스킬
    protected override void Skill()
    {
        base.Skill();

        DoSkillDamage();

        // 검기 발사 룬을 가지고 있을 때 검기 발사
        Missile(targetPos, true);
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

    // 원 안의 적을 판별하여 데미지를 입힘
    void DoSkillDamage()
    {
        Transform slimeTransform = slime.transform;

        Collider[] colliders = Physics.OverlapSphere(slimeTransform.position, slime.Stat.attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("DamagedObject"))
            {
                Monster monster = colliders[i].GetComponent<Monster>();
                if (monster) StartCoroutine(monster.Jump());            // 몬스터들을 공중 부양시킴

                Damage(colliders[i].transform, true);
            }
        }
    }
    #endregion
}
