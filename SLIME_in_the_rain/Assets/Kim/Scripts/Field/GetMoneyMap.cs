/**
 * @brief 맵에 랜덤으로 재화를 스폰
 * @author 김미성
 * @date 22-07-24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetMoneyMap : MapManager
{
    #region 변수
    private int jellyIndex = (int)EObjectFlag.jelly;
    private int gelatinIndex = (int)EObjectFlag.gelatin;
    private int randObj;

    [Header("-------------- Get Money Map")]
    [SerializeField]
    private int spawnRange;
    private Vector3 randPos;

    [SerializeField]
    private int objCount = 70;       // 오브젝트의 개수

    // 타임 카운트
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

    // 캐싱
    private ObjectPoolingManager objectPoolingManager;
    #endregion


    private IEnumerator Start()
    {
        yield return null;

        objectPoolingManager = ObjectPoolingManager.Instance;

        StartCoroutine(TimeCount());

        for (int i = 0; i < objCount; i++)
        {
            randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // 랜덤으로 젤리, 젤라틴을 정하여 맵에 가져옴

            RandomPosition.GetRandomNavPoint(Vector3.zero, 10, out randPos);
            randPos.y = 2;
            objectPoolingManager.Get((EObjectFlag)randObj, randPos);
        }
    }

    // 시간을 세는 코루틴
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
