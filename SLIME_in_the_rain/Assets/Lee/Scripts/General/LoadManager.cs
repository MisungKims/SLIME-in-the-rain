using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    //Start랑 OnEnable로 소스 돌릴 예정이라 싱글톤 ㄴㄴ
    //EditorSceneManager.GetSceneManagerSetup /SceneSetup 오브젝트 리스트를 얻을 수 있습니다.
    //그리고 이를 씬 설정에 저장할 기타 정보와 더불어 ScriptableObject 등으로 직렬화할 수 있습니다.
    //계층을 복원하려면 SceneSetups 리스트를 다시 생성하고 EditorSceneManager.RestoreSceneManagerSetup를 활용하십시오.

    //런타임 동안 로드된 씬의 리스트를 얻으려면 sceneCount를 받은 후 GetSceneAt를 씬에 반복적으로 적용합니다.

    //게임 오브젝트가 속한 씬을 GameObject.scene으로 얻을 수 있으며 SceneManager.MoveGameObjectToScene을 활용해 게임 오브젝트를 씬의 루트로 옮길 수 있습니다.

    //씬에서 남겨두기 원하는 게임 오브젝트를 지속적으로 관리하는 데 DontDestroyOnLoad를 사용하는 것은 권장하지 않습니다.
    //대신 모든 매니저가 있는 manager scene을 생성하고 게임 프로세스를 관리할 때 SceneManager.LoadScene(<path>, LoadSceneMode.Additive)과SceneManager.UnloadScene을 활용하십시오.

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
    SceneDesign sceneDesign;
    [SerializeField]
    SettingCanvas settingCanvas;


    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Init_Title();
        }
        if(scene.buildIndex == 1)
        {
            Init_Village();
        }

        Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init_Title()
    {
        //true
        jellyManager.enabled = true;
        sceneDesign.enabled = true;
        settingCanvas.enabled = true;
        
        slime.enabled = false;
        statManager.enabled = false;
        uIObjectPoolingManager.enabled = false;
        runeManager.enabled = false;
        itemDatabase.enabled = false;
        inventory.enabled = false;
    }
    void Init_Village()
    {
        //All true
        slime.enabled = true;
        slime.InitSlime();
        slime.transform.localScale = Vector3.one;
        //스탯
        statManager.enabled = true;    
        statManager.InitStats();

        uIObjectPoolingManager.enabled = true;
        //룬
        runeManager.enabled = true;
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
        jellyManager.enabled = true;
        
        if (PlayerPrefs.HasKey("jellyCount"))
        {
            jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        }
        else
        {
            jellyManager.JellyCount = 0;
        }
        //인벤토리
        inventory.enabled = true;
        inventory.ResetInven();
        //씬디자인
        sceneDesign.enabled = true;
        sceneDesign.finalClear = false;
        //세팅 캔버스
        settingCanvas.enabled = true;
        settingCanvas.settingIcon.SetActive(true);

        itemDatabase.enabled = true;

    }
}
