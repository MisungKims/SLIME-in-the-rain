/**
 * @brief �� �Ŵ���
 * @author ��̼�
 * @date 22-06-29
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static RuneManager instance = null;
    public static RuneManager Instance
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

    [SerializeField]
    private List<Rune> runes = new List<Rune>();        // ��ü ���� ����Ʈ

    public Rune[] myRunes = new Rune[3];       // �� ��
    public int runeCount = 0;

    public RuneSlot[] runeSlots = new RuneSlot[3];      // ui ����

    int rand;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
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
    }
    #endregion

    #region �Լ�
    // �������� ���� ��ȯ
    public Rune GetRandomRune()
    {
        rand = Random.Range(0, runes.Count);

        return runes[rand];
    }

    // ���� �߰�
    public void AddMyRune(Rune rune)
    {
        if (runeCount > 2) return;

        Rune runeObj = GameObject.Instantiate(rune, this.transform);
        myRunes[runeCount] = runeObj;
        UsePassiveRune(runeObj);         // �߰��� ���� �нú� ���̸� �ٷ� ���� (��� ����, ���� ���� ��)
        UseWeaponRune(Slime.Instance.currentWeapon);
        runeSlots[runeCount].SetUI(runeObj);

        runeCount++;
    }


    // ���� �� �ߵ�
    public void UseWeaponRune(Weapon weapon)
    {
        if (!weapon) return;

        for (int i = 0; i < runeCount; i++)
        {
            IWeaponRune weaponRune = myRunes[i].GetComponent<IWeaponRune>();
            if (weaponRune != null)
            {
                weaponRune.Use(weapon);
            }
        }
    }

    // �нú� �� �ߵ�
    public void UsePassiveRune(Rune rune)
    {
        IPassiveRune passiveRune = rune.GetComponent<IPassiveRune>();
        if (passiveRune != null)
        {
            passiveRune.Passive();
        }
    }


    // ���� �� �� �ߵ�
    public void UseAttackRune()
    {
        for (int i = 0; i < runeCount; i++)
        {
            IAttackRune attackRune = myRunes[i].GetComponent<IAttackRune>();
            if (attackRune != null)
            {
                attackRune.Attack();
            }
        }
    }

    // ��ų �� �� �ߵ�
    public void UseSkillRune()
    {
        for (int i = 0; i < runeCount; i++)
        {
            IAttackRune attackRune = myRunes[i].GetComponent<IAttackRune>();
            if (attackRune != null)
            {
                attackRune.Attack();
            }
        }
    }

    // ��� �� �� �ߵ�
    public void UseDashRune()
    {
        for (int i = 0; i < runeCount; i++)
        {
            IDashRune dashRune = myRunes[i].GetComponent<IDashRune>();
            if (dashRune != null)
            {
                dashRune.Dash();
            }
        }
    }
    #endregion
}
