/**
 * @brief �ʿ� �������� ��ȭ�� ����
 * @author ��̼�
 * @date 22-07-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetMoneyMap : MapManager
{
    #region ����
    private int jellyIndex = (int)EObjectFlag.jelly;
    private int gelatinIndex = (int)EObjectFlag.gelatin;
    private int randObj;

    [Header("-------------- Get Money Map")]
    [SerializeField]
    private int spawnRange;
    private Vector3 randPos;

    [SerializeField]
    private int objCount = 70;       // ������Ʈ�� ����

    // Ÿ�� ī��Ʈ
    [SerializeField]
    private TextMeshProUGUI secondText;
    private int second;
    public int Second 
    { 
        get { return second; }
        set 
        { 
            second = value;
            secondText.text = second.ToString();
        } 
    }

    // ĳ��
    private ObjectPoolingManager objectPoolingManager;
    #endregion


    private IEnumerator Start()
    {
        yield return null;

        objectPoolingManager = ObjectPoolingManager.Instance;

        StartCoroutine(TimeCount());

        for (int i = 0; i < objCount; i++)
        {
            randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // �������� ����, ����ƾ�� ���Ͽ� �ʿ� ������

            RandomPosition.GetRandomNavPoint(Vector3.zero, 10, out randPos);
            randPos.y = 2;
            objectPoolingManager.Get((EObjectFlag)randObj, randPos);
        }
    }

    // �ð��� ���� �ڷ�ƾ
    IEnumerator TimeCount()
    {
        Second = 60;

        while (second > 0)
        {
            yield return new WaitForSeconds(1f);

            Second--;
        }

        ClearMap();
    }
}
