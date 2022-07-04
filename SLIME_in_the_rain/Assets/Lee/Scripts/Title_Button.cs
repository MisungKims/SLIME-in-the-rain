using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title_Button : MonoBehaviour
{
    //버튼을 누르고 있을때 버튼의 자식의 텍스트 색 변경(노란색으로)
    //버튼에 커서가 놀라가면 버튼의 자식의 텍스트 색 변경(노란색테두리가 생기는걸로)

    private Vector3 point = new Vector3();
    private Vector3 buttonPointLB = new Vector3();
    private Vector3 buttonPointRT = new Vector3();

    private void Start()
    {
        Rect rect = this.GetComponent<RectTransform>().rect;
        Vector3 pos = this.transform.position;

        //L
        buttonPointLB.x = pos.x - rect.width / 2;
        //R
        buttonPointRT.x = pos.x + rect.width / 2;
        //B
        buttonPointLB.y = pos.y - rect.height / 2;
        //T
        buttonPointRT.y = pos.y + rect.height / 2;

    }

    private void Update()
    {
        point = new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0);

        if(point.x > buttonPointLB.x && point.y > buttonPointLB.y
            && point.x <buttonPointRT.x && point.y <buttonPointRT.y)
        {
            //Debug.Log("onButton");
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255,232,124,255);
        }
        else
        {
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        }
    }
    
}
