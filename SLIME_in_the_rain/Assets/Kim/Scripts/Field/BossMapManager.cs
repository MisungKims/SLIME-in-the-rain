/**
 * @brief 보스 맵 매니저
 * @author 김미성
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMapManager : MapManager
{
    #region 변수
    #region 싱글톤
    private static BossMapManager instance = null;
    public static BossMapManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public Boss boss;
    [SerializeField]
    private SelectRuneWindow selectRuneWindow;

    [Header("-------------- Monster")]
    [SerializeField]
    private Transform monstersObject;

    // 캐싱
    private WaitForSeconds waitFor5s = new WaitForSeconds(5f);
    SoundManager sound;
    #endregion

    protected override void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        base.Awake();
        sound = SoundManager.Instance;
        sound.Play("Boss", SoundType.BGM);
        StartCoroutine(SoundFaster());
    }

    public void DieBoss()
    {
        StartCoroutine(IsDie());
    }

    // 보스가 죽으면 룬 선택 창을 보여줌
    IEnumerator IsDie()
    {
        yield return waitFor5s;

        if (!boss.GetComponent<Metalon>())
        {
            selectRuneWindow.OpenWindow();
        }
        StartCoroutine(sound.BGMPitchReset());                     ///////////////////추가 빨라진 브금 리셋 -TG
        ClearMap();
    }

    public void SetMonsterHPBar()
    {
        boss.HideHPBar();

        if(monstersObject)
        {
            for (int i = 0; i < monstersObject.childCount; i++)
            {
                if(monstersObject.GetChild(i).gameObject.activeSelf)
                {
                    monstersObject.GetChild(i).GetComponent<Monster>().HideHPBar();
                }
            }
        }
    }

    public void ShowBossHPBar()
    {
        boss.SetHPBar();
    }
    
    ///////////////////////////////////////////추가 브금 빠르게!
    IEnumerator SoundFaster()
    {
        while ((boss.Stats.HP / boss.Stats.maxHP * 100f) > 50f)
        {
            yield return null;
        }
        StartCoroutine(sound.BGMFaster());
    }

    /////////////////////////////////////////
}
