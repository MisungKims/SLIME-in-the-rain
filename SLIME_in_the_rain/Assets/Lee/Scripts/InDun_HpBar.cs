using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_HpBar : MonoBehaviour
{
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public StatManager statmanager;

    public Slider skillCool;



    // Update is called once per frame
    void Update()
    {
        hpSlider.GetComponent<Slider>().value = statmanager.myStats.HP;
        hpText.text = statmanager.myStats.HP + " / " + statmanager.myStats.maxHP;
    }
}
