/**
 * @brief ��հ� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    #region ����
    // ���
    float originSpeed;
    float dashSpeed = 2.5f;
    float dashDuration = 2.5f;

    // ����
    private float maxDistance = 1.1f;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.sword;
        angle = Vector3.zero;
        dashCoolTime = 1f;
    }
    #endregion

    #region �ڷ�ƾ
    // ���� �ð����� �̼��� ����
    IEnumerator IncrementSpeed(Slime slime)
    {
        StatManager statManager = slime.statManager;

        originSpeed = statManager.myStats.moveSpeed;
        statManager.myStats.moveSpeed += dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        statManager.myStats.moveSpeed = originSpeed;
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

        DoSkillDamage();
    }

    // ���
    public override bool Dash(Slime slime)
    {
        bool canDash = base.Dash(slime);

        if (canDash)
        {
            StartCoroutine(IncrementSpeed(slime));                  // �̼� ����
            slime.isDash = false;
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

    // ��ų �� ����������
    void DoSkillDamage()
    {
        // ��ä�� ����
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
