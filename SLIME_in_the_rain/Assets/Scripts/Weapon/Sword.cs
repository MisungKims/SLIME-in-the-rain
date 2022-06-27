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
    float originSpeed;
    float dashSpeed = 3f;
    float dashDuration = 1.5f;
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

    /// <summary>
    /// ��Ÿ
    /// </summary>
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        Debug.Log("AutoAttack");
    }

    /// <summary>
    /// ��ų
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        Debug.Log("Skill");
    }


    // ���
    public override void Dash(Slime slime)
    {
        if (isDash)
        {
            slime.isDash = false;
            return;
        }

        StartCoroutine(IncrementSpeed(slime));                  // �̼� ����

        StartCoroutine(DashTimeCount());        // ��� ��Ÿ�� ī��Ʈ

        slime.isDash = false;
    }
    #endregion

}
