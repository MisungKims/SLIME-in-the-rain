/**
 * @brief �ʿ� �������� ��ȭ�� ����
 * @author ��̼�
 * @date 22-07-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoneyMap : MonoBehaviour
{
    #region ����
    private int jellyIndex = (int)EObjectFlag.jelly;
    private int gelatinIndex = (int)EObjectFlag.gelatin;
    private int randObj;

    private float minX = -8f;
    private float maxX = 8f;
    private float minZ = -6f;
    private float maxZ = 8f;

    [SerializeField]
    private int objCount = 70;       // ������Ʈ�� ����

    private ObjectPoolingManager objectPoolingManager;
    #endregion

    private void Awake()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    private IEnumerator Start()
    {
        yield return null;

        for (int i = 0; i < objCount; i++)
        {
            randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // �������� ����, ����ƾ�� ���Ͽ� �ʿ� ������

            objectPoolingManager.Get((EObjectFlag)randObj, RandomPosition.GetRandomPosition(minX, maxX, minZ, maxZ, 2.5f));
        }
    }
}
