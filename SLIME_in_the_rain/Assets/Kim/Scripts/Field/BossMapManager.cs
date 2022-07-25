using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMapManager : MapManager
{
    #region 변수
    [SerializeField]
    private Boss boss;

    private WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    #endregion


    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(IsDie());
    }

    // 보스가 죽으면 룬 선택 창을 보여줌
    IEnumerator IsDie()
    {
        while (!boss.isDie)
        {
            yield return null;
        }

        yield return waitFor3s;

        SelectRuneWindow.Instance.OpenWindow();
    }
}
