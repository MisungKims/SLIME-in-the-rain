using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Monster
{
    #region ����
    [SerializeField]
    private Slider hpBar;

    private WaitForSeconds waitFor6s = new WaitForSeconds(6f);
    #endregion

    #region �ڷ�ƾ
    protected override IEnumerator DieCoroutine()
    {
        yield return waitFor6s;

        this.gameObject.SetActive(false);
    }
    #endregion

    #region �Լ�

    public override void ShowHPBar()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
            hpBar.maxValue = stats.maxHP;
        }

        hpBar.value = stats.HP;
    }

    public override void HideHPBar()
    {
        hpBar.gameObject.SetActive(false);
    }
    #endregion
}
