/**
 * @brief 단검 스크립트
 * @author 김미성
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

public class Dagger : Short
{
    #region 변수
    //private float maxDistance = 0.8f;               // 평타 공격 범위
    private float dashDistance = 2f;

    // 스킬
    private float skillDuration = 5f;        // 스킬 지속시간
    private Material mat;                   // 투명도를 조절할 머터리얼
    private float alpha;
    private float maxAlpha = 1f;
    private float minAlpha = 0.6f;

    // 돌진 베기
    private float detectRadius = 0.6f;
    #endregion

    public Slime slime2; // 나주엥 지우기

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
        flag = EProjectileFlag.dagger;
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

    // 대시 코루틴
    IEnumerator DashCorutine()
    {
        slime.DashDistance = dashDistance;
        slime.Dash();           // 일반 대시

        yield return new WaitForSeconds(0.07f);        // 대시가 끝날 때까지 대기

        // 대시 후 공격
        PlayAnim(AnimState.autoAttack);
        StartCoroutine(CheckAnimEnd("AutoAttack"));
        DoDashDamage();
    }
    #endregion

    #region 함수
    // 스킬
    protected override void Skill(Vector3 targetPos)
    {
        //base.Skill(targetPos);

        StartCoroutine(Stealth());
    }

    // 대시
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        // 돌진 베기
        if (canDash) StartCoroutine(DashCorutine());

        return canDash;
    }

    // 돌진 대시 시 데미지입힘
    void DoDashDamage()
    {
        Transform slimeTransform = slime.transform;

        // 원 안에 있는 적들을 감지
        Collider[] colliders = Physics.OverlapSphere(slimeTransform.position, detectRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("DamagedObject"))
            {
                Damage(colliders[i].transform);
            }
        }
    }
    #endregion
}
    // 유니티 에디터에 부채꼴을 그려줄 메소드
    //private void OnDrawGizmos()
    //{
    //    Transform slimeTransform = slime2.transform;

    //    Handles.color = new Color(0f, 0f, 1f, 0.2f);
    //    // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
    //    Handles.DrawSolidArc(slimeTransform.position, Vector3.up, slimeTransform.forward, angleRange / 2, detectRadius);
    //    Handles.DrawSolidArc(slimeTransform.position, Vector3.up, slimeTransform.forward, -angleRange / 2, detectRadius);
    //}

//    void OnDrawGizmosSelected()
//    {
//        Transform slimeTransform = slime2.transform;

//        Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
//        Gizmos.DrawSphere(slimeTransform.position, detectRadius);
//    }
//}
