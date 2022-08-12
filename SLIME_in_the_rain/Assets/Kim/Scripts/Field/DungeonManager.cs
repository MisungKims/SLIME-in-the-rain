/**
 * @brief ���� �Ŵ���
 * @author ��̼�
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MapManager
{
    #region ����
    #region �̱���
    private static DungeonManager instance = null;
    public static DungeonManager Instance
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

    [Header("-------------- MoneyBox")]
    [SerializeField]
    private bool isSpawnBox;        // ��ȭ �ڽ��� ������ ������?

    private int objCount;       // ������ �ڽ��� ����
    [SerializeField]
    private int minObjCount = 2;
    [SerializeField]
    private int maxObjCount = 5;

    public int mapRange;        // ���� ����

    private Vector3 randPos;

    [Header("-------------- Monster")]
    [SerializeField]
    private Transform monstersObject;
    // �ʿ� �ִ� ���͵�
    private List<Monster> monsters = new List<Monster>();
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        base.Awake();

        for (int i = 0; i < monstersObject.childCount; i++)
        {
            monsters.Add(monstersObject.GetChild(i).GetComponent<Monster>());
        }
    }

    private IEnumerator Start()
    {
        yield return null;

        SpawnBox();
    }
    #endregion

    #region �Լ�
    // ��ȭ �ڽ��� ����
    private void SpawnBox()
    {
        if (!isSpawnBox) return;

        objCount = Random.Range(minObjCount, maxObjCount);
        for (int i = 0; i < objCount; i++)
        {
            RandomPosition.GetRandomNavPoint(Vector3.zero, mapRange, out randPos);
            randPos.y = 2;
            objectPoolingManager.Get(EObjectFlag.box, randPos);
        }
    }


    // ���Ͱ� �׾��� �� ����Ʈ���� ����
    public void DieMonster(Monster monster)
    {
        if (!monsters.Contains(monster)) return;

        monsters.Remove(monster);

        if (monsters.Count <= 0)            // ��� ���Ͱ� �׾����� �� Ŭ����
        {
            ClearMap();
        }
    }
    #endregion
}