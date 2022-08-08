/**
 * @brief 길 찾기 맵
 * @author 김미성
 * @date 22-08-08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapArray //행에 해당되는 이름
{
    public List<GameObject> map = new List<GameObject>();
}

public class FindingWayMap : MapManager
{
    #region 변수
    #region 싱글톤
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
    private List<MapArray> mapArrays = new List<MapArray>();      //열에 해당되는 이름

    Stack<GameObject> roadStack = new Stack<GameObject>();

    // 캐싱
    private Slime slime;
    #endregion

    #region 유니티 함수
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

    #region 코루틴
    // 슬라임이 떨어지면 초기 위치로 이동
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

    #region 함수
    // mapArrays 초기화
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

    // 맵 설정
    private void SetMap()
    {
        // 시작점과 끝점을 랜덤으로 설정
        startIdx = Random.Range(0, width);

        mapArrays[width - 1].map[startIdx].tag = roadTag;       // 시작점

        w = width - 1;      // 행
        h = startIdx;       // 열

        Debug.Log(w + " " + h);

        // 랜덤으로 길을 생성
        while (w > 0)
        {
            GenerateRoad();
        }
    }

    // 길 생성
    private void GenerateRoad()
    {
        rand = Random.Range(0, 3);
        direction = (EDirection)rand;
        GameObject obj;

        // 갈 수 있는 길(왼쪽, 오른쪽, 앞 방향)으로 길을 생성함

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
