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
   
    [SerializeField]
    protected Transform slimeSpawnPos;

    // 캐싱
    protected ObjectPoolingManager objectPoolingManager;
    #endregion

    protected virtual void Awake()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;

        Slime.Instance.transform.position = slimeSpawnPos.position;
    }

    // TODO:
    // 맵 클리어
    public virtual void ClearMap()
    {
        Debug.Log("Clear Map");
        // SceneDesign.Instance.mapClear = true;
    }
}
