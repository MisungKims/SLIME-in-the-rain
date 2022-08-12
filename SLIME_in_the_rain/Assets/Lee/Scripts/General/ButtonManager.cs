using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour                                 //다음엔 인스턴스 만들어서 ui 중복되는거 처리하게 하기
{
    #region 변수
    [Header("---- ESC로 끌 UI (SetActive) ----")]
    public List<GameObject> canvasList;
    [Header("---- 세팅 아이콘 ----")]
    public Button settingIcon;
    [Header("---재시작(초기화) / 타이틀로(게임종료)---")]
    public Button reButton;
    public Button quitButton;
    [Header("---- 팝업 ----")]
    public GameObject popup;
    public TextMeshProUGUI popupText;
    public Button popupYes;
    public Button popupNo;


    Vector3 pos;

    //캐싱
    SceneDesign sceneDesign;
    #endregion

    #region 유니티함수

    private void Start()
    {
        //singleton
        sceneDesign = SceneDesign.Instance;
        
        ///onClick
        //세팅 버튼
        settingIcon.onClick.AddListener(delegate { SettingButton(); });
        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].gameObject.SetActive(false);
        }


        //타이틀 화면일 때
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            TitleSettingButtons();
        }
        //그 외 인게임 전부
        else
        {
            GameSettingButtons();
        }
        //기본 세팅: 팝업 끔
        popup.SetActive(false);
    }

    //ESC 누르면 그 순서 대로 끄기
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1 , unenable  = 0; i > -1; i--)     //메인(0), 세팅 아이콘(마지막) 꺼지면 안됨  ,,, 인벤토리(1)은 하위 자식만 꺼지게 해야함
            {
                //창이 하나라도 떠있으면 ESC 눌렀을때 그 창을 닫음
                if (canvasList[i].activeSelf)
                {
                    canvasList[i].SetActive(false);
                    
                    break;
                }
                //기본 화면 일때 ESC 누르면 설정창 뜸
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


#region 함수
    void SettingButton()
    {
        //PlayerPrefs.GetString();
        canvasList[canvasList.Count - 2].SetActive(true);
    }
    void TitleSettingButtons()
    {
        //Re버튼 : 초기화
        reButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "초기화";
        //초기 설정
        pos = reButton.transform.localPosition;
        pos.x = 0;
        reButton.transform.localPosition = pos;
        reButton.onClick.AddListener(delegate { OnPopup("00"); });

        //Quit버튼 : 에셋 출처 적을 것들
        quitButton.gameObject.SetActive(false);
    }
    void GameSettingButtons()
    {
        //Re버튼 : 재시작
        reButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "재시작";
        //초기 설정
        pos = reButton.transform.localPosition;
        pos.x = -205;
        reButton.transform.localPosition = pos;
        reButton.onClick.AddListener(delegate { OnPopup("10"); });

        //Quit버튼 : 타이틀로
        quitButton.gameObject.SetActive(true);
        quitButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "타이틀로";
        quitButton.onClick.AddListener(delegate { OnPopup("11"); });
    }
    void OnPopup(string str)
    {
        switch (str)
        {
            case "00":
                popupText.text = "초기화 하시겠습니까?";
                popupYes.onClick.AddListener(ResetButton);
                break;
            case "10":
                popupText.text = "재시작 하시겠습니까?";
                popupYes.onClick.AddListener(RestartButton);
                break;
            case "11":
                popupText.text = "타이틀로 가시겠습니까?";
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
        //기본 세팅: 팝업 끔
        popup.SetActive(false);
    }
    void RestartButton()
    {
        //기본 세팅: 팝업 끔
        popup.SetActive(false);
        SceneManager.LoadScene(1);
    }
    void GoTitleButton()
    {
        //기본 세팅: 팝업 끔
        popup.SetActive(false);
        SceneManager.LoadScene(0);
    }

#endregion

}
