using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class Title_Manager : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject settingCanvas;

    public void ChlickStartButton()
    {
        EditorSceneManager.LoadScene("Town");
    }

    public void ChickSettingButton()
    {
        mainCanvas.gameObject.SetActive(false);
        settingCanvas.gameObject.SetActive(true);
    }

    public void ExitSetting()
    {
        mainCanvas.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(false);
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
