using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Monster
{
    #region ����
    // ������ �̸�
    [SerializeField]
    protected string bossName;
    public string BossName { get { return bossName; } }

    // ü�¹�
    [SerializeField]
    private Slider hpBar;

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
