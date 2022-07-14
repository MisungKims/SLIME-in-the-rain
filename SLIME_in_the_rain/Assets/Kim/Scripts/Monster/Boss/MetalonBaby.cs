/**
 * @brief Metalon의 새끼 거미
 * @author 김미성
 * @date 22-07-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalonBaby : Monster
{
    // 체력바
    private Slider hpBar;
    private Vector3 hpBarPos;


    protected override void OnEnable()
    {
        base.OnEnable();

        TryStartChase();
    }

    protected override IEnumerator DieCoroutine()
    {
        yield return waitFor3s;

        this.gameObject.SetActive(false);
    }

    // 체력바의 위치를 조절하는 코루틴
    IEnumerator SetHPBarPos()
    {
        while (hpBar)
        {
            hpBarPos = transform.position;
            hpBarPos.y += 1.5f;

            hpBar.transform.position = hpBarPos;

            yield return null;
        }
    }

    // 체력바 활성화
    public override void ShowHPBar()
    {
        if (!hpBar)
        {
            hpBar = objectPoolingManager.Get(EObjectFlag.hpBar).GetComponent<Slider>();
            hpBar.maxValue = stats.maxHP;

            StartCoroutine(SetHPBarPos());
        }

        hpBar.value = stats.HP;
    }

    // 체력바 비활성화
    public override void HideHPBar()
    {
        if (!hpBar) return;

        objectPoolingManager.Set(hpBar.gameObject, EObjectFlag.hpBar);
        hpBar = null;
    }
}
