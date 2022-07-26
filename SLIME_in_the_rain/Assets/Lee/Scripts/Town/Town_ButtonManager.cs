using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class Town_ButtonManager : MonoBehaviour                                 //다음엔 인스턴스 만들어서 ui 중복되는거 처리하게 하기
{
    #region 변수
    [Header("---- ESC에 꺼질 UI 순서대로 넣으세요 ----")]
    public List<GameObject> canvasList;
    [Header("---- 설정 ----")]
    public Button settingIcon;
    [Header("---- 상점 ----")]
    public List<Button> shopButtonList;
    #endregion


    #region 유니티함수
    private void Start()
    {
        ///onClick 관련 스크립트
        //세팅 버튼에 세팅 버튼 onClick 추가
        settingIcon.onClick.AddListener(delegate { SettingButton(); });

        for (int  i = 0;  i < shopButtonList.Count;  i++)
        {
            shopButtonList[i].GetComponent<Button>().onClick.AddListener(delegate { Remain(); });
        }
    }



    //ESC 누르면 그 순서 대로 끄기
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1 , unenable  = 0; i > -1; i--)     //메인(0), 세팅 아이콘(마지막) 꺼지면 안됨  ,,, 인벤토리(1)은 하위 자식만 꺼지게 해야함
            {
                //창이 하나라도 떠있으면 ESC 눌렀을때 그 창을 닫음
                if (canvasList[i].gameObject.activeSelf)
                {
                    canvasList[i].gameObject.SetActive(false);
                    break;
                }
                //기본 화면 일때 ESC 누르면 설정창 뜸
                if (!canvasList[i].gameObject.activeSelf)
                {
                    unenable++;
                }
                if (unenable == canvasList.Count)
                {
                    canvasList[canvasList.Count - 1].gameObject.SetActive(true);
                }
            }
        }
    }
    #endregion


    #region 함수
    void SettingButton()
    {
        canvasList[canvasList.Count - 1].gameObject.SetActive(true);
    }
    void Remain()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;     //버튼 누른 오브젝트 가져옴
        TextMeshProUGUI remainText = clickObject.transform.GetChild(clickObject.transform.childCount - 2).GetChild(3).GetComponent<TextMeshProUGUI>();        //누른 버튼 -> 뒤에서 2번째인 update -> 4번째 자식인 remain
        remainText.text = (int.Parse(remainText.text) - 1).ToString();
        
        //남은거 0개 됐을때 끄기
        if(int.Parse(remainText.text) == 0)
        {
            clickObject.GetComponent<Town_Shop_Button>().enabled = false;     //버튼 작동 스크립트 끔
            clickObject.transform.GetChild(clickObject.transform.childCount - 1).gameObject.SetActive(true);        //누른버튼 -> 뒤에서 첫번째인 panel on
            clickObject.transform.GetChild(clickObject.transform.childCount - 1).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f); 
            clickObject.GetComponent<Button>().interactable = false;
        }
    }


    //임시
    public void GoInDun()
    {
        SceneManager.LoadScene("Village");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

}
