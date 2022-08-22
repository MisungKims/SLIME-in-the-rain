using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    //EditorSceneManager.GetSceneManagerSetup /SceneSetup 오브젝트 리스트를 얻을 수 있습니다.
    //그리고 이를 씬 설정에 저장할 기타 정보와 더불어 ScriptableObject 등으로 직렬화할 수 있습니다.
    //계층을 복원하려면 SceneSetups 리스트를 다시 생성하고 EditorSceneManager.RestoreSceneManagerSetup를 활용하십시오.
    //런타임 동안 로드된 씬의 리스트를 얻으려면 sceneCount를 받은 후 GetSceneAt를 씬에 반복적으로 적용합니다.
    //게임 오브젝트가 속한 씬을 GameObject.scene으로 얻을 수 있으며 SceneManager.MoveGameObjectToScene을 활용해 게임 오브젝트를 씬의 루트로 옮길 수 있습니다.
    //씬에서 남겨두기 원하는 게임 오브젝트를 지속적으로 관리하는 데 DontDestroyOnLoad를 사용하는 것은 권장하지 않습니다.
    //대신 모든 매니저가 있는 manager scene을 생성하고 게임 프로세스를 관리할 때 SceneManager.LoadScene(<path>, LoadSceneMode.Additive)과SceneManager.UnloadScene을 활용하십시오.

    //Start랑 OnEnable로 소스 돌릴 예정이라 싱글톤 안됨
    #region 싱글톤
    private static LoadManager instance = null;
    public static LoadManager Instance
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
    Slime slime;
    [SerializeField]
    StatManager statManager;
    [SerializeField]
    UIObjectPoolingManager uIObjectPoolingManager;
    [SerializeField]
    RuneManager runeManager;
    [SerializeField]
    ItemDatabase itemDatabase;
    [SerializeField]
    JellyManager jellyManager;
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    InventoryUI inventoryUI;
    [SerializeField]
    SceneDesign sceneDesign;
    [SerializeField]
    SettingCanvas settingCanvas;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public void Init_Title()
    {
        //싱글톤 SetActive
        //true
        jellyManager.gameObject.SetActive(true);
        sceneDesign.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(true);
        //false
        slime.gameObject.SetActive(false);
        statManager.gameObject.SetActive(false);
        uIObjectPoolingManager.gameObject.SetActive(false);
        runeManager.gameObject.SetActive(false);
        itemDatabase.gameObject.SetActive(false);
        inventory.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(false);
    }

    public void Init_Village()
    {
        //싱글톤 SetActive
        //true
        slime.gameObject.SetActive(true);
        statManager.gameObject.SetActive(true);
        jellyManager.gameObject.SetActive(true);
        sceneDesign.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(true);
        uIObjectPoolingManager.gameObject.SetActive(true);
        runeManager.gameObject.SetActive(true);
        itemDatabase.gameObject.SetActive(true);
        inventory.gameObject.SetActive(true);
        inventoryUI.gameObject.SetActive(true);

        //슬라임
        slime.InitSlime();
        slime.transform.localScale = Vector3.one;


        //스탯  
        statManager.InitStats();

        //룬
        Transform runeSlot = runeManager.gameObject.transform.GetChild(0);
        Vector3 pos;
        pos.x = 22.5f; pos.y = 56.5f; pos.z = 0;
        runeSlot.position = pos;
        runeSlot.localScale = Vector3.one * 0.7f;
        if (!runeManager.transform.GetChild(0).gameObject.activeSelf)
        {
            runeManager.transform.GetChild(0).gameObject.SetActive(true);
        }
        runeManager.InitRune();

        //젤리
        if (PlayerPrefs.HasKey("jellyCount"))
        {
            jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        }
        else
        {
            jellyManager.JellyCount = 0;
        }

        //인벤토리
        inventory.ResetInven();

        //씬디자인
        sceneDesign.finalClear = false;

    }
}
