using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class Boss : Monster
{
    #region ����
    // ������ �̸�
    protected string bossName;
    [SerializeField]
    private TextMeshProUGUI bossNameText;

    // ü�¹�
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private TextMeshProUGUI hPText;
    StringBuilder sb = new StringBuilder();

    // ���� ���
    private int randJellyCount;
    private int minJellyCnt = 8;
    private int maxJellyCnt = 15;

    private GameObject jelly;
    private Vector3 jellyPos;

    // ĳ��
    private WaitForSeconds waitFor6s = new WaitForSeconds(6f);
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        minAtkTime = 0.5f;
        maxAtkTime = 1.5f;
    }
    #endregion

    #region �ڷ�ƾ
    protected override IEnumerator DieCoroutine()
    {
        yield return waitFor6s;

        this.gameObject.SetActive(false);
    }
    #endregion

    #region �Լ�
    // HP�� ����
    protected void SetHPBar()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
        }

        bossNameText.text = bossName;
        hpBar.maxValue = stats.maxHP;
        ShowHPBar();
    }

    public override void ShowHPBar()
    {
        hpBar.value = stats.HP;

        sb.Clear();
        sb.Append(hpBar.value);
        sb.Append("/");
        sb.Append(hpBar.maxValue);
        hPText.text = sb.ToString();
    }

    public override void HideHPBar()
    {
        hpBar.gameObject.SetActive(false);
    }

    protected override void Die()
    {
        base.Die();

        // ���� ���
        randJellyCount = Random.Range(minJellyCnt, maxJellyCnt);
        for (int i = 0; i < randJellyCount; i++)
        {
            jelly = objectPoolingManager.Get(EObjectFlag.jelly);

            jellyPos = transform.position;
            jellyPos.x += Random.Range(-1f, 1f);
            jellyPos.y += 3f;
            jellyPos.z += Random.Range(-1f, 1f);

            jelly.transform.position = jellyPos;
        }
    }
    #endregion
}
