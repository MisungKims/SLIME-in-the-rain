using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMapManager : MapManager
{
    #region ����
    [SerializeField]
    private Boss boss;

    private WaitForSeconds waitFor3s = new WaitForSeconds(3f);
    #endregion


    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(IsDie());
    }

    // ������ ������ �� ���� â�� ������
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
