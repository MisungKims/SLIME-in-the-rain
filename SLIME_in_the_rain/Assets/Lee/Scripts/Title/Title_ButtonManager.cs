using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_ButtonManager : MonoBehaviour
{
    public GameObject setting;   //설정창
    public Button[] buttons;    //시작
                                //설정
                                //종료

    //캐싱
    SettingCanvas settingCanvas;
    SceneDesign sceneDesign;


    private void Start()
    {
        //singleton
        settingCanvas = SettingCanvas.Instance;
        settingCanvas.settingIcon.SetActive(false);
        sceneDesign = SceneDesign.Instance;

        //Tutorial
        if (PlayerPrefs.HasKey("tutorial"))
        {
            sceneDesign.doTutorial = System.Convert.ToBoolean(PlayerPrefs.GetInt("tutorial"));
        }


        //OnClick
        int i = 0;
        buttons[i++].onClick.AddListener(delegate { StartButton(); });  //시작
        buttons[i++].onClick.AddListener(delegate { SettingButton(); });  //설정
        buttons[i++].onClick.AddListener(delegate { ExitGame(); });  //종료
    }

    public void StartButton()
    {
        if(sceneDesign.doTutorial)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
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
