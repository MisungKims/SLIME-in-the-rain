using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ICanvas : MonoBehaviour
{
    #region ����
    #region �̱���
    private static ICanvas instance = null;
    public static ICanvas Instance
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
    //�̱���
    Slime slime;
    StatManager statManager;
    JellyManager jellyManager;

    //public
    [Header("���� HP")]
    public Slider hpSlime;
    [Header("���� �ϴ� HP")]
    public Slider hp;
    public TextMeshProUGUI hpText;
    [Header("���� Text")]
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

    #region ����Ƽ�Լ�
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
        jellyManager = JellyManager.Instance;

        //hp ���� �θ���
        beforeMaxHP = statManager.myStats.maxHP;
        beforeHP = statManager.myStats.HP;
    }

    // Update is called once per frame
    void Update()
    {
        #region �⺻ ������ ��� ��Ÿ�� ���� ��ũ��Ʈ (�ּ�)
        //if(!slime.currentWeapon)    //���� �ȵ�������� 
        //{
        //    float time = slime.dashCoolTime - (Time.time - startTime);
        //    Debug.Log(time);
        //    dashCool.maxValue = (int)slime.dashCoolTime;
        //    if (dashReady)
        //    {
        //        dashCool.value = 0;
        //        dashText.text = " ";
        //        if (slime.isDash)
        //        {
        //            dashReady = false;
        //        }
        //    }
        //    //�뽬
        //    else
        //    {
        //        dashCool.value = time;
        //        if (dashCool.value > 1)
        //        {
        //            dashText.text = ((int)dashCool.value).ToString();
        //        }
        //        else if (dashCool.value > 0)
        //        {
        //            dashText.text = ((float)dashCool.value).ToString("0.0");
        //        }
        //        else
        //        {
        //            startTime = Time.time;
        //            dashReady = true;
        //        }
        //    }

        //}
        ////���� ��ų ��Ÿ��, HP��
        //else    //if (slime.currentWeapon)
        //{
        #endregion
        
        //HP 
        hp.maxValue = statManager.myStats.maxHP;
        hp.value = statManager.myStats.HP;
        hpSlime.maxValue = statManager.myStats.maxHP;
        hpSlime.value = statManager.myStats.HP;
        hpText.text = (int)statManager.myStats.HP + "/" + (int)statManager.myStats.maxHP;
        if (slime.currentWeapon)
        {
            //��ų ��Ÿ��
            skillCool.maxValue = (int)statManager.myStats.coolTime;
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
        
            //�뽬
            dashCool.maxValue = (int)slime.currentWeapon.maxDashCoolTime;
            dashCool.value = slime.currentWeapon.dashCoolTime;

            if (slime.currentWeapon.dashCoolTime > 1)
            {
                dashText.text = ((int)slime.currentWeapon.dashCoolTime).ToString();
            }
            else if (slime.currentWeapon.dashCoolTime > 0)
            {
                dashText.text = ((float)slime.currentWeapon.dashCoolTime).ToString("0.0");
            }
            else    //��ų ��Ÿ�� ���ƿ�
            {
                dashText.text = " ";
                dashCool.transform.GetChild(0).GetComponent<Image>().sprite = Dash(slime.currentWeapon.weaponType);

            }
        }
        //����
        jellyText.text = jellyManager.JellyCount.ToString();
        #region �⺻ ������ ��� ���� ��ũ��Ʈ
        //}
        #endregion
    }
    #endregion

    #region �Լ�

    //���� �ٲ����� HP ��ȯ ���� �Լ�
    public void changeWeapon()     
    {
        //Debug.Log(beforeHP);
        //Debug.Log(statManager.myStats.HP);
        //Debug.Log(beforeMaxHP);
        //Debug.Log(statManager.myStats.maxHP);

        //���� ��ȯ�� (�������� ü���̶� ������ �״��)

        //�������� ü���� �� ������ -> ������� ü���� ����
        if (beforeMaxHP < statManager.myStats.maxHP)            
        {
            statManager.myStats.HP = statManager.myStats.maxHP * ( beforeHP / beforeMaxHP);
        }

        //�������� ü���� �� ����, ���� ü���� ���� ������ -> �ִ� ü�º��� �ǰ� �� ����
        else if (beforeMaxHP > statManager.myStats.maxHP && beforeHP > statManager.myStats.maxHP)

        {
            statManager.myStats.HP = statManager.myStats.maxHP;
        }
        
        beforeMaxHP = statManager.myStats.maxHP;
        beforeHP = statManager.myStats.HP;

    }

    //��� ������ ��Ī
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
    //��ų ������ ��Ī
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
