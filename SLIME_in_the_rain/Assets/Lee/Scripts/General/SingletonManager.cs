using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{
    #region �̱���
    private static SingletonManager instance = null;
    public static SingletonManager Instance
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

        //�̱��� SetActive: true
        //true
        jellyManager.gameObject.SetActive(true);
        sceneDesign.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(true);

        //////////////Manager////////////
        
        //////////////UI////////////
        //���� ������
        settingCanvas.settingIcon.SetActive(false);

        //UIǮ���Ŵ���
        uIObjectPoolingManager.InitUI();

        //�̱��� SetActive: false
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
        //�̱��� SetActive: true
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

        //////////////Manager////////////
        //������
        slime.InitSlime();
        slime.transform.localScale = Vector3.one;

        //����  
        statManager.InitStats();


        //����
        if (PlayerPrefs.HasKey("jellyCount"))
        {
            jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        }
        else
        {
            jellyManager.JellyCount = 0;
        }
        
        //��������
        sceneDesign.finalClear = false;


        //////////////UI////////////
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

        //�κ��丮
        inventory.ResetInven();

        //UIǮ��
        uIObjectPoolingManager.hpSlime.transform.parent.gameObject.SetActive(true);

        //���� ������
        settingCanvas.settingIcon.SetActive(true);
    }

    public void Init_Result()
    {
        //////////////Manager////////////
        //������
        slime.transform.localScale = Vector3.one * 500f;

        //////////////UI////////////
        //��
        Transform runeSlot = runeManager.gameObject.transform.GetChild(0);
        Vector3 pos;
        pos.x = 410f; pos.y = 250f; pos.z = 0;
        runeSlot.localScale = Vector3.one * 1.2f;
        if (!runeManager.transform.GetChild(0).gameObject.activeSelf)
        {
            runeManager.transform.GetChild(0).gameObject.SetActive(true);
        }
        //UIǮ���Ŵ���
        uIObjectPoolingManager.InitUI();
        //���� ������
        settingCanvas.settingIcon.SetActive(false);

    }
}