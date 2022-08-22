/**
 * @brief º¸½º ¸Ê ¸Å´ÏÀú
 * @author ±è¹Ì¼º
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMapManager : MapManager
{
    #region º¯¼ö
    [SerializeField]
    private Boss boss;
    [SerializeField]
    private SelectRuneWindow selectRuneWindow;

    //[SerializeField]
    //private BossCamera bossCamera;

    // Ä³½Ì
    private WaitForSeconds waitFor5s = new WaitForSeconds(5f);
    #endregion

    //protected override void Awake()
    //{
    //    base.Awake();

        
    //}

    //IEnumerator Show()
    //{
    //    yield return StartCoroutine(bossCamera.MoveCam());
    //}

    public void DieBoss()
    {
        StartCoroutine(IsDie());
    }

    // º¸½º°¡ Á×À¸¸é ·é ¼±ÅÃ Ã¢À» º¸¿©ÁÜ
    IEnumerator IsDie()
    {
        if (!boss.GetComponent<Metalon>())
        {
            yield return waitFor5s;

            selectRuneWindow.OpenWindow();
            ClearMap();
        }
        else
        {
            yield return waitFor5s;

            ClearMap();
        }
    }
}
