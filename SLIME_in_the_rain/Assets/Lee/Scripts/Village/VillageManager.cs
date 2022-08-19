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

    StatManager statManager;
    JellyManager jellyManager;
    RuneManager runeManager;
    
    Inventory inventory;

    ICamera _camera;
    SceneDesign sceneDesign;
    SettingCanvas settingCanvas;
    #endregion

    #region ����Ƽ ����������Ŭ

    private void Start()
    {
        //singletons
        slime = Slime.Instance;

        statManager = StatManager.Instance;
        jellyManager = JellyManager.Instance;
        runeManager = RuneManager.Instance;

        inventory = Inventory.Instance;

        _camera = ICamera.Instance;
        sceneDesign = SceneDesign.Instance;
        settingCanvas = SettingCanvas.Instance;
        Init();
        StartCoroutine(Clear());
        
    }
    
    private void Update()
    {
        #region ī�޶� ���� ���ǹ�
        if (ShopCanvas.activeSelf)       //esc�� ������ �ְ� ��ư���� ������ ����  //���� ��������
        {

            slime.canMove = false;
            _camera.Focus_Shop(this.gameObject);

        }
        else if (TowerCanvas.activeSelf)
        {
            slime.canMove = false;
            _camera.Focus_Slime();
        }
        else
        {
            slime.canMove = true;
            _camera.Focus_Slime();
        }
        #endregion
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

            sceneDesign.SceneInit();
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
    void Init()
    {
        //������
        slime.transform.localScale = Vector3.one;
        slime.InitSlime();
        //����
        statManager.InitStats();
        //����
        if(PlayerPrefs.HasKey("jellyCount"))
        {
            jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        }
        else
        {
            jellyManager.JellyCount = 0;
        }
        //��
        Transform runeSlot = runeManager.gameObject.transform.GetChild(0);
        Vector3 pos;
        pos.x = 22.5f; pos.y = 56.5f; pos.z = 0;
        runeSlot.position = pos;
        runeSlot.localScale = Vector3.one * 0.7f;
        if(!runeManager.transform.GetChild(0).gameObject.activeSelf)
        {
            runeManager.transform.GetChild(0).gameObject.SetActive(true);
        }
        runeManager.InitRune();

        //�κ��丮
        inventory.ResetInven();

        //��������
        sceneDesign.finalClear = false;

        //���� ĵ����
        settingCanvas.settingIcon.SetActive(true);
    }
}
