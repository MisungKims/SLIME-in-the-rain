/**
 * @brief 몬스터
 * @author 김미성
 * @date 22-07-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamage
{
    #region 변수

    StatManager statManager;

    [SerializeField]
    private Stats stats;

    #endregion

    #region 유니티 함수

    void Start()
    {
        statManager = StatManager.Instance;
    }
    #endregion

    #region 함수
    public void AutoAtkDamaged()
    {
        stats.HP -= statManager.GetAutoAtkDamage();

        Debug.Log(name + "평타" + statManager.GetAutoAtkDamage());
    }

    public void SkillDamaged()
    {
        stats.HP -= statManager.GetSkillDamage();

        Debug.Log(name + " 데미지 " + statManager.GetSkillDamage());
    }

    public void Stun(float stunTime)
    {
        Debug.Log("스턴");
    }
    #endregion
}
