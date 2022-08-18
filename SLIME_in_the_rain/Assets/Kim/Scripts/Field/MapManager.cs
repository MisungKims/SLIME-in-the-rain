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
        objectPoolingManager.AllSet();      // 씬이 변경될 때마다

        Slime slime = Slime.Instance;
        slime.RegisterMinimap();
        slime.transform.position = slimeSpawnPos.position;
        slime.canMove = true;
        slime.canAttack = true;
    }

    // TODO:
    // 맵 클리어
    public virtual void ClearMap()
    {
        Debug.Log("Clear Map");
       SceneDesign.Instance.mapClear = true;
    }
}
