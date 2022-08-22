/**
 * @brief ���� �� �Ŵ���
 * @author ��̼�
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMapManager : MapManager
{
    #region ����
    [SerializeField]
    private Boss boss;
    [SerializeField]
    private SelectRuneWindow selectRuneWindow;

    //[SerializeField]
    //private BossCamera bossCamera;

    // ĳ��
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

    // ������ ������ �� ���� â�� ������
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
