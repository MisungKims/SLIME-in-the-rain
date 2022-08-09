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
    private List<MapArray> mapArrays = new List<MapArray>();      // Road ������Ʈ�� �迭

    private List<GameObject> roadList = new List<GameObject>();

    // ī�޶�
    private Camera mainCam;
    [SerializeField]
    private Camera movingCamera;
    [SerializeField]
    private Transform startCamPos;
    [SerializeField]
    private Transform endCamPos;

    private bool canMoveCam = true;
    private Vector3 offset;
    private float distance;

    [SerializeField]
    private GameObject wall;

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
        mainCam = Camera.main;

        movingCamera.enabled = true;
        movingCamera.transform.localPosition = startCamPos.localPosition;

        wall.SetActive(false);

        isClear = false;

        InitArray();
        SetMap();

        StartCoroutine(ShowRoad());
        
        StartCoroutine(DetectFall());
    }
    #endregion

    #region �ڷ�ƾ

    // ���� ���۵� �� ���� �˷���
    IEnumerator ShowRoad()
    {
        slime.canMove = false;
        canMoveCam = true;
        StartCoroutine(MoveCamera());

        for (int i = 0; i < roadList.Count; i++)
        {
            yield return new WaitForSeconds(0.2f);

            roadList[i].GetComponent<RoadObject>().ChangeMesh(true);
        }

        yield return new WaitForSeconds(1.5f);

        canMoveCam = false;
        for (int i = 0; i < roadList.Count; i++)
        {
            roadList[i].GetComponent<RoadObject>().ChangeMesh(false);
        }
    }

    // ī�޶� ������ ������
    IEnumerator MoveCamera()
    {
        yield return new WaitForSeconds(0.5f);

        offset = movingCamera.transform.localPosition - endCamPos.localPosition;
        distance = offset.sqrMagnitude;

        while (distance > 0.5f && canMoveCam)
        {
            movingCamera.transform.localPosition = Vector3.Lerp(movingCamera.transform.localPosition, endCamPos.localPosition, Time.deltaTime * 0.3f);

            yield return null;
        }

        movingCamera.enabled = false;
        slime.canMove = true;
    }

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

        GameObject obj = mapArrays[width - 1].map[startIdx];
        obj.tag = roadTag;       // ������
        roadList.Add(obj);

        w = width - 1;      // ��
        h = startIdx;       // ��

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

                    if (roadList.Contains(obj))
                    {
                        h++;
                        break;
                    }

                    obj.tag = roadTag;
                    roadList.Add(obj);
                }
                break;
            case EDirection.forward:
                obj = mapArrays[--w].map[h].gameObject;

                if (roadList.Contains(obj))
                {
                    w--;
                    break;
                }

                obj.tag = roadTag;
                roadList.Add(obj);

                break;
            case EDirection.right:
                if (h < height - 1)
                {
                    obj = mapArrays[w].map[++h].gameObject;

                    if (roadList.Contains(obj))
                    {

                        h--;
                        break;
                    }

                    obj.tag = roadTag;
                    roadList.Add(obj);
                }

                break;
        }
    }

    // �� Ŭ���� �� ȣ��
    public override void ClearMap()
    {
        base.ClearMap();

        wall.SetActive(true);
    }
    #endregion
}
