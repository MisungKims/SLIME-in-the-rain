using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InDun_UI_SkillCool : MonoBehaviour
{
    private Weapon weapon;

    public Slider skillCool;

    int getTime = new int();

    // Start is called before the first frame update
    void Start()
    {
        //getTime = weapon.CurrentCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        //weapon = Slime.Instance.currentWeapon;
        //if (weapon)
        //{
        //    skillCool.GetComponent<Slider>().maxValue = weapon.stats.coolTime;
        //    skillCool.GetComponent<Slider>().value = getTime;
            
        //    if (weapon.isCanSkill)
        //    {
        //        skillCool.transform.GetChild(1).gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        skillCool.transform.GetChild(1).gameObject.SetActive(true);
                
        //        if(getTime > 3)
        //        {
        //            skillCool.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = weapon.CurrentCoolTime.ToString();
        //        }
        //        else
        //        {
        //            skillCool.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = weapon.CurrentCoolTime.ToString();
        //        }
        //    }
            
        //}
    }
}
