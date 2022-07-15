using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title_OnButton : MonoBehaviour
{
    //��ư�� ������ ������ ��ư�� �ڽ��� �ؽ�Ʈ �� ����(���������)
    //��ư�� Ŀ���� ��󰡸� ��ư�� �ڽ��� �ؽ�Ʈ �� ����(������׵θ��� ����°ɷ�)

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
