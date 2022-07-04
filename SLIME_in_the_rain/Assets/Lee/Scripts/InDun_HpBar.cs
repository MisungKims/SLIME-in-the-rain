using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_HpBar : MonoBehaviour
{
    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    private StatManager statmanager;


    private void Start()
    {
        statmanager = StatManager.Instance;

        hpSlider.GetComponent<Slider>().value = 10;
        hpText.text = "-";


    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.GetComponent<Slider>().value = (int)statmanager.myStats.HP;
        hpText.text = (int)statmanager.myStats.HP + " / " + (int)statmanager.myStats.maxHP;


    }
}
