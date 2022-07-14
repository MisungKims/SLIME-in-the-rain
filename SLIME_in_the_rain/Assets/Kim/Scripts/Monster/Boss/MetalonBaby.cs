/**
 * @brief Metalon�� ���� �Ź�
 * @author ��̼�
 * @date 22-07-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalonBaby : Monster
{
    // ü�¹�
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

    // ü�¹��� ��ġ�� �����ϴ� �ڷ�ƾ
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

    // ü�¹� Ȱ��ȭ
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

    // ü�¹� ��Ȱ��ȭ
    public override void HideHPBar()
    {
        if (!hpBar) return;

        objectPoolingManager.Set(hpBar.gameObject, EObjectFlag.hpBar);
        hpBar = null;
    }
}
