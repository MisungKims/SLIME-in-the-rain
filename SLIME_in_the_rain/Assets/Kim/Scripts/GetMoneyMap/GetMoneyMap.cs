/**
 * @brief �ʿ� �������� ��ȭ�� ����
 * @author ��̼�
 * @date 22-07-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoneyMap : RandomPosition
{
    #region ����
    [SerializeField]
    private Transform slimeSpawnPos;

    private int jellyIndex = (int)EObjectFlag.jelly;
    private int gelatinIndex = (int)EObjectFlag.gelatin;
    private int randObj;

    [SerializeField]
    private int objCount = 70;       // ������Ʈ�� ����

    private ObjectPoolingManager objectPoolingManager;
    #endregion

    private void Awake()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;

        Slime.Instance.transform.position = slimeSpawnPos.position;

        minX = -10f;
        maxX = 10f;
        minZ = -8f;
        maxZ = 10f;
    }

    private IEnumerator Start()
    {
        yield return null;

        for (int i = 0; i < objCount; i++)
        {
            randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // �������� ����, ����ƾ�� ���Ͽ� �ʿ� ������

            objectPoolingManager.Get((EObjectFlag)randObj, GetRandomPosition());
        }
    }
}
