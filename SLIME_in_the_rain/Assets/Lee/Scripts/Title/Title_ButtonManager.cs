using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_ButtonManager : MonoBehaviour
{
    public Button[] buttons;    //���� ���� ����
    GameObject setting;
    //ĳ��
    SettingCanvas settingCanvas;
    SceneDesign sceneDesign;


    private void Start()
    {
        //singleton
        settingCanvas = SettingCanvas.Instance;
        sceneDesign = SceneDesign.Instance;

        settingCanvas.settingIcon.SetActive(false);
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
