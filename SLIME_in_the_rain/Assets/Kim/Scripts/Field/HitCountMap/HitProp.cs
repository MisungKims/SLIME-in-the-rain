/**
 * @details Ÿ�� �ʿ��� Ÿ���ؾ��� ������Ʈ
 * @author ��̼�
 * @date 22-08-09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitProp : MonoBehaviour, IDamage
{
    #region ����

    private HitCountMap hitCountMap;

    #endregion

    private void Start()
    {
        hitCountMap = HitCountMap.Instance;
    }

    private void OnDisable()
    {
        hitCountMap.GetParticle(transform.position);
    }


    // �������� ������ Ÿ�� ����

    private void TakeDamage()
    {
        hitCountMap.GetParticle(transform.position);
        hitCountMap.Count++;
    }
    
    public void AutoAtkDamaged()
    {
        TakeDamage();
    }

    public void SkillDamaged()
    {
        TakeDamage();
    }

    public void Stun(float stunTime)
    {
        TakeDamage();
    }
}
