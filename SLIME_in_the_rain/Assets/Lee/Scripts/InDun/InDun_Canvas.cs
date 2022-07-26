using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_Canvas : MonoBehaviour
{
    #region 싱글톤
    private static InDun_Canvas instance = null;
    public static InDun_Canvas Instance
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
    Slime slime;
    StatManager statManager;
    [Header(" ")]
    public Slider hp;
    public TextMeshProUGUI hpText;
    [Header(" ")]
    public Slider skillCool;
    public TextMeshProUGUI skillText;
    [Header(" ")]
    public Slider dashCool;
    public TextMeshProUGUI dashText;

    Weapon beforeWeapon;
    float beforeMaxHP;
    float beforeHP;

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
        //hp
        //스탯 부르기
    }

    // Update is called once per frame
    void Update()
    {
        if (slime.currentWeapon)
        {
            Debug.Log(statManager.myStats.HP);
            hp.maxValue = statManager.myStats.maxHP;
            hp.value = statManager.myStats.HP;
            hpText.text = (int)statManager.myStats.HP + "/" + (int)statManager.myStats.maxHP;

            skillCool.maxValue = (int)statManager.myStats.coolTime;
            skillCool.value = slime.currentWeapon.CurrentCoolTime;

            dashCool.maxValue = (int)slime.currentWeapon.maxDashCoolTime;
            dashCool.value = slime.currentWeapon.dashCoolTime;

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
            }

            if (slime.currentWeapon.dashCoolTime > 1)
            {
                dashText.text = ((int)slime.currentWeapon.dashCoolTime).ToString();
            }
            else if (slime.currentWeapon.dashCoolTime > 0)
            {
                dashText.text = ((float)slime.currentWeapon.dashCoolTime).ToString("0.0");
            }
            else
            {
                dashText.text = " ";
            }
            Debug.Log(beforeWeapon);

            //무기 바꿀 시 HP 비율대로 줄이거나 최대체력 넘지 않게
            //if (beforeWeapon != slime.currentWeapon && beforeWeapon != null)
            //{
            //    changeWeapon();
            //    //현재 무기 정보 저장
            //    beforeWeapon = slime.currentWeapon;
            //    beforeMaxHP = statManager.myStats.maxHP;
            //    beforeHP = statManager.myStats.HP;

            //}
        }

        
    }


    public void changeWeapon()
    {
        Debug.Log(slime.currentWeapon);
        //무기 변환시
        if (beforeWeapon != slime.currentWeapon)
        {
            //새무기의 체력이 더 높을시 -> 비율대로 체력을 넣음
            if (beforeMaxHP < statManager.myStats.maxHP)
            {
                statManager.myStats.HP = statManager.myStats.maxHP * ( beforeHP / beforeMaxHP);
            }
            //새무기의 체력이 더 낮을시 -> 최대 체력보단 피가 더 안참
            else if(beforeMaxHP > statManager.myStats.maxHP)
            {

            }
            //새무기의 체력이랑 같을땐 그대로
        }



    }
}
