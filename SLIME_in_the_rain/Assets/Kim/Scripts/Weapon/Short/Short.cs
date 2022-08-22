/**
 * @brief �ܰŸ� ����
 * @author ��̼�
 * @date 22-08-15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region ����
    Vector3 lookRot;

    protected EProjectileFlag flag;         // � ������ ����ü����?

    [SerializeField]
    float size = 2f;

    #endregion

    #region �Լ�
    // ��Ÿ
    protected override void AutoAttack()
    {
        base.AutoAttack();

        DoDamage(false);

        // �˱� �߻� ���� ������ ���� �� �˱� �߻�
        Missile(targetPos, false, EProjectileFlag.slash);
    }

    // ������Ʈ�� �����ϸ� �������� ����
    protected void DoDamage(bool isSkill)
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + Vector3.up * 0.1f, slime.transform.lossyScale * size, transform.forward, slime.transform.rotation, slime.Stat.attackRange);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("DamagedObject"))
            {
                Debug.Log(hits[i].transform.name);

                Damage(hits[i].transform, isSkill);          // �������� ����
            }
        }
    }

    // �˱� �߻�
    protected void Missile(Vector3 targetPos, bool isSkill, EProjectileFlag _flag)
    {
        if (weaponRuneInfos[1].isActive)       
        {
            GameObject projectile = ObjectPoolingManager.Instance.Get(_flag, transform.position, Vector3.zero);
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

//#if UNITY_EDITOR
//    private Color _rayColor = Color.red;

//    void OnDrawGizmos()
//    {
//        Gizmos.color = _rayColor;

//        // �Լ� �Ķ���� : ���� ��ġ, Box�� ���� ������, Ray�� ����, RaycastHit ���, Box�� ȸ����, BoxCast�� ������ �Ÿ�
//        if (true == Physics.BoxCast(transform.position + Vector3.up * 0.1f, slime.transform.lossyScale * size, slime.transform.forward, out RaycastHit hit, slime.transform.rotation, slime.Stat.attackRange))
//        {
//            // Hit�� �������� ray�� �׷��ش�.
//            Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, slime.transform.forward * hit.distance);

//            // Hit�� ������ �ڽ��� �׷��ش�.
//            Gizmos.DrawWireCube(transform.position + Vector3.up * 0.1f + slime.transform.forward * hit.distance, slime.transform.lossyScale * size);
//        }
//        else
//        {
//            // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
//            Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, slime.transform.forward * slime.Stat.attackRange);
//        }
//    }
//#endif
}
