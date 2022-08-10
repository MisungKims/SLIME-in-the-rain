/**
 * @brief ��հ� ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Short
{
    #region ����
    // ���
    float originSpeed;
    float dashSpeed = 2.5f;
    float dashDuration = 2.5f;

    #endregion

    #region ����Ƽ �Լ�
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

        UIseting("��հ�", "�ʷϻ�", "��������"); //���� ���� ���� //jeon �߰�
    }

    #endregion

    #region �ڷ�ƾ
    // ���� �ð����� �̼��� ����
    IEnumerator IncrementSpeed(Slime slime)
    {
      //  StatManager statManager = slime.statManager;

        originSpeed = statManager.myStats.moveSpeed;
        statManager.myStats.moveSpeed += dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        statManager.myStats.moveSpeed = originSpeed;
    }
    #endregion

    #region �Լ�
    // ��ų
    protected override void Skill()
    {
        base.Skill();

        DoSkillDamage();

        // �˱� �߻� ���� ������ ���� �� �˱� �߻�
        Missile(targetPos, true);
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

    // �� ���� ���� �Ǻ��Ͽ� �������� ����
    void DoSkillDamage()
    {
        Transform slimeTransform = slime.transform;

        Collider[] colliders = Physics.OverlapSphere(slimeTransform.position, slime.Stat.attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("DamagedObject"))
            {
                Monster monster = colliders[i].GetComponent<Monster>();
                if (monster) StartCoroutine(monster.Jump());            // ���͵��� ���� �ξ��Ŵ

                Damage(colliders[i].transform, true);
            }
        }
    }
    #endregion
}
