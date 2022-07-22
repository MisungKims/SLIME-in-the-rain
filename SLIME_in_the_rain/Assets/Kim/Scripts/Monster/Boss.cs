using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Monster
{
    #region 변수
    // 보스의 이름
    [SerializeField]
    protected string bossName;
    public string BossName { get { return bossName; } }

    // 체력바
    [SerializeField]
    private Slider hpBar;

    // 젤리 드롭
    private int randJellyCount;
    private int minJellyCnt = 8;
    private int maxJellyCnt = 15;

    private GameObject jelly;
    private Vector3 jellyPos;

    // 캐싱
    private WaitForSeconds waitFor6s = new WaitForSeconds(6f);
    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        minAtkTime = 0.5f;
        maxAtkTime = 1.5f;
    }
    #endregion

    #region 코루틴
    protected override IEnumerator DieCoroutine()
    {
        yield return waitFor6s;

        this.gameObject.SetActive(false);
    }
    #endregion

    #region 함수

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

        // 젤리 드롭
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
