using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title_OnButton : MonoBehaviour
{
    //버튼을 누르고 있을때 버튼의 자식의 텍스트 색 변경(노란색으로)
    //버튼에 커서가 놀라가면 버튼의 자식의 텍스트 색 변경(노란색테두리가 생기는걸로)

    public Button[] buttonArr;

    private Vector3 point;
    private Vector3[] buttonPointLBArr;
    private Vector3[] buttonPointRTArr;



    private void Start()
    {
        buttonPointLBArr = new Vector3[buttonArr.Length];
        buttonPointRTArr = new Vector3[buttonArr.Length];



        for (int i = 0; i < buttonArr.Length; i++)
        {
            Rect rect = buttonArr[i].GetComponent<RectTransform>().rect;
            Vector3 pos = buttonArr[i].transform.position;

            //L
            buttonPointLBArr[i].x = pos.x - rect.width / 2;
            //R
            buttonPointRTArr[i].x = pos.x + rect.width / 2;
            //B
            buttonPointLBArr[i].y = pos.y - rect.height / 2;
            //T
            buttonPointRTArr[i].y = pos.y + rect.height / 2;

        }



    }

    private void Update()
    {
        point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        for (int i = 0; i < buttonArr.Length; i++)
        {
            if (point.x > buttonPointLBArr[i].x && point.y > buttonPointLBArr[i].y
                && point.x < buttonPointRTArr[i].x && point.y < buttonPointRTArr[i].y)
            {
                //Debug.Log("onButton");
                buttonArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 232, 124, 255);
            }
            else
            {
                buttonArr[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
            }
        }

    }

}
