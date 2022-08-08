/**
 * @brief �� ã�� ��
 * @author ��̼�
 * @date 22-08-08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapArray //�࿡ �ش�Ǵ� �̸�
{
    public List<GameObject> map = new List<GameObject>();
}

public class FindingWayMap : MapManager
{
    #region ����
    #region �̱���
    private static FindingWayMap instance = null;
    public static FindingWayMap Instance
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

    private string trapTag = "Trap";
    private string roadTag = "Road";

    private int width;
    private int height;

    private int w;
    private int h;
    private int startIdx;
    private int rand;

    public bool isClear;

    enum EDirection { left, forward, right };
    EDirection direction;

    [SerializeField]
    private Transform RoadObject;

    [SerializeField]
    private List<MapArray> mapArrays = new List<MapArray>();      //���� �ش�Ǵ� �̸�

    Stack<GameObject> roadStack = new Stack<GameObject>();

    // ĳ��
    private Slime slime;
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

        slime = Slime.Instance;

        isClear = false;

        InitArray();
        SetMap();

        StartCoroutine(DetectFall());
    }
    #endregion

    #region �ڷ�ƾ
    // �������� �������� �ʱ� ��ġ�� �̵�
    IEnumerator DetectFall()
    {
        while (true)
        {
            if (slime.IsInWater)
            {
                slime.transform.position = slimeSpawnPos.position;
            }

            yield return null;
        }
    }
    #endregion

    #region �Լ�
    // mapArrays �ʱ�ȭ
    private void InitArray()
    {
        width = RoadObject.childCount;
        height = RoadObject.GetChild(0).childCount;

        for (int i = 0; i < width; i++)
        {
            MapArray m = new MapArray();
            for (int j = 0; j < height; j++)
            {
                GameObject obj = RoadObject.GetChild(i).GetChild(j).gameObject;
                obj.tag = trapTag;
                m.map.Add(obj);
            }

            mapArrays.Add(m);
        }
    }

    // �� ����
    private void SetMap()
    {
        // �������� ������ �������� ����
        startIdx = Random.Range(0, width);

        mapArrays[width - 1].map[startIdx].tag = roadTag;       // ������

        w = width - 1;      // ��
        h = startIdx;       // ��

        Debug.Log(w + " " + h);

        // �������� ���� ����
        while (w > 0)
        {
            GenerateRoad();
        }
    }

    // �� ����
    private void GenerateRoad()
    {
        rand = Random.Range(0, 3);
        direction = (EDirection)rand;
        GameObject obj;

        // �� �� �ִ� ��(����, ������, �� ����)���� ���� ������

        switch (direction)
        {
            case EDirection.left:

                if (h > 0)
                {
                    obj = mapArrays[w].map[--h].gameObject;

                    if (roadStack.Contains(obj))
                    {
                        h++;
                        break;
                    }

                    obj.tag = roadTag;
                    roadStack.Push(obj);
                }
                break;
            case EDirection.forward:
                obj = mapArrays[--w].map[h].gameObject;

                if (roadStack.Contains(obj))
                {
                    w--;
                    break;
                }

                obj.tag = roadTag;
                roadStack.Push(obj);

                break;
            case EDirection.right:
                if (h < height - 1)
                {
                    obj = mapArrays[w].map[++h].gameObject;

                    if (roadStack.Contains(obj))
                    {

                        h--;
                        break;
                    }

                    obj.tag = roadTag;
                    roadStack.Push(obj);
                }

                break;
        }
    }
    #endregion
}
