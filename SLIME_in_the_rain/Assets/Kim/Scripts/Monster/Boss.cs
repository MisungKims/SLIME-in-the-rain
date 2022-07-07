using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : DetectingMonster
{
    #region ����
    [SerializeField]
    private Slider hpBar;
    #endregion

    #region �Լ�

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
