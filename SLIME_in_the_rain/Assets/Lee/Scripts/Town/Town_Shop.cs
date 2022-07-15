using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Town_Shop : MonoBehaviour
{
    public GameObject itemDatabase;

    /// <summary>
    ///    //���� ���
    ///TextMeshProUGUI gelatinNameText; //����ƾ �̸�; ������ ���̽����� ������
    ///TextMeshProUGUI gelatinInfoText; //����ƾ ����; ������ ���̽����� ������
    ///TextMeshProUGUI jellyPriceText;  //����; ������
    ///TextMeshProUGUI remainText;      //�����ִ� ����; ������
    ///Image gelatinImage;              //����ƾ �̹���
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
            //����ƾ�� �̱�
            while (true)
            {   
                ranValue = Random.Range(0, itemDatabase.GetComponent<ItemDatabase>().AllitemDB.Count - 1);
                if (itemDatabase.GetComponent<ItemDatabase>().AllitemDB[ranValue].itemType == ItemType.gelatin) break;
            }
            categorys[i].GetChild(index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].itemExplain;    //TextMeshProUGUI gelatinNameText
            ++index;
            //categorys[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].;           //����ƾ �ɼ� �޾ƿ���
            categorys[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(10, 30).ToString();          //�Ǹ� �ݾ� (����: 10 ~ 30)
            categorys[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(1, 5).ToString();            //���� �� (����: 1 ~ 5)
            categorys[i].GetChild(++index).GetComponent<Image>().sprite = gelatin.AllitemDB[ranValue].itemIcon;             //Image gelatinImage
        }
    }

}