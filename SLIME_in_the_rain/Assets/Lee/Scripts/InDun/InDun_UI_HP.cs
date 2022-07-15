using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_UI_HP : MonoBehaviour
{
    //스탯 변수 참조
    private StatManager statmanager;

    //hp 관련 ui 오브젝트
    public Slider hpSlider;
    public TextMeshProUGUI hpText;  //상속 순서 헷갈릴가봐 따로 받음




    private void Start()
    {
        //스탯 부르기
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
