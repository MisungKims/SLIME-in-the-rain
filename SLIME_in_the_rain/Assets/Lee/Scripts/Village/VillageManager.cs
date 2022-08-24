using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MapManager
{
    #region ����

    public GameObject ShopCanvas;
    public GameObject TowerCanvas;

    //cashing
    Slime slime;
    JellyManager jellyManager;
    SingletonManager singletonManager;
    ICamera _camera;
    SceneDesign sceneDesign;
    #endregion

    #region ����Ƽ ����������Ŭ

    private void Start()
    {
        //singletons
        singletonManager = SingletonManager.Instance;
        slime = Slime.Instance;
        jellyManager = JellyManager.Instance;
        _camera = ICamera.Instance;
        sceneDesign = SceneDesign.Instance;
        
        StartCoroutine(Clear());
    }
    IEnumerator Clear()
    {
        while (!slime.currentWeapon)
        {
            yield return null;
        }
        ClearMap();
    }
    void OnDisable()
    {
        if (sceneDesign)
        {

            sceneDesign.VillageSceneInit();
            Debug.Log("Execution Reset");
        }
        else
        {
            //Debug.Log("Null SceneDesign instance");
        }

        if (jellyManager)
        {
            sceneDesign.jellyInit = jellyManager.JellyCount;
            Debug.Log("Execution sceneDesign.jellyInit");
        }
        else
        {
            //Debug.Log("Null JellyManager instance");
        }
    }
    #endregion
}
