using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    //EditorSceneManager.GetSceneManagerSetup /SceneSetup ������Ʈ ����Ʈ�� ���� �� �ֽ��ϴ�.
    //�׸��� �̸� �� ������ ������ ��Ÿ ������ ���Ҿ� ScriptableObject ������ ����ȭ�� �� �ֽ��ϴ�.
    //������ �����Ϸ��� SceneSetups ����Ʈ�� �ٽ� �����ϰ� EditorSceneManager.RestoreSceneManagerSetup�� Ȱ���Ͻʽÿ�.
    //��Ÿ�� ���� �ε�� ���� ����Ʈ�� �������� sceneCount�� ���� �� GetSceneAt�� ���� �ݺ������� �����մϴ�.
    //���� ������Ʈ�� ���� ���� GameObject.scene���� ���� �� ������ SceneManager.MoveGameObjectToScene�� Ȱ���� ���� ������Ʈ�� ���� ��Ʈ�� �ű� �� �ֽ��ϴ�.
    //������ ���ܵα� ���ϴ� ���� ������Ʈ�� ���������� �����ϴ� �� DontDestroyOnLoad�� ����ϴ� ���� �������� �ʽ��ϴ�.
    //��� ��� �Ŵ����� �ִ� manager scene�� �����ϰ� ���� ���μ����� ������ �� SceneManager.LoadScene(<path>, LoadSceneMode.Additive)��SceneManager.UnloadScene�� Ȱ���Ͻʽÿ�.

    //Start�� OnEnable�� �ҽ� ���� �����̶� �̱��� �ȵ�
    #region �̱���
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
        //�̱��� SetActive
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
        //�̱��� SetActive
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

        //������
        slime.InitSlime();
        slime.transform.localScale = Vector3.one;


        //����  
        statManager.InitStats();

        //��
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
        if (PlayerPrefs.HasKey("jellyCount"))
        {
            jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        }
        else
        {
            jellyManager.JellyCount = 0;
        }

        //�κ��丮
        inventory.ResetInven();

        //��������
        sceneDesign.finalClear = false;

    }
}
