/**
 * @brief º¸½º ¸Ê ¸Å´ÏÀú
 * @author ±è¹Ì¼º
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMapManager : MapManager
{
    #region º¯¼ö
    [SerializeField]
    private Boss boss;

    //[SerializeField]
    //private BossCamera bossCamera;

    // Ä³½Ì
    private WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    #endregion

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(IsDie());


    }

    //IEnumerator Show()
    //{
    //    yield return StartCoroutine(bossCamera.MoveCam());
    //}

    // º¸½º°¡ Á×À¸¸é ·é ¼±ÅÃ Ã¢À» º¸¿©ÁÜ
    IEnumerator IsDie()
    {
        while (!boss.isDie)
        {
            yield return null;
        }

        yield return waitFor3s;

        SelectRuneWindow.Instance.OpenWindow();
        ClearMap();
    }
}
