/**
 * @brief 단검 스크립트
 * @author 김미성
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    #region 변수
    private float maxDistance = 0.8f;               // 평타 공격 범위
    private float maxDashAttackDistance = 2f;       // 돌진 대시 공격 범위
    private float dashDistance = 2f;

    // 스킬
    private float skillDuration = 5f;        // 스킬 지속시간
    private Material mat;                   // 투명도를 조절할 머터리얼
    private float alpha;
    private float maxAlpha = 1f;
    private float minAlpha = 0.6f;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
    }
    #endregion

    #region 코루틴
    // 은신 스킬 코루틴 (투명도 조절)
    IEnumerator Stealth()
    {
        slime.isStealth = true;
        slimeMat = slime.SkinnedMesh.material;

        // 반투명하게
        alpha = maxAlpha;
        while (alpha >= minAlpha)
        {
            alpha -= Time.deltaTime * 1.5f;

            slimeMat.color = new Color(slimeMat.color.r, slimeMat.color.g, slimeMat.color.b, alpha);

            yield return null;
        }

        yield return new WaitForSeconds(skillDuration);

        // 원래대로
        alpha = slimeMat.color.a;
        while (alpha <= maxAlpha)
        {
            alpha += Time.deltaTime * 1.5f;

            slimeMat.color = new Color(slimeMat.color.r, slimeMat.color.g, slimeMat.color.b, alpha);

            yield return null;
        }

        slime.isStealth = false;
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

        StartCoroutine(Stealth());
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        // 돌진 베기
        if (canDash)
        {
            DoDashDamage();
            slime.DashDistance = dashDistance;
            slime.Dash();           // 일반 대시

            PlayAnim(AnimState.autoAttack);
            StartCoroutine(CheckAnimEnd("AutoAttack"));
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

    // 돌진 대시 시 데미지입힘
    void DoDashDamage()
    {
        // 돌진 시 앞에 있는 모든 것에게 데미지
        Transform slimeTransform = slime.transform;
        RaycastHit[] hits = Physics.RaycastAll(slimeTransform.position, slimeTransform.forward, maxDashAttackDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("DamagedObject"))
            {
                Damage(hits[i].transform);          // 데미지를 입힘
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
