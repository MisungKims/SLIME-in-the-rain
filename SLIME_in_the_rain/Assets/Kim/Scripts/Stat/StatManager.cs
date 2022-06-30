/**
 * @brief 스탯 매니저
 * @author 김미성
 * @date 22-06-30
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
    private Stats extraStats;       // 젤라틴, 룬 등으로 추가될 스탯 증가량

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
        test();
    }
    #endregion

    #region 함수
    // 스탯 초기화
    void InitStats()
    {
        originStats = new Stats(100, 100, 1f, 1.2f, 1f, 1f, 1f, 1f, 1);
        myStats = new Stats(100, 100, 1f, 1.2f, 1f, 1f, 1f, 1f, 1);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1);
    }

    // amount 만큼 증가값을 반환
    // ex) HP 30% 증가값
    public float GetIncrementStat(string statName, float percent)
    {
        float returnVal = 0f;

        switch (statName)
        {
            case "MaxHP":
                returnVal = (myStats.maxHP * percent) * 0.01f;
                break;
            case "AtkSpeed":
                returnVal = (myStats.attackSpeed * percent) * 0.01f;
                break;
            case "AtkPower":
                returnVal = (myStats.attackPower * percent) * 0.01f;
                break;
            case "AtkRange":
                returnVal = (myStats.attackRange * percent) * 0.01f;
                break;
        }

        return returnVal;
    }

    // 무기 변경 시 해당 무기의 스탯으로 변경
    public void ChangeStats(Weapon weapon)
    {
        myStats.maxHP = weapon.stats.maxHP + extraStats.maxHP;
        myStats.coolTime = weapon.stats.coolTime + extraStats.coolTime;
        myStats.moveSpeed = weapon.stats.moveSpeed + extraStats.moveSpeed;
        myStats.attackSpeed = weapon.stats.attackSpeed + extraStats.attackSpeed;
        myStats.attackPower = weapon.stats.attackPower + extraStats.attackPower;
        myStats.attackRange = weapon.stats.attackRange + extraStats.attackRange;
        myStats.defensePower = weapon.stats.defensePower + extraStats.defensePower;
        myStats.hitCount = weapon.stats.hitCount * extraStats.hitCount;
    }


    public void test()
    {
        Debug.Log("1 " + myStats.attackSpeed);
        AddAttackSpeed(GetIncrementStat("AtkSpeed", 50) * -1);
        Debug.Log("2 " + myStats.attackSpeed);

    }

    // Max Hp 스탯 변경
    public void AddMaxHP(float amount)
    {
        extraStats.maxHP += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon != null)
        {
            myStats.maxHP = currentWeapon.stats.maxHP + extraStats.maxHP;            // 무기를 가진 상태라면, 무기의 스탯 값에서 계산
        }
        else
        {
            myStats.maxHP = originStats.maxHP + extraStats.maxHP;
        }
    }

    // Hp 스탯 변경
    public void AddHP(float amount)
    {
        float sum = amount + myStats.HP;
        if (sum > myStats.maxHP)
        {
            myStats.HP = myStats.maxHP;
        }
        else
        {
            myStats.HP = sum;
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
        if (currentWeapon != null)
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

    // 공격 범위 스탯 변경
    public void AddAttackRange(float amount)
    {
        extraStats.attackRange += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.attackRange = currentWeapon.stats.attackRange + extraStats.attackRange;
        }
        else
        {
            myStats.attackRange = originStats.attackRange + extraStats.attackRange;
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

    // 타수 스탯 변경
    // ex) amount가 2면 2배
    public void AddHitCount(int amount)
    {
        extraStats.hitCount *= amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.hitCount = currentWeapon.stats.hitCount * extraStats.hitCount;
        }
        else
        {
            myStats.hitCount = originStats.hitCount * extraStats.hitCount;
        }
    }

    #endregion
}
