using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_ButtonManager : MonoBehaviour
{
    public Button[] buttons;    //시작 설정 종료
    GameObject setting;
    //캐싱
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
        buttons[i++].onClick.AddListener(delegate { StartButton(); });  //시작
        buttons[i++].onClick.AddListener(delegate { SettingButton(); });  //설정
        buttons[i++].onClick.AddListener(delegate { ExitGame(); });  //종료
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
