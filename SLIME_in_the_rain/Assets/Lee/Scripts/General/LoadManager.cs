using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    //Start�� OnEnable�� �ҽ� ���� �����̶� �̱��� ����
    //EditorSceneManager.GetSceneManagerSetup /SceneSetup ������Ʈ ����Ʈ�� ���� �� �ֽ��ϴ�.
    //�׸��� �̸� �� ������ ������ ��Ÿ ������ ���Ҿ� ScriptableObject ������ ����ȭ�� �� �ֽ��ϴ�.
    //������ �����Ϸ��� SceneSetups ����Ʈ�� �ٽ� �����ϰ� EditorSceneManager.RestoreSceneManagerSetup�� Ȱ���Ͻʽÿ�.

    //��Ÿ�� ���� �ε�� ���� ����Ʈ�� �������� sceneCount�� ���� �� GetSceneAt�� ���� �ݺ������� �����մϴ�.

    //���� ������Ʈ�� ���� ���� GameObject.scene���� ���� �� ������ SceneManager.MoveGameObjectToScene�� Ȱ���� ���� ������Ʈ�� ���� ��Ʈ�� �ű� �� �ֽ��ϴ�.

    //������ ���ܵα� ���ϴ� ���� ������Ʈ�� ���������� �����ϴ� �� DontDestroyOnLoad�� ����ϴ� ���� �������� �ʽ��ϴ�.
    //��� ��� �Ŵ����� �ִ� manager scene�� �����ϰ� ���� ���μ����� ������ �� SceneManager.LoadScene(<path>, LoadSceneMode.Additive)��SceneManager.UnloadScene�� Ȱ���Ͻʽÿ�.

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
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
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
        //����
        statManager.enabled = true;    
        statManager.InitStats();

        uIObjectPoolingManager.enabled = true;
        //��
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
        
        //����
        jellyManager.enabled = true;
        
        if (PlayerPrefs.HasKey("jellyCount"))
        {
            jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        }
        else
        {
            jellyManager.JellyCount = 0;
        }
        //�κ��丮
        inventory.enabled = true;
        inventory.ResetInven();
        //��������
        sceneDesign.enabled = true;
        sceneDesign.finalClear = false;
        //���� ĵ����
        settingCanvas.enabled = true;
        settingCanvas.settingIcon.SetActive(true);

        itemDatabase.enabled = true;

    }
}
