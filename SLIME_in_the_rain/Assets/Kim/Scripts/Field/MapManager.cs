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
        
        Slime slime = Slime.Instance;
        slime.RegisterMinimap();
        slime.transform.position = slimeSpawnPos.position;
        slime.SetCanAttack();

        UIObjectPoolingManager.Instance.SetHealthBarCanvas();
    }

    // TODO:
    // 맵 클리어
    public virtual void ClearMap()
    {
       SceneDesign.Instance.mapClear = true;
    }
}
