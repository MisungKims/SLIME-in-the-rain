using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MapManager
{
    #region 변수

    public GameObject ShopCanvas;
    public GameObject TowerCanvas;

    //cashing
    Slime slime;
    StatManager statManager;
    JellyManager jellyManager;
    RuneManager runeManager;
    SingletonManager singletonManager;

    Inventory inventory;

    ICamera _camera;
    SceneDesign sceneDesign;
    SettingCanvas settingCanvas;
    #endregion

    #region 유니티 라이프사이클

    private void Start()
    {
        //singletons
        singletonManager = SingletonManager.Instance;
        slime = Slime.Instance;

        statManager = StatManager.Instance;
        jellyManager = JellyManager.Instance;
        runeManager = RuneManager.Instance;

        inventory = Inventory.Instance;

        _camera = ICamera.Instance;
        sceneDesign = SceneDesign.Instance;
        settingCanvas = SettingCanvas.Instance;
        singletonManager.Init_Village();
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
