using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Monster
{
    #region ����
    [SerializeField]
    private Slider hpBar;
    #endregion

    #region �Լ�

    public override void ShowHPBar()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
        }

        hpBar.value = stats.HP;
    }

    public override void HideHPBar()
    {
        hpBar.gameObject.SetActive(false);
    }
    #endregion
}
