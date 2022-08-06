/**
 * @brief �� �Ŵ���
 * @author ��̼�
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static MapManager instance = null;
    public static MapManager Instance
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

    [SerializeField]
    private Transform slimeSpawnPos;

    [Header("-------------- MoneyBox")]
    [SerializeField]
    private bool isSpawnBox;        // ��ȭ �ڽ��� ������ ������?

    private int objCount;       // ������Ʈ�� ����
    [SerializeField]
    private int minObjCount = 2;
    [SerializeField]
    private int maxObjCount = 5;

    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minZ;
    [SerializeField]
    private float maxZ;

    [Header("-------------- Map")]
    public List<Monster> monsters = new List<Monster>();

    // ĳ��
    private ObjectPoolingManager objectPoolingManager;
    #endregion


    protected virtual void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        objectPoolingManager = ObjectPoolingManager.Instance;

        Slime.Instance.transform.position = slimeSpawnPos.position;


    }

    private IEnumerator Start()
    {
        yield return null;

        SpawnBox();
    }

    // ��ȭ�ڽ��� ����
    private void SpawnBox()
    {
        if (!isSpawnBox) return;

        objCount = Random.Range(minObjCount, maxObjCount);
        for (int i = 0; i < objCount; i++)
        {
            objectPoolingManager.Get(EObjectFlag.box, RandomPosition.GetRandomPosition(minX, maxX, minZ, maxZ, 2));
        }
    }

    // ���Ͱ� �׾��� �� ���� ����Ʈ���� �ش� ���͸� ����
    public void DieMonster(Monster monster)
    {
        if (!monsters.Contains(monster)) return;

        monsters.Remove(monster);

        if (monsters.Count <= 0)            // ��� ���Ͱ� �׾����� �� Ŭ����
        {
            ClearMap();
        }
    }

    // TODO:
    // �� Ŭ���� (�ⱸ ��������)
    void ClearMap()
    {
        // SceneDesign.Instance.mapClear = true;
    }
}
