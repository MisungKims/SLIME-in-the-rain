using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour        //아이템DB 쓰는거라 그보다 늦게 실행되어야함
{
    #region 변수



    //private
    Item item;       //상점 버튼의 젤라틴 속성들 저장함
    bool isFind;        //인벤토리내 중복 확인
    List<Transform> shopButtonList = new List<Transform>();    //상점 버튼들
    int shopBtnCount = 3;           //버튼 개수 -> 3개

    //singleton
    ItemDatabase itemDB;
    Inventory inventory;
    InventoryUI inventoryUI;
    JellyManager jellyManager;
    #endregion

    #region 유니티함수
    private void Start()
    {
        //변수 초기값 대입
        for (int i = 0; i < shopBtnCount; i++)
        {
            shopButtonList.Add(this.transform.GetChild(i + 1));
        }

        //singleton
        itemDB = ItemDatabase.Instance;
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;

        ///onClick
        //Shop Btn
        for (int i = 0; i < shopButtonList.Count; i++)
        {
            shopButtonList[i].GetComponent<Button>().onClick.AddListener(delegate { ClickEvent(); });
        }

        //상점 버튼내 속성을 아이템DB에서 끌고오기
        int categoryNum = 3;
        Transform[] updates = new Transform[categoryNum];

        for (int i = 0; i < categoryNum; i++)
        {
            //Shop -> ShopBtn -> 뒤에서 2번째
            updates[i] = this.transform.GetChild(1 + i).GetChild(this.transform.GetChild(1 + i).childCount - 2);
            //Debug.Log(updates[i]);

        }

        for (int i = 0, index = 0, ranValue = 0; i < categoryNum; i++, index = 0)
        {
            while (true)     //젤라틴만 받음   --->   유동적인 템 DB를 위해
            {
                ranValue = Random.Range(0, itemDB.AllitemDB.Count - 1);
                //Debug.Log(gelatin.AllitemDB.Count);                   //※스크립트 inventoryUI보다 늦게 실행 돼야함
                if (itemDB.AllitemDB[ranValue].itemType == itemDB.AllitemDB[0].itemType) break;     //0번 아이템 확인; 젤라틴일때
            }
            //상점 버튼 관리
            updates[i].GetChild(index).GetComponent<TextMeshProUGUI>().text = itemDB.AllitemDB[ranValue].itemExplain;    //젤라틴 이름
            ++index;
            //updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = gelatin.AllitemDB[ranValue].;           //젤라틴 정보
            updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(10, 30).ToString();          //가격 (랜덤: 10 ~ 30)
            updates[i].GetChild(++index).GetComponent<TextMeshProUGUI>().text = Random.Range(1, 5).ToString();            //남은 수 (랜덤: 1 ~ 5)
            updates[i].GetChild(++index).GetComponent<Image>().sprite = itemDB.AllitemDB[ranValue].itemIcon;             //Image gelatinImage
        }
    }
    #endregion
    #region 함수


    //버튼 누를시 남은 수량 줄이는 함수
    void ClickEvent()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;     //누른 버튼가 뭔지 가져옴
        //Debug.Log(clickObject);
        //    void compareJelly(GameObject _clickObject)
            Remain(clickObject);
            SetInven(clickObject);
    }
    #region  남은 수량 관련 함수
    void Remain(GameObject _clickObject)
    {
        //누른 버튼 -> 뒤에서 2번째인 update -> 4번째 자식인 remain
        TextMeshProUGUI remainText = _clickObject.transform.GetChild(_clickObject.transform.childCount - 2).GetChild(3).GetComponent<TextMeshProUGUI>();        
        remainText.text = (int.Parse(remainText.text) - 1).ToString();

        //남은거 0개 됐을때 끄기
        if (int.Parse(remainText.text) == 0)
        {
            _clickObject.GetComponent<Town_Shop_Button>().enabled = false;     //버튼 작동 스크립트 끔
            _clickObject.transform.GetChild(_clickObject.transform.childCount - 1).gameObject.SetActive(true);        //누른버튼 -> 뒤에서 첫번째인 panel on //판넬은 이미지가 위에 있어서 setActive로 함
            _clickObject.transform.GetChild(_clickObject.transform.childCount - 1).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
            _clickObject.GetComponent<Button>().interactable = false;
        }
    }
    #endregion

    #region 버튼 클릭시 해당 젤라틴 인벤토리에 들어가는 함수들
    public void SetInven(GameObject _clickObject)
    {
        //누른 버튼 -> 뒤에서 2번째인 update -> 첫번째 자식인 Name 
        item = (FindItem(_clickObject.transform.GetChild(_clickObject.transform.childCount - 2).GetChild(0).GetComponent<TextMeshProUGUI>().text));
        //아이템을 인벤토리에 추가
        AddInventory(item);
    }

    //버튼내 아이템 이름으로 아이템 찾기
    public Item FindItem(string str)
    {
       // Item item = new Item(ItemType.gelatin, string.Empty, string.Empty);
        for (int i = 0; i < itemDB.AllitemDB.Count; i++)
        {
            if (itemDB.AllitemDB[i].itemExplain == str)
            {
                item = itemDB.AllitemDB[i];
                break;
            }
        }
        return item;
    }

    /// <summary>
    /// Jeon's sc FieldItems  <- not singletone
    /// </summary>
    public void AddInventory(Item item)
    {
        //if (!FindSame())        // 인벤토리에 이 아이템이 없으면
        //{
        //    AddItem();          // 새로 추가
        //}
        // 인벤토리에 공간이 있으면 젤라틴을 인벤토리에 추가
        FieldItems gelatin =    ObjectPoolingManager.Instance.Get(EObjectFlag.gelatin).GetComponent<FieldItems>();

        gelatin.canDetect = false;
        gelatin.SetItem(item);
        gelatin.AddInventory();
    }
    //void AddItem()
    //{
    //    inventory.items.Add(item);
    //    inventoryUI.slots[inventoryUI.index].itemCount = 1;

    //    if (inventory.onChangedItem != null)
    //    {
    //        inventory.onChangedItem.Invoke();
    //    }
    //    inventoryUI.index++;
    //}

    // 인벤토리에서 같은 아이템을 찾아 카운트를 증가시킴
    private bool FindSame()
    {
        if (!inventory) inventory = Inventory.Instance;
        if (!inventoryUI) inventoryUI = InventoryUI.Instance;

        isFind = false;

        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemName == item.itemName)
            {
                inventoryUI.slots[i].SetSlotCount();
                isFind = true;
                break;
            }
        }
        return isFind;
    }
    #endregion

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
