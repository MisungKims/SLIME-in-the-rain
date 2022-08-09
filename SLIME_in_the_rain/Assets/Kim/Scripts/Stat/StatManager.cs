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
    }
    #endregion

    #region 함수
    

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
        myStats.increasesDamage = weapon.stats.increasesDamage + extraStats.increasesDamage;
    }

    // Max Hp 스탯 변경
    public void AddMaxHP(float amount)
    {
        extraStats.maxHP += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon != null)
        {
            myStats.maxHP = currentWeapon.stats.maxHP + extraStats.maxHP + gelatinStat.maxHP;            // 무기를 가진 상태라면, 무기의 스탯 값에서 계산
        }
        else
        {
            myStats.maxHP = originStats.maxHP + extraStats.maxHP + gelatinStat.maxHP;
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
            myStats.coolTime = currentWeapon.stats.coolTime + extraStats.coolTime + gelatinStat.coolTime;
        }
        else
        {
            myStats.coolTime = originStats.coolTime + extraStats.coolTime + gelatinStat.coolTime;
        }
    }

    // 이속 스탯 변경
    public void AddMoveSpeed(float amount)
    {
        extraStats.moveSpeed += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.moveSpeed = currentWeapon.stats.moveSpeed + extraStats.moveSpeed + gelatinStat.moveSpeed;
        }
        else
        {
            myStats.moveSpeed = originStats.moveSpeed + extraStats.moveSpeed + gelatinStat.moveSpeed;
        }
    }

    // 공속 스탯 변경
    public void AddAttackSpeed(float amount)
    {
        extraStats.attackSpeed += amount*0.01f;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon != null)
        {
            myStats.attackSpeed = currentWeapon.stats.attackSpeed - extraStats.attackSpeed - gelatinStat.attackSpeed;
        }
        else
        {
            myStats.attackSpeed = originStats.attackSpeed - extraStats.attackSpeed - gelatinStat.attackSpeed;
        }

        if (myStats.attackSpeed <= 0)
        {
            myStats.attackSpeed = 0.001f;
        }

    }

    // 공격력 스탯 변경
    public void AddAttackPower(float amount)
    {
        extraStats.attackPower += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.attackPower = currentWeapon.stats.attackPower + extraStats.attackPower + gelatinStat.attackPower;
        }
        else
        {
            myStats.attackPower = originStats.attackPower + extraStats.attackPower + gelatinStat.attackPower;
        }
    }

    // 공격 범위 스탯 변경
    public void MultipleAttackRange(float amount)
    {
        extraStats.attackRange *= amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.attackRange = currentWeapon.stats.attackRange * (extraStats.attackRange + gelatinStat.attackPower);
        }
        else
        {
            myStats.attackRange = originStats.attackRange * (extraStats.attackRange + gelatinStat.attackPower);
        }
    }

    // 방어력 스탯 변경
    public void AddDefensePower(float amount)
    {
        extraStats.defensePower += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.defensePower = currentWeapon.stats.defensePower + extraStats.defensePower+ gelatinStat.defensePower;
        }
        else
        {
            myStats.defensePower = originStats.defensePower + extraStats.defensePower + gelatinStat.defensePower;
        }
    }

    // 타수 스탯 변경
    // ex) amount가 2면 2배
    public void MultipleHitCount(int amount)
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

    // 데미지 증가
    public void AddDamage(float amount)
    {
        extraStats.increasesDamage += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.increasesDamage = currentWeapon.stats.increasesDamage + extraStats.increasesDamage;
        }
        else
        {
            myStats.increasesDamage = originStats.increasesDamage + extraStats.increasesDamage;
        }
    }

    
    /// TODO : 데미지 구현

    // 평타 데미지 반환
    public float GetAutoAtkDamage()
    {
        float damage = 1f;

        damage += (damage * myStats.increasesDamage) * 0.01f;        // 데미지 증가량 계산

        return damage;
    }

    // 스킬 데미지 반환
    public float GetSkillDamage()
    {
        float damage = 0.1f;

        damage += (damage * myStats.increasesDamage) * 0.01f;        // 데미지 증가량 계산

        return damage;
    }
    #endregion

    /////추가
    /// add함수, 모든 스탯 함수에 gelatinStat.(); 추가하기.
    public Stats gelatinStat;           // 젤라틴 스탯

    // 스탯 초기화
    void InitStats()
    {
        originStats = new Stats(100, 100, 1f, 1.2f, 1f, 1f, 1f, 1f, 1, 0);
        myStats = new Stats(100, 100, 1f, 1.2f, 1f, 1f, 1f, 1f, 1, 0);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 1, 0);
        gelatinStat = new Stats(0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 1, 0);
    }



    public void AddGelatinMaxHP(float amount)
    {
        gelatinStat.maxHP = amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon != null)
        {
            myStats.maxHP = currentWeapon.stats.maxHP + extraStats.maxHP + gelatinStat.maxHP;            // 무기를 가진 상태라면, 무기의 스탯 값에서 계산
        }
        else
        {
            myStats.maxHP = originStats.maxHP + extraStats.maxHP + gelatinStat.maxHP;
        }
    }


    // 쿨타임 스탯 변경
    public void AddGelatinCoolTime(float amount)
    {
        gelatinStat.coolTime = amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.coolTime = currentWeapon.stats.coolTime + extraStats.coolTime + gelatinStat.coolTime;
        }
        else
        {
            myStats.coolTime = originStats.coolTime + extraStats.coolTime + gelatinStat.coolTime;
        }
    }

    // 이속 스탯 변경
    public void AddGelatinMoveSpeed(float amount)
    {
        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            gelatinStat.moveSpeed = currentWeapon.stats.moveSpeed * amount*0.01f;
            myStats.moveSpeed = currentWeapon.stats.moveSpeed + extraStats.moveSpeed + gelatinStat.moveSpeed;
        }
        else
        {
            gelatinStat.moveSpeed = originStats.moveSpeed * amount * 0.01f;
            myStats.moveSpeed = originStats.moveSpeed + extraStats.moveSpeed + gelatinStat.moveSpeed;
        }
    }

    // 공속 스탯 변경
    public void AddGelatinAttackSpeed(float amount)
    {
        currentWeapon = slime.currentWeapon;
        if (currentWeapon != null)
        {
            gelatinStat.attackSpeed = currentWeapon.stats.attackSpeed * amount*0.01f;
            myStats.attackSpeed = currentWeapon.stats.attackSpeed - extraStats.attackSpeed - gelatinStat.attackSpeed;

        }
        else
        {
            gelatinStat.attackSpeed = originStats.attackSpeed * amount * 0.01f;
            myStats.attackSpeed = originStats.attackSpeed - extraStats.attackSpeed - gelatinStat.attackSpeed;
        }
        if (myStats.attackSpeed <= 0)
        {
            myStats.attackSpeed = 0.001f;
        }

    }

    // 공격력 스탯 변경
    public void AddGelatinAttackPower(float amount)
    {
        gelatinStat.attackPower = amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.attackPower = currentWeapon.stats.attackPower + extraStats.attackPower + gelatinStat.attackPower;
        }
        else
        {
            myStats.attackPower = originStats.attackPower + extraStats.attackPower+ gelatinStat.attackPower;
        }
    }

    // 공격 범위 스탯 변경
    public void MultipleGelatinAttackRange(float amount)
    {
        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            gelatinStat.attackRange = currentWeapon.stats.attackRange * amount*0.01f;
            myStats.attackRange = currentWeapon.stats.attackRange * (extraStats.attackRange+gelatinStat.attackRange);
        }
        else
        {
            gelatinStat.attackRange = originStats.attackRange * amount*0.01f;
            myStats.attackRange = originStats.attackRange * (extraStats.attackRange + gelatinStat.attackRange);
        }
    }

    // 방어력 스탯 변경
    public void AddGelatinDefensePower(float amount)
    {
        gelatinStat.defensePower = amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)
        {
            myStats.defensePower = currentWeapon.stats.defensePower + extraStats.defensePower + gelatinStat.defensePower;
        }
        else
        {
            myStats.defensePower = originStats.defensePower + extraStats.defensePower + gelatinStat.defensePower;
        }
    }


}
