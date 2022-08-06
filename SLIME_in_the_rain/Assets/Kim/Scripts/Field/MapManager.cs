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
    #region 싱글톤
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

    [Header("-------------- Map")]
    public List<Monster> monsters = new List<Monster>();

    // 캐싱
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

    // 재화박스를 스폰
    private void SpawnBox()
    {
        if (!isSpawnBox) return;

        objCount = Random.Range(minObjCount, maxObjCount);
        for (int i = 0; i < objCount; i++)
        {
            objectPoolingManager.Get(EObjectFlag.box, RandomPosition.GetRandomPosition(minX, maxX, minZ, maxZ, 2));
        }
    }

    // 몬스터가 죽었을 때 몬스터 리스트에서 해당 몬스터를 제거
    public void DieMonster(Monster monster)
    {
        if (!monsters.Contains(monster)) return;

        monsters.Remove(monster);

        if (monsters.Count <= 0)            // 모든 몬스터가 죽었으면 맵 클리어
        {
            ClearMap();
        }
    }

    // TODO:
    // 맵 클리어 (출구 나오도록)
    void ClearMap()
    {
        // SceneDesign.Instance.mapClear = true;
    }
}
