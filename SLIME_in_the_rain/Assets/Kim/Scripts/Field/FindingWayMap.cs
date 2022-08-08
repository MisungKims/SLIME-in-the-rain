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
   
    [SerializeField]
    private Transform RoadObject;

    [SerializeField]
    private List<MapArray> mapArrays = new List<MapArray>();      //���� �ش�Ǵ� �̸�



    // ĳ��
    private Slime slime;

    #endregion

    protected override void Awake()
    {

        base.Awake();

        slime = Slime.Instance;


        InitArray();
        SetMap();

        StartCoroutine(DetectFall());
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

    // mapArrays �ʱ�ȭ
    private void InitArray()
    {
        for (int i = 0; i < RoadObject.childCount; i++)
        {
            MapArray m = new MapArray();
            for (int j = 0; j < RoadObject.GetChild(i).childCount; j++)
            {
                m.map.Add(RoadObject.GetChild(i).GetChild(j).gameObject);
            }

            mapArrays.Add(m);
        }
    }

    // �� ����
    private void SetMap()
    {

    }
}
