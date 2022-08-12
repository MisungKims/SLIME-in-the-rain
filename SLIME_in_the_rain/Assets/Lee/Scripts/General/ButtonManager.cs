using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour                                 //������ �ν��Ͻ� ���� ui �ߺ��Ǵ°� ó���ϰ� �ϱ�
{
    #region ����
    [Header("---- ESC�� �� UI (SetActive) ----")]
    public List<GameObject> canvasList;
    [Header("---- ���� ������ ----")]
    public Button settingIcon;
    [Header("---�����(�ʱ�ȭ) / Ÿ��Ʋ��(��������)---")]
    public Button reButton;
    public Button quitButton;
    [Header("---- �˾� ----")]
    public GameObject popup;
    public TextMeshProUGUI popupText;
    public Button popupYes;
    public Button popupNo;


    Vector3 pos;

    //ĳ��
    SceneDesign sceneDesign;
    #endregion

    #region ����Ƽ�Լ�

    private void Start()
    {
        //singleton
        sceneDesign = SceneDesign.Instance;
        
        ///onClick
        //���� ��ư
        settingIcon.onClick.AddListener(delegate { SettingButton(); });
        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].gameObject.SetActive(false);
        }


        //Ÿ��Ʋ ȭ���� ��
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            TitleSettingButtons();
        }
        //�� �� �ΰ��� ����
        else
        {
            GameSettingButtons();
        }
        //�⺻ ����: �˾� ��
        popup.SetActive(false);
    }

    //ESC ������ �� ���� ��� ����
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1 , unenable  = 0; i > -1; i--)     //����(0), ���� ������(������) ������ �ȵ�  ,,, �κ��丮(1)�� ���� �ڽĸ� ������ �ؾ���
            {
                //â�� �ϳ��� �������� ESC �������� �� â�� ����
                if (canvasList[i].activeSelf)
                {
                    canvasList[i].SetActive(false);
                    
                    break;
                }
                //�⺻ ȭ�� �϶� ESC ������ ����â ��
                if (!canvasList[i].activeSelf)
                {
                    unenable++;
                }
                if (unenable == canvasList.Count)
                {
                    canvasList[canvasList.Count - 2].SetActive(true);
                }
            }
        }
    }
#endregion


#region �Լ�
    void SettingButton()
    {
        //PlayerPrefs.GetString();
        canvasList[canvasList.Count - 2].SetActive(true);
    }
    void TitleSettingButtons()
    {
        //Re��ư : �ʱ�ȭ
        reButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�ʱ�ȭ";
        //�ʱ� ����
        pos = reButton.transform.localPosition;
        pos.x = 0;
        reButton.transform.localPosition = pos;
        reButton.onClick.AddListener(delegate { OnPopup("00"); });

        //Quit��ư : ���� ��ó ���� �͵�
        quitButton.gameObject.SetActive(false);
    }
    void GameSettingButtons()
    {
        //Re��ư : �����
        reButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�����";
        //�ʱ� ����
        pos = reButton.transform.localPosition;
        pos.x = -205;
        reButton.transform.localPosition = pos;
        reButton.onClick.AddListener(delegate { OnPopup("10"); });

        //Quit��ư : Ÿ��Ʋ��
        quitButton.gameObject.SetActive(true);
        quitButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ÿ��Ʋ��";
        quitButton.onClick.AddListener(delegate { OnPopup("11"); });
    }
    void OnPopup(string str)
    {
        switch (str)
        {
            case "00":
                popupText.text = "�ʱ�ȭ �Ͻðڽ��ϱ�?";
                popupYes.onClick.AddListener(ResetButton);
                break;
            case "10":
                popupText.text = "����� �Ͻðڽ��ϱ�?";
                popupYes.onClick.AddListener(RestartButton);
                break;
            case "11":
                popupText.text = "Ÿ��Ʋ�� ���ðڽ��ϱ�?";
                popupYes.onClick.AddListener(GoTitleButton);
                break;
            default:
                break;
        }
        popupNo.onClick.AddListener(ClosePopup);
        popup.SetActive(true);
        
    }
    void ClosePopup()
    {
        popup.SetActive(false);
    }
    void ResetButton()
    {
        PlayerPrefs.DeleteKey("MaxHP" + "level");
        PlayerPrefs.DeleteKey("CoolTime" + "level");
        PlayerPrefs.DeleteKey("MoveSpeed" + "level");
        PlayerPrefs.DeleteKey("AttackSpeed" + "level");
        PlayerPrefs.DeleteKey("AttackPower" + "level");
        PlayerPrefs.DeleteKey("AttackRange" + "level");
        PlayerPrefs.DeleteKey("DefensePower" + "level");
        PlayerPrefs.DeleteKey("InventorySlot" + "level");
        //�⺻ ����: �˾� ��
        popup.SetActive(false);
    }
    void RestartButton()
    {
        //�⺻ ����: �˾� ��
        popup.SetActive(false);
        SceneManager.LoadScene(1);
    }
    void GoTitleButton()
    {
        //�⺻ ����: �˾� ��
        popup.SetActive(false);
        SceneManager.LoadScene(0);
    }

#endregion

}
