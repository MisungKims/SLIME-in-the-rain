using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region ����
    // ��
    protected bool isHaveRune2 = false;
    public bool IsHaveRune2 { set { isHaveRune = value; } }

    Vector3 lookRot;
    protected EProjectileFlag flag;
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

        if (isHaveRune2)        // �˱� �߻� ���� ������ ���� ��
        {
            Missile(slimeTransform.forward);
        }
    }

    // �˱� �߻�
    protected void Missile(Vector3 targetPos)
    {
        GameObject arrow = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero);
        arrow.transform.LookAt(targetPos);      // �� ���� �� ���콺 ������ �ٶ�

        lookRot = arrow.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        arrow.transform.eulerAngles = lookRot;
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
