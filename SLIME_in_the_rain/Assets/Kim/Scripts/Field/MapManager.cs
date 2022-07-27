/**
 * @brief 맵 매니저
 * @author 김미성
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region 변수

    #endregion
    [SerializeField]
    private Transform slimeSpawnPos;

    [SerializeField]
    private bool isSpawnBox;        // 재화 박스를 스폰할 것인지?

    private int objCount;       // 오브젝트의 개수
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

    // 캐싱
    private ObjectPoolingManager objectPoolingManager;

    protected virtual void Awake()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;

        Slime.Instance.transform.position = slimeSpawnPos.position;

    }

    private IEnumerator Start()
    {
        yield return null;

        SpawnBox();
    }

    private void SpawnBox()
    {
        if (!isSpawnBox) return;

        objCount = Random.Range(minObjCount, maxObjCount);
        for (int i = 0; i < objCount; i++)
        {
          
            objectPoolingManager.Get(EObjectFlag.box, RandomPosition.GetRandomPosition(minX, maxX, minZ, maxZ, 2));
        }
    }
}
