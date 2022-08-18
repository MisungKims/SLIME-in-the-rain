using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MapManager           
{
    #region ����
    
    public GameObject ShopCanvas;
    public GameObject TowerCanvas;

    private Transform runeSlot;

    //singletons
    Slime slime;
    ICamera _camera;
    SceneDesign sceneDesign;
    JellyManager jellyManager;
    SettingCanvas settingCanvas;
    RuneManager runeManager;


    #endregion

    #region ����Ƽ ����������Ŭ

    private void Start()
    {
        //singletons
        sceneDesign = SceneDesign.Instance;
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        jellyManager = JellyManager.Instance;
        settingCanvas = SettingCanvas.Instance;
        runeManager = RuneManager.Instance;

        runeSlot = runeManager.gameObject.transform.GetChild(0);

        
        Init();
        StartCoroutine(Clear());
        settingCanvas.settingIcon.SetActive(true);
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
        while(!slime.currentWeapon)
        {
            yield return null;
        }
        ClearMap();
    }
    void OnDisable()
    {
        if (sceneDesign)
        {

            sceneDesign.ResetScene();
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
        slime.currentWeapon = null;
        //�齽��
        Vector3 pos;
        pos.x = 22.5f; pos.y = 56.5f; pos.z = 0;
        runeSlot.position = pos;
        runeSlot.localScale = Vector3.one * 0.7f;
        //����
    }
}
