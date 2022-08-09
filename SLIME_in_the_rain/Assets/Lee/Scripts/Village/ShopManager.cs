using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopManager : MonoBehaviour        //������DB ���°Ŷ� �׺��� �ʰ� ����Ǿ����
{
    #region ����
    //private
    Item item;       //���� ��ư�� ����ƾ �Ӽ��� ������
    //bool isFind;        //�κ��丮�� �ߺ� Ȯ��
    List<Transform> shopButtonList = new List<Transform>();    //���� ��ư��
    int shopBtnCount = 3;           //��ư ���� -> 3��

    //singleton
    ItemDatabase itemDB;
    Inventory inventory;
    JellyManager jellyManager;
    #endregion

    #region �ڷ�ƾ
    IEnumerator ActiveOnOffWFS(GameObject gameObject)
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    #endregion
    #region ����Ƽ�Լ�
    private void Start()
    {
        //���� �ʱⰪ ����
        for (int i = 0; i < shopBtnCount; i++)
        {
            shopButtonList.Add(this.transform.GetChild(i + 1));
        }

        //singleton
        itemDB = ItemDatabase.Instance;
        inventory = Inventory.Instance;
        jellyManager = JellyManager.Instance;

        ///onClick
        //Shop Btn
        for (int i = 0; i < shopButtonList.Count; i++)
        {
            shopButtonList[i].GetComponent<Button>().onClick.AddListener(delegate { ClickEvent(); });
        }

        //���� ��ư�� �Ӽ��� ������DB���� �������
        int categoryNum = 3;
        Transform[] updates = new Transform[categoryNum];

        for (int i = 0; i < categoryNum; i++)
        {
            //Shop -> ShopBtn -> �ڿ��� 2��° : Updates
            updates[i] = this.transform.GetChild(1 + i).GetChild(this.transform.GetChild(1 + i).childCount - 2);
            //Debug.Log(updates[i]);

        }

        for (int i = 0, index = 0, ranValue = 0; i < categoryNum; i++, index = 0)
        {
            while (true)     //����ƾ�� ����   --->   �������� �� DB�� ����
            {
                ranValue = Random.Range(0, itemDB.AllitemDB.Count - 1);
                //Debug.Log(gelatin.AllitemDB.Count);                   //�ؽ�ũ��Ʈ inventoryUI���� �ʰ� ���� �ž���
                if (itemDB.AllitemDB[ranValue].itemType == itemDB.AllitemDB[0].itemType) break;     //0�� ������ Ȯ��; ����ƾ�϶�
            }
            //���� ��ư ����
            updates[i].GetChild(index).GetComponent<TextMeshProUGUI>().text = itemDB.AllitemDB[ranValue].itemExplain;    //����ƾ �̸�
            ++index;
            //updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = itemDB.AllitemDB[ranValue].;           //����ƾ ����
            updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(10, 30).ToString();          //���� (����: 10 ~ 30)
            updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(1, 5).ToString();            //���� �� (����: 1 ~ 5)
            updates[i].GetChild(++index).GetComponent<Image>().sprite = itemDB.AllitemDB[ranValue].itemIcon;             //Image gelatinImage
        }
    }
    #endregion
    #region �Լ�


    //��ư ������ ���� ���� ���̴� �Լ�
    void ClickEvent()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;     //���� ��ư�� ���� ������
        //Ŭ����ư -> �ڿ��� 2��° update -> 3��° ����
        if (jellyManager.JellyCount > int.Parse(clickObject.transform.GetChild((clickObject.transform.childCount) - 2).GetChild(2).GetComponent<TextMeshProUGUI>().text))
        {
            Remain(clickObject);
            SetInven(clickObject);
        }
        else
        {
            //��ĵ���� -> �ڿ��� 2��° �ǳ�
            GameObject cantBuy = this.transform.GetChild(clickObject.transform.childCount - 2).gameObject;
            cantBuy.SetActive(true);
            StartCoroutine(ActiveOnOffWFS(cantBuy));
        }
    }
    #region  ���� ���� ���� �Լ�
    void Remain(GameObject _clickObject)
    {
        //���� ��ư -> �ڿ��� 2��°�� update -> 4��° �ڽ��� remain
        TextMeshProUGUI remainText = _clickObject.transform.GetChild(_clickObject.transform.childCount - 2).GetChild(3).GetComponent<TextMeshProUGUI>();        
        remainText.text = (int.Parse(remainText.text) - 1).ToString();

        //������ 0�� ������ ����
        if (int.Parse(remainText.text) == 0)
        {
            _clickObject.GetComponent<ButtonCustom>().enabled = false;     //��ư �۵� ��ũ��Ʈ ��
            _clickObject.transform.GetChild(_clickObject.transform.childCount - 1).gameObject.SetActive(true);        //������ư -> �ڿ��� ù��°�� panel on //�ǳ��� �̹����� ���� �־ setActive�� ��
            _clickObject.transform.GetChild(_clickObject.transform.childCount - 1).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
            _clickObject.GetComponent<Button>().interactable = false;
        }
    }
    #endregion

    #region ��ư Ŭ���� �ش� ����ƾ �κ��丮�� ���� �Լ���
    public void SetInven(GameObject _clickObject)
    {
        //���� ��ư -> �ڿ��� 2��°�� update -> ù��° �ڽ��� Name 
        item = (FindItem(_clickObject.transform.GetChild(_clickObject.transform.childCount - 2).GetChild(0).GetComponent<TextMeshProUGUI>().text));
        AddInventory(item);
    }

    //��ư�� ������ �̸����� ������ ã��
    public Item FindItem(string str)
    {
        Item item;
        for (int i = 0; i < itemDB.AllitemDB.Count; i++)
        {
            if (itemDB.AllitemDB[i].itemExplain == str)
            {
                item = itemDB.AllitemDB[i];
                return item;
            }
        }
        return null;
    }
    public void AddInventory(Item item)
    {
        inventory.addItem(item, 1);
    }

    #endregion
    

    #endregion
}
