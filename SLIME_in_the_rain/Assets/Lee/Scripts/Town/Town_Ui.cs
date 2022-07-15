using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Town_Ui : MonoBehaviour
{
    #region 변수
    //레이어 순서대로 넣기
    public List<GameObject> canvasList = new List<GameObject>();

    #endregion

    #region 유니티함수

    //ESC 누르면 맨 위의 레이어인 Ui를 닫음
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1; i > - 1; i--)
            {
                if (canvasList[i].gameObject.activeSelf)
                {
                    canvasList[i].gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
    #endregion

}
