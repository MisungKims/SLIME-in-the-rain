using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_UI_HP : MonoBehaviour
{
    //���� ���� ����
    private StatManager statmanager;

    //hp ���� ui ������Ʈ
    public Slider hpSlider;
    public TextMeshProUGUI hpText;  //��� ���� �򰥸����� ���� ����




    private void Start()
    {
        //���� �θ���
        statmanager = StatManager.Instance;

        //hp
        hpSlider.GetComponent<Slider>().maxValue = 1;
        hpSlider.GetComponent<Slider>().value = 0;
        hpText.text = "-";

    }

    // Update is called once per frame
    void Update()
    {

        hpSlider.GetComponent<Slider>().maxValue = (int)statmanager.myStats.maxHP;
        hpSlider.GetComponent<Slider>().value = (int)statmanager.myStats.HP;
        hpText.text = (int)statmanager.myStats.HP + " / " + (int)statmanager.myStats.maxHP;

    }
}
