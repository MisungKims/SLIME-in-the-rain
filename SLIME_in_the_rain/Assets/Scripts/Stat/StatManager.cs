/**
 * @brief ���� �Ŵ���
 * @author ��̼�
 * @date 22-06-27
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
    private Stats extraStats;       // ����ƾ, �� ������ �߰��� ����

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
    }
    #endregion

    #region �Լ�
    // ���� �ʱ�ȭ
    void InitStats()
    {
        originStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        myStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f);
    }

    // ���� ���� �� �ش� ������ �������� ����
    public void ChangeStats(Weapon weapon)
    {
        myStats.HP = weapon.stats.HP + extraStats.HP;
        myStats.coolTime = weapon.stats.coolTime + extraStats.coolTime;
        myStats.moveSpeed = weapon.stats.moveSpeed + extraStats.moveSpeed;
        myStats.attackSpeed = weapon.stats.attackSpeed + extraStats.attackSpeed;
        myStats.attackPower = weapon.stats.attackPower + extraStats.attackPower;
        myStats.defensePower = weapon.stats.defensePower + extraStats.defensePower;
    }

    // Hp ���� ����
    public void AddHP(float amount)
    {
        extraStats.HP += amount;

        currentWeapon = slime.currentWeapon;
        if (currentWeapon)                       
        {
            // ���⸦ ���� ���¶��, ������ ���� ������ ���
            myStats.HP = currentWeapon.stats.HP + extraStats.HP;
        }
        else
        {
            myStats.HP = originStats.HP + extraStats.HP;
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
        if (currentWeapon)
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

#endregion
}
