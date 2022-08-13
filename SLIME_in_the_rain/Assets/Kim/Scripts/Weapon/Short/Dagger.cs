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
    private float addDashDistance = 2.5f;

    // ��ų
    private float skillDuration = 5f;        // ��ų ���ӽð�
    private float alpha;
    private float maxAlpha = 1f;
    private float minAlpha = 0.6f;

    // ���� ����
    private float detectRadius = 1f;
    #endregion

    public Slime slime2; // ���ֿ� �����

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        maxDashCoolTime = 0.5f;
        flag = EProjectileFlag.dagger;
    }

    protected override void Start()
    {
        base.Start();

        UIseting("�ܰ�", "ȸ��", "����"); //���� ���� ���� //jeon �߰�
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
        slime.DashDistance = slime.originDashDistance + addDashDistance;
        slime.Dash();           // �Ϲ� ���

        yield return new WaitForSeconds(0.07f);        // ��ð� ���� ������ ���

        // ��� �� ����
        PlayAnim(AnimState.autoAttack);
        StartCoroutine(CheckAnimEnd("AutoAttack"));
        DoDashDamage(false);
    }
    #endregion

    #region �Լ�
    // ��ų
    protected override void Skill()
    {
        //RuneManager.Instance.UseAttackRune();
        RuneManager.Instance.UseSkillRune();

        StartCoroutine(CheckAnimEnd("Skill"));

        StartCoroutine(SkillTimeCount());

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
    void DoDashDamage(bool isSkill)
    {
        Transform slimeTransform = slime.transform;

        // �� �ȿ� �ִ� ������ ����
        Collider[] colliders = Physics.OverlapSphere(slimeTransform.position, detectRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("DamagedObject"))
            {
                Damage(colliders[i].transform, isSkill);
            }
        }
    }
    #endregion

#if UNITY_EDITOR
    //����Ƽ �����Ϳ� ��ä���� �׷��� �޼ҵ�
    //private void OnDrawGizmos()
    //{
    //    Transform slimeTransform = slime2.transform;

    //    Handles.color = new Color(0f, 0f, 1f, 0.2f);
    //    // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
    //    Handles.DrawSolidArc(slimeTransform.position, Vector3.up, slimeTransform.forward, angleRange / 2, detectRadius);
    //    Handles.DrawSolidArc(slimeTransform.position, Vector3.up, slimeTransform.forward, -angleRange / 2, detectRadius);
    //}

    //void OnDrawGizmosSelected()
    //{
    //    Transform slimeTransform = slime2.transform;

    //    Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
    //    Gizmos.DrawSphere(slimeTransform.position, detectRadius);
    //}
}
#endif

