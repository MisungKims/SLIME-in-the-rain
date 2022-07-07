using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : DetectingMonster
{
    #region 변수
    [SerializeField]
    private Slider hpBar;
    #endregion

    #region 함수

    protected override void ShowHPBar()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
        }

        hpBar.value = stats.HP;
    }

    protected override void HideHPBar()
    {
        hpBar.gameObject.SetActive(false);
    }
    #endregion
}
