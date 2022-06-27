/**
 * @brief 스탯 매니저
 * @author 김미성
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static StatManager instance = null;
    public static StatManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    //////// 스탯
    private Stats originStats;      // 기본 스탯
    public Stats myStats;           // 현재 스탯
    private Stats extraStats;       // 젤라틴, 룬 등으로 추가될 스탯

    //////// 캐싱
    private Slime slime;
    private Weapon currentWeapon;
    #endregion

    #region 유니티 함수
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitStats();
    }

    private void Start()
    {
        slime = Slime.Instance;
    }
    #endregion

    #region 함수
    // 스탯 초기화
    void InitStats()
    {
        originStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        myStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f);
    }

    // 무기 변경 시 해당 무기의 스탯으로 변경
    public void ChangeStats(Weapon weapon)
    {
        myStats.HP = weapon.stats.HP + extraStats.HP;
        myStats.coolTime = weapon.stats.coolTime + extraStats.coolTime;
        myStats.moveSpeed = weapon.stats.moveSpeed + extraStats.moveSpeed;
        myStats.attackSpeed = weapon.stats.attackSpeed + extraStats.attackSpeed;
        myStats.attackPower = weapon.stats.attackPower + extraStats.attackPower;
        myStats.defensePower = weapon.stats.defensePower + extraStats.defensePower;
    }

    // Hp 스탯 변경
    public void AddHP(float amount)
    {
        extraStats.HP += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)                       
        {
            // 무기를 가진 상태라면, 무기의 스탯 값에서 계산
            myStats.HP = currentWeapon.stats.HP + extraStats.HP;
        }
        else
        {
            myStats.HP = originStats.HP + extraStats.HP;
        }
    }

    // 쿨타임 스탯 변경
    public void AddCoolTime(float amount)
    {
        extraStats.coolTime += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.coolTime = currentWeapon.stats.coolTime + extraStats.coolTime;
        }
        else
        {
            myStats.coolTime = originStats.coolTime + extraStats.coolTime;
        }
    }

    // 이속 스탯 변경
    public void AddMoveSpeed(float amount)
    {
        extraStats.moveSpeed += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.moveSpeed = currentWeapon.stats.moveSpeed + extraStats.moveSpeed;
        }
        else
        {
            myStats.moveSpeed = originStats.moveSpeed + extraStats.moveSpeed;
        }
    }

    // 공속 스탯 변경
    public void AddAttackSpeed(float amount)
    {
        extraStats.attackSpeed += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.attackSpeed = currentWeapon.stats.attackSpeed + extraStats.attackSpeed;
        }
        else
        {
            myStats.attackSpeed = originStats.attackSpeed + extraStats.attackSpeed;
        }
    }

    // 공격력 스탯 변경
    public void AddAttackPower(float amount)
    {
        extraStats.attackPower += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.attackPower = currentWeapon.stats.attackPower + extraStats.attackPower;
        }
        else
        {
            myStats.attackPower = originStats.attackPower + extraStats.attackPower;
        }
    }

    // 방어력 스탯 변경
    public void AddDefensePower(float amount)
    {
        extraStats.defensePower += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.defensePower = currentWeapon.stats.defensePower + extraStats.defensePower;
        }
        else
        {
            myStats.defensePower = originStats.defensePower + extraStats.defensePower;
        }
    }

#endregion
}
