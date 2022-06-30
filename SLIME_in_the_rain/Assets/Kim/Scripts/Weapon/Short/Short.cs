using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region ����
    // ��
    private bool isHaveRune2 = false;
    public bool IsHaveRune2 { set { isHaveRune = value; } }
    #endregion

    #region �Լ�
    protected override void UseRune()
    {
        if (!isHaveRune)
        {
            RuneManager.Instance.UseWeaponRune(this);       // �ߵ����� ���� ������� ������ �ִٸ� ����� �ߵ�
        }

        if (!isHaveRune2)
        {
            RuneManager.Instance.UseWeaponRune(this);       // �ߵ����� ���� ������� ������ �ִٸ� ����� �ߵ�
        }
    }

    // ��Ÿ
    protected override void AutoAttack(Vector3 targetPos)
    {
        base.AutoAttack(targetPos);

        DoDamage();
    }

    // ������Ʈ�� �����ϸ� �������� ����
    protected void DoDamage()
    {
        Transform slimeTransform = slime.transform;

        // �������� ��ġ���� ���� �Ÿ���ŭ ray�� ��
        RaycastHit hit;
        if (Physics.Raycast(slimeTransform.position, slimeTransform.forward, out hit, slime.Stat.attackRange))
        {
            //Debug.DrawRay(slime.transform.position, slime.transform.forward * hit.distance, Color.red);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Damage(hit.transform);          // �������� ����
            }
        }
    }

    // �������� ����
    protected void Damage(Transform hitObj)
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
