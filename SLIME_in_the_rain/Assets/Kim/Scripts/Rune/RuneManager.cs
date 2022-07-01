/**
 * @brief 룬 매니저
 * @author 김미성
 * @date 22-06-29
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    #region 변수
    #region 싱글톤
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
    private List<Rune> runes = new List<Rune>();        // 전체 룬의 리스트

    public Rune[] myRunes = new Rune[3];       // 내 룬
    public int runeCount = 0;

    public RuneSlot[] runeSlots = new RuneSlot[3];      // ui 슬롯

    int rand;
    #endregion

    #region 유니티 함수
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

    #region 함수
    // 랜덤으로 룬을 반환
    public Rune GetRandomRune()
    {
        rand = Random.Range(0, runes.Count);

        return runes[rand];
    }

    // 룬을 추가
    public void AddMyRune(Rune rune)
    {
        if (runeCount > 2) return;

        Rune runeObj = GameObject.Instantiate(rune, this.transform);
        myRunes[runeCount] = runeObj;
        UsePassiveRune(runeObj);         // 추가한 룬이 패시브 룬이면 바로 적용 (목숨 증가, 스탯 증가 등)
        UseWeaponRune(Slime.Instance.currentWeapon);
        runeSlots[runeCount].SetUI(runeObj);

        runeCount++;
    }


    // 무기 룬 발동
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

    // 패시브 룬 발동
    public void UsePassiveRune(Rune rune)
    {
        IPassiveRune passiveRune = rune.GetComponent<IPassiveRune>();
        if (passiveRune != null)
        {
            passiveRune.Passive();
        }
    }


    // 공격 시 룬 발동
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

    // 스킬 시 룬 발동
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

    // 대시 시 룬 발동
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
