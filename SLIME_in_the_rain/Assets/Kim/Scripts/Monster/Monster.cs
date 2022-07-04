/**
 * @brief ����
 * @author ��̼�
 * @date 22-07-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamage
{
    #region ����

    StatManager statManager;

    [SerializeField]
    private Stats stats;

    #endregion

    #region ����Ƽ �Լ�

    void Start()
    {
        statManager = StatManager.Instance;
    }
    #endregion

    #region �Լ�
    public void AutoAtkDamaged()
    {
        stats.HP -= statManager.GetAutoAtkDamage();

        Debug.Log(name + "��Ÿ" + statManager.GetAutoAtkDamage());
    }

    public void SkillDamaged()
    {
        stats.HP -= statManager.GetSkillDamage();

        Debug.Log(name + " ������ " + statManager.GetSkillDamage());
    }

    public void Stun(float stunTime)
    {
        Debug.Log("����");
    }
    #endregion
}
