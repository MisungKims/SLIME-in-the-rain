/**
 * @brief ���� �� �Ŵ���
 * @author ��̼�
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMapManager : MapManager
{
    #region ����
    [SerializeField]
    private Boss boss;

    //[SerializeField]
    //private BossCamera bossCamera;

    // ĳ��
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

    // ������ ������ �� ���� â�� ������
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
