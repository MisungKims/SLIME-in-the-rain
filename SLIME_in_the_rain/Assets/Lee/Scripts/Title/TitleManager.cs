using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button[] buttons;    //���� ���� ����
    GameObject setting;
    //ĳ��
    SettingCanvas settingCanvas;
    SceneDesign sceneDesign;
    RuneManager runeManager;
    LoadManager loadManager;


    private void Start()
    {
        //singleton
        loadManager = LoadManager.Instance;
        runeManager = RuneManager.Instance;
        settingCanvas = SettingCanvas.Instance;
        sceneDesign = SceneDesign.Instance;

        loadManager.Init_Title();
        setting = settingCanvas.transform.GetChild(0).gameObject;

        //OnClick
        int i = 0;
        buttons[i++].onClick.AddListener(delegate { StartButton(); });  //����
        buttons[i++].onClick.AddListener(delegate { SettingButton(); });  //����
        buttons[i++].onClick.AddListener(delegate { ExitGame(); });  //����
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingButton()
    {
        setting.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
