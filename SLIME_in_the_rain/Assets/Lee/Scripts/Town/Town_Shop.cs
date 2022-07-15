using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Town_Shop : MonoBehaviour
{
    public GameObject itemDatabase;

    /// <summary>
    ///    //상점 요소
    ///TextMeshProUGUI gelatinNameText; //젤라틴 이름; 아이템 베이스에서 가져옴
    ///TextMeshProUGUI gelatinInfoText; //젤라틴 설명; 아이템 베이스에서 가져옴
    ///TextMeshProUGUI jellyPriceText;  //가격; 랜덤값
    ///TextMeshProUGUI remainText;      //남아있는 개수; 랜덤값
    ///Image gelatinImage;              //젤라틴 이미지
    /// </summary>
    public Transform shopCanvas;



    // Start is called before the first frame update
    void Start()
    {

        int categoryNum = 3;

        //Debug.Log(itemDatabase.GetComponent<ItemDatabase>().AllitemDB[0].itemInfo);

        Transform[] categorys = new Transform[categoryNum];
        ItemDatabase gelatin = itemDatabase.GetComponent<ItemDatabase>();

        for (int i = 0; i < categoryNum; i++)
        {
            categorys[i] = shopCanvas.GetChild(3+i).GetChild(3);    //Shop->Category->CategoryDB
        }

        for (int i = 0, ranValue,index = 0; i < categoryNum; i++,index = 0)
        {
            //젤라틴만 뽑기
            while (true)
            {   
                ranValue = Random.Range(0, itemDatabase.GetComponent<ItemDatabase>().AllitemDB.Count - 1);
                if (itemDatabase.GetComponent<ItemDatabase>().AllitemDB[ranValue].itemType == ItemType.gelatin) break;
            }
            categorys[i].GetChild(index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].itemExplain;    //TextMeshProUGUI gelatinNameText
            ++index;
            //categorys[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].;           //젤라틴 옵션 받아오기
            categorys[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(10, 30).ToString();          //판매 금액 (랜덤: 10 ~ 30)
            categorys[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(1, 5).ToString();            //남은 수 (랜덤: 1 ~ 5)
            categorys[i].GetChild(++index).GetComponent<Image>().sprite = gelatin.AllitemDB[ranValue].itemIcon;             //Image gelatinImage
        }
    }

}