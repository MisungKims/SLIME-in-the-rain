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
    private float maxDistance = 0.8f;
    private float maxDashAttackDistance = 2f;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.dagger;
        angle = new Vector3(90f, 0, 90f);
        dashCoolTime = 0.5f;
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
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        // ���� ����
        if (canDash)
        {
            DoDashDamage();
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

        // �������� ��ġ���� ���� ������ŭ ray�� ��
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, maxDistance))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Debug.Log(hit.transform.name);

                // �������� ����
                IDamage damagedObject = hit.transform.GetComponent<IDamage>();
                if (damagedObject != null)
                {
                    damagedObject.Damaged();
                }
            }
        }
    }

    void DoDashDamage()
    {
        Transform slimeTransform = slime.transform;
        RaycastHit[] hits = Physics.RaycastAll(slimeTransform.position, slimeTransform.forward, maxDashAttackDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("DamagedObject"))
            {
                Debug.Log(hits[i].transform.name);

                // �������� ����
                IDamage damagedObject = hits[i].transform.GetComponent<IDamage>();
                if (damagedObject != null)
                {
                    damagedObject.Damaged();
                }
            }
        }
    }
    #endregion
}
