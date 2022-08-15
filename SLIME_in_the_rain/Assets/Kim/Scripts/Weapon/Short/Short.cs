/**
 * @brief 단거리 무기
 * @author 김미성
 * @date 22-08-15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short : Weapon
{
    #region 변수
    Vector3 lookRot;

    protected EProjectileFlag flag;         // 어떤 종류의 투사체인지?

    #endregion

    #region 함수
    // 평타
    protected override void AutoAttack()
    {
        base.AutoAttack();

        DoDamage(false);

        // 검기 발사 룬을 가지고 있을 때 검기 발사
        Missile(targetPos, false);
    }

    // 오브젝트를 공격하면 데미지를 입힘
    protected void DoDamage(bool isSkill)
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + Vector3.up * 0.1f, slime.transform.lossyScale * size, transform.forward, slime.transform.rotation, slime.Stat.attackRange);
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].transform.name);

            if (hits[i].transform.CompareTag("DamagedObject"))
            {
                Damage(hits[i].transform, isSkill);          // 데미지를 입힘
            }
        }

    }

    [SerializeField]
    float size = 2f;

    // 검기 발사
    protected void Missile(Vector3 targetPos, bool isSkill)
    {
        if (weaponRuneInfos[1].isActive)       
        {
            GameObject projectile = ObjectPoolingManager.Instance.Get(flag, transform.position, Vector3.zero);
            projectile.GetComponent<Projectile>().isSkill = isSkill;

            projectile.transform.LookAt(targetPos);      // 검 생성 뒤 마우스 방향을 바라봄

            lookRot = projectile.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.z = 0;

            projectile.transform.eulerAngles = lookRot;
        }
    }

    // 데미지를 입힘
    protected void Damage(Transform hitObj, bool isSkill)
    {
        Debug.Log(hitObj.name);

        IDamage damagedObject = hitObj.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            if (isSkill) damagedObject.SkillDamaged();
            else damagedObject.AutoAtkDamaged();

            if (hitObj.gameObject.layer == 8)       // 데미지를 입히는 오브젝트가 몬스터일 때 룬 발동
            {
                RuneManager.Instance.UseAttackRune(hitObj.gameObject);
            }
        }
    }
    #endregion

#if UNITY_EDITOR
    private Color _rayColor = Color.red;

    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;

        // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
        if (true == Physics.BoxCast(transform.position + Vector3.up * 0.1f, slime.transform.lossyScale * size, slime.transform.forward, out RaycastHit hit, slime.transform.rotation, slime.Stat.attackRange))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, slime.transform.forward * hit.distance);

            // Hit된 지점에 박스를 그려준다.
            Gizmos.DrawWireCube(transform.position + Vector3.up * 0.1f + slime.transform.forward * hit.distance, slime.transform.lossyScale * size);
        }
        else
        {
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, slime.transform.forward * slime.Stat.attackRange);
        }
    }
#endif
}
