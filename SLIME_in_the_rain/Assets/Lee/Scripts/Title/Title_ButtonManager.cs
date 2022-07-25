using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_ButtonManager : MonoBehaviour
{
    public GameObject setting;   //����â
    public Button[] buttons;    //����
                                //����
                                //����



    private void Start()
    {
        int i = 0;
        buttons[i++].onClick.AddListener(delegate { StartButton(); });  //����
        buttons[i++].onClick.AddListener(delegate { SettingButton(); });  //����
        buttons[i++].onClick.AddListener(delegate { ExitGame(); });  //����
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Town");
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
