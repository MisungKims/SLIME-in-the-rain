using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainCanvas : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static MainCanvas instance = null;
    public static MainCanvas Instance
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
    //싱글톤
    Slime slime;
    StatManager statManager;
    JellyManager jellyManager;

    //public
    [Header("메인 HP")]
    public Slider hpSlime;
    [Header("좌측 하단 HP")]
    public Slider hp;
    public TextMeshProUGUI hpText;
    [Header("젤리 Text")]
    public TextMeshProUGUI jellyText;
    [Header(" ")]
    public Slider skillCool;
    public TextMeshProUGUI skillText;
    [Header(" ")]
    public Slider dashCool;
    public TextMeshProUGUI dashText;
    [Header("Dash")]
    public List<Sprite> dashSprite;
    [Header("Skill")]
    public List<Sprite> skillSprite;

    //private
    float beforeMaxHP;
    float beforeHP;
    #endregion

    #region 유니티함수
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //싱글톤
        slime = Slime.Instance;
        statManager = StatManager.Instance;
        jellyManager = JellyManager.Instance;

        //hp 스탯 부르기
        beforeMaxHP = statManager.myStats.maxHP;
        beforeHP = statManager.myStats.HP;

        if(!slime.currentWeapon)
        {
            skillCool.gameObject.SetActive(false);
            dashCool.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //HP 
        hp.maxValue = statManager.myStats.maxHP;    
        hp.value = statManager.myStats.HP;
        hpSlime.maxValue = statManager.myStats.maxHP;
        hpSlime.value = statManager.myStats.HP;
        hpText.text = (int)statManager.myStats.HP + "/" + (int)statManager.myStats.maxHP;
        if (slime.currentWeapon)
        {
            //스킬 아이콘 꺼져있으면 켜주기
            if (!skillCool.gameObject.activeSelf)   skillCool.gameObject.SetActive(true);

            //스킬 쿨타임
            if (statManager.myStats.coolTime >= 1)
            {
                skillCool.maxValue = (int)statManager.myStats.coolTime;
            }
            else
            {
                skillCool.maxValue = statManager.myStats.coolTime;
            }
            skillCool.value = slime.currentWeapon.CurrentCoolTime;

            if (slime.currentWeapon.CurrentCoolTime > 1)
            {
                skillText.text = ((int)slime.currentWeapon.CurrentCoolTime).ToString();
            }
            else if (slime.currentWeapon.CurrentCoolTime > 0)
            {
                skillText.text = ((float)slime.currentWeapon.CurrentCoolTime).ToString("0.0");
            }
            else
            {
                skillText.text = " ";
                //Debug.Log("-" + slime.currentWeapon.weaponType);
                skillCool.transform.GetChild(0).GetComponent<Image>().sprite = Skill(slime.currentWeapon.weaponType);
            }

            //스킬 아이콘 꺼져있으면 켜주기
            if (!dashCool.gameObject.activeSelf) dashCool.gameObject.SetActive(true);

            //대쉬
            if(slime.currentWeapon.maxDashCoolTime >= 1)
            {
                dashCool.maxValue = (int)slime.currentWeapon.maxDashCoolTime;
            }
            else
            {
                dashCool.maxValue = slime.currentWeapon.maxDashCoolTime;
            }
            dashCool.value = slime.currentWeapon.dashCoolTime;

            if (slime.currentWeapon.dashCoolTime > 1)
            {
                dashText.text = ((int)slime.currentWeapon.dashCoolTime).ToString();
            }
            else if (slime.currentWeapon.dashCoolTime > 0)
            {
                dashText.text = ((float)slime.currentWeapon.dashCoolTime).ToString("0.0");
            }
            else    //스킬 쿨타임 돌아옴
            {
                dashText.text = " ";
                dashCool.transform.GetChild(0).GetComponent<Image>().sprite = Dash(slime.currentWeapon.weaponType);

            }
        }
        //젤리
        jellyText.text = jellyManager.JellyCount.ToString();
        
    }
    #endregion

    #region 함수

    //무기 바꿨을때 HP 변환 관련 함수
    public void changeWeapon()     
    {
        //Debug.Log(beforeHP);
        //Debug.Log(statManager.myStats.HP);
        //Debug.Log(beforeMaxHP);
        //Debug.Log(statManager.myStats.maxHP);

        //무기 변환시 (새무기의 체력이랑 같을땐 그대로)

        //새무기의 체력이 더 높을시 -> 비율대로 체력을 넣음
        if (beforeMaxHP < statManager.myStats.maxHP)            
        {
            statManager.myStats.HP = statManager.myStats.maxHP * ( beforeHP / beforeMaxHP);
        }

        //새무기의 체력이 더 낮고, 전의 체력이 보다 많을시 -> 최대 체력보단 피가 더 안참
        else if (beforeMaxHP > statManager.myStats.maxHP && beforeHP > statManager.myStats.maxHP)

        {
            statManager.myStats.HP = statManager.myStats.maxHP;
        }
        
        beforeMaxHP = statManager.myStats.maxHP;
        beforeHP = statManager.myStats.HP;

    }

    //대시 아이콘 매칭
    public Sprite Dash(EWeaponType weaponType)      
    {
        
        Sprite _sprite = null;
        switch (weaponType)
        {
            case EWeaponType.dagger:
                _sprite = dashSprite[0];
                break;
            case EWeaponType.sword:
                _sprite = dashSprite[1];
                break;
            case EWeaponType.iceStaff:
                _sprite = dashSprite[2];
                break;
            case EWeaponType.fireStaff:
                _sprite = dashSprite[3];
                break;
            case EWeaponType.bow:
                _sprite = dashSprite[4];
                break;
            default:
                break;
        }
        return _sprite;
    }
    //스킬 아이콘 매칭
    public Sprite Skill(EWeaponType weaponType)     
    {

        Sprite _sprite = null;
        switch (weaponType)
        {
            case EWeaponType.dagger:
                _sprite = skillSprite[0];
                break;
            case EWeaponType.sword:
                _sprite = skillSprite[1];
                break;
            case EWeaponType.iceStaff:
                _sprite = skillSprite[2];
                break;
            case EWeaponType.fireStaff:
                _sprite = skillSprite[3];
                break;
            case EWeaponType.bow:
                _sprite = skillSprite[4];
                break;
            default:
                break;
        }
        return _sprite;
    }
    #endregion
}
