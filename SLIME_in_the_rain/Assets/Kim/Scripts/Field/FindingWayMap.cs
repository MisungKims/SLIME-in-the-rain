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
   
    [SerializeField]
    private Transform RoadObject;

    [SerializeField]
    private List<MapArray> mapArrays = new List<MapArray>();      //열에 해당되는 이름



    // 캐싱
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

    // mapArrays 초기화
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

    // 맵 설정
    private void SetMap()
    {

    }
}
