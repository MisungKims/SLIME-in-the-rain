/**
 * @brief �ܰ� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

public class Dagger : Short
{
    #region ����
    //private float maxDistance = 0.8f;               // ��Ÿ ���� ����
    private float dashDistance = 2f;

    // ��ų
    private float skillDuration = 5f;        // ��ų ���ӽð�
    private Material mat;                   // ������ ������ ���͸���
    private float alpha;
    private float maxAlpha = 1f;
    private float minAlpha = 0.6f;

    // ���� ����
    private float detectRadius = 0.6f;
    #endregion

    public Slime slime2; // ���ֿ� �����

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
        flag = EProjectileFlag.dagger;
    }
    #endregion

    #region �ڷ�ƾ
    // ���� ��ų �ڷ�ƾ (���� ����)
    IEnumerator Stealth()
    {
        slime.isStealth = true;
        slimeMat = slime.SkinnedMesh.material;

        // �������ϰ�
        alpha = maxAlpha;
        while (alpha >= minAlpha)
        {
            alpha -= Time.deltaTime * 1.5f;

            slimeMat.color = new Color(slimeMat.color.r, slimeMat.color.g, slimeMat.color.b, alpha);

            yield return null;
        }

        yield return new WaitForSeconds(skillDuration);

        // �������
        alpha = slimeMat.color.a;
        while (alpha <= maxAlpha)
        {
            alpha += Time.deltaTime * 1.5f;

            slimeMat.color = new Color(slimeMat.color.r, slimeMat.color.g, slimeMat.color.b, alpha);

            yield return null;
        }

        slime.isStealth = false;
    }

    // ��� �ڷ�ƾ
    IEnumerator DashCorutine()
    {
        slime.DashDistance = dashDistance;
        slime.Dash();           // �Ϲ� ���

        yield return new WaitForSeconds(0.07f);        // ��ð� ���� ������ ���

        // ��� �� ����
        PlayAnim(AnimState.autoAttack);
        StartCoroutine(CheckAnimEnd("AutoAttack"));
        DoDashDamage();
    }
    #endregion

    #region �Լ�
    // ��ų
    protected override void Skill(Vector3 targetPos)
    {
        //base.Skill(targetPos);

        StartCoroutine(Stealth());
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        // ���� ����
        if (canDash) StartCoroutine(DashCorutine());

        return canDash;
    }

    // ���� ��� �� ����������
    void DoDashDamage()
    {
        Transform slimeTransform = slime.transform;

        // �� �ȿ� �ִ� ������ ����
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
    // ����Ƽ �����Ϳ� ��ä���� �׷��� �޼ҵ�
    //private void OnDrawGizmos()
    //{
    //    Transform slimeTransform = slime2.transform;

    //    Handles.color = new Color(0f, 0f, 1f, 0.2f);
    //    // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
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
