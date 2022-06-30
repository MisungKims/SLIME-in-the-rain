/**
 * @brief ���� �Ŵ���
 * @author ��̼�
 * @date 22-06-30
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    #region ����
    #region �̱���
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

    //////// ����
    private Stats originStats;      // �⺻ ����
    public Stats myStats;           // ���� ����
    private Stats extraStats;       // ����ƾ, �� ������ �߰��� ���� ������

    //////// ĳ��
    private Slime slime;
    private Weapon currentWeapon;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�
    // ���� �ʱ�ȭ
    void InitStats()
    {
        originStats = new Stats(100, 100, 1f, 1.2f, 1f, 1f, 1f, 1f, 1);
        myStats = new Stats(100, 100, 1f, 1.2f, 1f, 1f, 1f, 1f, 1);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1);
    }

    // amount ��ŭ �������� ��ȯ
    // ex) HP 30% ������
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

    // ���� ���� �� �ش� ������ �������� ����
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

    // Max Hp ���� ����
    public void AddMaxHP(float amount)
    {
        extraStats.maxHP += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon != null)
        {
            myStats.maxHP = currentWeapon.stats.maxHP + extraStats.maxHP;            // ���⸦ ���� ���¶��, ������ ���� ������ ���
        }
        else
        {
            myStats.maxHP = originStats.maxHP + extraStats.maxHP;
        }
    }

    // Hp ���� ����
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

    // ��Ÿ�� ���� ����
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

    // �̼� ���� ����
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

    // ���� ���� ����
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

    // ���ݷ� ���� ����
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

    // ���� ���� ���� ����
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

    // ���� ���� ����
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

    // Ÿ�� ���� ����
    // ex) amount�� 2�� 2��
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
