/**
 * @brief �ܰ� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    #region ����
    private float maxDistance = 0.8f;               // ��Ÿ ���� ����
    private float maxDashAttackDistance = 2f;       // ���� ��� ���� ����
    private float dashDistance = 2f;

    // ��ų
    private float skillDuration = 5f;        // ��ų ���ӽð�
    private Material mat;                   // ������ ������ ���͸���
    private float alpha;
    private float maxAlpha = 1f;
    private float minAlpha = 0.6f;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
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
    #endregion

    #region �Լ�

    // ��Ÿ
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        DoDamage();
    }

    // ��ų
    protected override void Skill(Vector3 targetPos)
    {
        base.Skill(targetPos);

        StartCoroutine(Stealth());
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        // ���� ����
        if (canDash)
        {
            DoDashDamage();
            slime.DashDistance = dashDistance;
            slime.Dash();           // �Ϲ� ���

            PlayAnim(AnimState.autoAttack);
            StartCoroutine(CheckAnimEnd("AutoAttack"));
        }

        return canDash;
    }

    // ������Ʈ�� �����ϸ� �������� ����
    void DoDamage()
    {
        Transform slimeTransform = slime.transform;

        // �������� ��ġ���� ���� �Ÿ���ŭ ray�� ��
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, maxDistance))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Damage(hit.transform);          // �������� ����
            }
        }
    }

    // ���� ��� �� ����������
    void DoDashDamage()
    {
        // ���� �� �տ� �ִ� ��� �Ϳ��� ������
        Transform slimeTransform = slime.transform;
        RaycastHit[] hits = Physics.RaycastAll(slimeTransform.position, slimeTransform.forward, maxDashAttackDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("DamagedObject"))
            {
                Damage(hits[i].transform);          // �������� ����
            }
        }
    }

    // �������� ����
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
