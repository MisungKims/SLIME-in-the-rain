using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region ����
    Vector3 lookRot;

    protected EProjectileFlag flag;         // � ������ ����ü����?

    #endregion

    #region �Լ�
    // ��Ÿ
    protected override void AutoAttack()
    {
        base.AutoAttack();

        DoDamage(false);

        // �˱� �߻� ���� ������ ���� �� �˱� �߻�
        Missile(targetPos, false);
    }

    // ������Ʈ�� �����ϸ� �������� ����
    protected void DoDamage(bool isSkill)
    {
        Transform slimeTransform = slime.transform;

        // �������� ��ġ���� ���� �Ÿ���ŭ ray�� ��
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, slime.transform.forward, out hit, slime.Stat.attackRange))
        {

#if UNITY_EDITOR
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, slime.transform.forward * 5, Color.red, 0.3f);
#endif

            Debug.Log(hit.transform.name);

            if (hit.transform.CompareTag("DamagedObject"))
            {
                Damage(hit.transform, isSkill);          // �������� ����
            }
        }
    }

    // �˱� �߻�
    protected void Missile(Vector3 targetPos, bool isSkill)
    {
        if (weaponRuneInfos[1].isActive)       
        {
            GameObject projectile = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero);
            projectile.GetComponent<Projectile>().isSkill = isSkill;

            projectile.transform.LookAt(targetPos);      // �� ���� �� ���콺 ������ �ٶ�

            lookRot = projectile.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.z = 0;

            projectile.transform.eulerAngles = lookRot;
        }
    }

    // �������� ����
    protected void Damage(Transform hitObj, bool isSkill)
    {
        Debug.Log(hitObj.name);

        IDamage damagedObject = hitObj.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            if (isSkill) damagedObject.SkillDamaged();
            else damagedObject.AutoAtkDamaged();

            if (hitObj.gameObject.layer == 8)       // �������� ������ ������Ʈ�� ������ �� �� �ߵ�
            {
                RuneManager.Instance.UseAttackRune(hitObj.gameObject);
            }
        }
    }
    #endregion
}
