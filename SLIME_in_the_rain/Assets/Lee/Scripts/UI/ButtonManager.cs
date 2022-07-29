using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ButtonManager : MonoBehaviour                                 //다음엔 인스턴스 만들어서 ui 중복되는거 처리하게 하기
{
    #region 변수
    [Header("---- ESC로 끌 UI (SetActive) ----")]
    public List<GameObject> canvasList;
    [Header("---- 설정 ----")]
    public Button settingIcon;
    #endregion


    #region 유니티함수

    private void Start()
    {
        ///onClick
        //세팅 버튼
        settingIcon.onClick.AddListener(delegate { SettingButton(); });
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
                    canvasList[canvasList.Count - 1].SetActive(true);
                }
            }
        }
    }
    #endregion


    #region 함수
    void SettingButton()
    {
        canvasList[canvasList.Count - 1].SetActive(true);
    }

    #endregion

}
