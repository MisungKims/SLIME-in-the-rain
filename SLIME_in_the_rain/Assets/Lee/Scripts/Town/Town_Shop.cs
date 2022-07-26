using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Town_Shop : MonoBehaviour      //아이템DB와 연관되는 상점 관련 스크립트
{
    #region 변수
    [SerializeField]
    private Transform shopCanvas;          //상점 캔버스
    [SerializeField]
    private GameObject itemDB;              //아이템DB

    #endregion
    // Start is called before the first frame update
    void Start()                            //아이템베이스.cs 보다 늦게 실행 되어야함
    {
        //상점 버튼내 속성을 아이템DB에서 끌고오기
        int categoryNum = 3;
        Transform[] updates = new Transform[categoryNum];
        ItemDatabase gelatin = itemDB.GetComponent<ItemDatabase>();

        for (int i = 0; i < categoryNum; i++)
        {
            //Shop -> ShopBtn -> 뒤에서 2번째
            updates[i] = shopCanvas.GetChild(1+i).GetChild(shopCanvas.GetChild(1+i).childCount - 2);    
            //Debug.Log(updates[i]);
    
        }

        for (int i = 0, index = 0, ranValue = 0; i < categoryNum; i++, index = 0)
        {
            while (true)     //젤라틴만 받음   --->   유동적인 템 DB를 위해
            {
                ranValue = Random.Range(0, gelatin.AllitemDB.Count - 1);
                //Debug.Log(gelatin.AllitemDB.Count);                   //※스크립트 inventoryUI보다 늦게 실행 돼야함
                if (gelatin.AllitemDB[ranValue].itemType == gelatin.AllitemDB[0].itemType) break;     //0번 아이템 확인; 젤라틴일때
            }
            //상점 버튼 관리
            updates[i].GetChild(index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].itemExplain;    //젤라틴 이름
            ++index;
            //updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].;           //젤라틴 정보
            updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(10, 30).ToString();          //가격 (랜덤: 10 ~ 30)
            updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(1, 5).ToString();            //남은 수 (랜덤: 1 ~ 5)
            updates[i].GetChild(++index).GetComponent<Image>().sprite = gelatin.AllitemDB[ranValue].itemIcon;             //Image gelatinImage
        }
    }
}