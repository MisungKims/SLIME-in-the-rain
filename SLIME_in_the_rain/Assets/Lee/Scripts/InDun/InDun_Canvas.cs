using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_Canvas : MonoBehaviour
{
    #region �̱���
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
        //�̱���
        slime = Slime.Instance;
        statManager = StatManager.Instance;
        //hp
        //���� �θ���
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

            //���� �ٲ� �� HP ������� ���̰ų� �ִ�ü�� ���� �ʰ�
            //if (beforeWeapon != slime.currentWeapon && beforeWeapon != null)
            //{
            //    changeWeapon();
            //    //���� ���� ���� ����
            //    beforeWeapon = slime.currentWeapon;
            //    beforeMaxHP = statManager.myStats.maxHP;
            //    beforeHP = statManager.myStats.HP;

            //}
        }

        
    }


    public void changeWeapon()
    {
        Debug.Log(slime.currentWeapon);
        //���� ��ȯ��
        if (beforeWeapon != slime.currentWeapon)
        {
            //�������� ü���� �� ������ -> ������� ü���� ����
            if (beforeMaxHP < statManager.myStats.maxHP)
            {
                statManager.myStats.HP = statManager.myStats.maxHP * ( beforeHP / beforeMaxHP);
            }
            //�������� ü���� �� ������ -> �ִ� ü�º��� �ǰ� �� ����
            else if(beforeMaxHP > statManager.myStats.maxHP)
            {

            }
            //�������� ü���̶� ������ �״��
        }



    }
}
