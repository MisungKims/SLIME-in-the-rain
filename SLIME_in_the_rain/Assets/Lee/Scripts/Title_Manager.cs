using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class Title_Manager : MonoBehaviour
{
    public Canvas canvas_title;
    public Canvas canvas_setting;

    public void ChlickStartButton()
    {
        EditorSceneManager.LoadScene("Town");
    }

    public void ChickSettingButton()
    {
        canvas_title.gameObject.SetActive(false);
        canvas_setting.gameObject.SetActive(true);
    }

    public void ExitSetting()
    {
        canvas_title.gameObject.SetActive(true);
        canvas_setting.gameObject.SetActive(false);
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
