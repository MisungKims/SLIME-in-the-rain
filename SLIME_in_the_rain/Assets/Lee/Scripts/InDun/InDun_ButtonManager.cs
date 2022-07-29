using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class InDun_ButtonManager : MonoBehaviour                                 //다음엔 인스턴스 만들어서 ui 중복되는거 처리하게 하기
{
    #region 변수
    [Header("---- ESC에 꺼질 UI 순서대로 넣으세요 ----")]
    public List<GameObject> canvasList;
    [Header("---- 설정 ----")]
    public Button settingIcon;
    #endregion


    #region 유니티함수
    private void Start()
    {
        ///onClick 관련 스크립트
        //세팅 버튼에 세팅 버튼 onClick 추가
        settingIcon.onClick.AddListener(delegate { SettingButton(); });
    }



    //ESC 누르면 그 순서 대로 끄기
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1 , unenable  = 0; i > -1; i--)     //순서대로 끔
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


    //임시
    //public void GoInDun()
    //{
    //    SceneManager.LoadScene("Village");
    //}
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

}
