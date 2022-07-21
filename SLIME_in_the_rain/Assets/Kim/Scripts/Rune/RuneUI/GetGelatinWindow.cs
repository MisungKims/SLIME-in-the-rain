using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetGelatinWindow : MonoBehaviour
{
    #region 변수
    [SerializeField]
    private TextMeshProUGUI gelatinNameTxt;
    private string gelatinName;
    public string GelatinName
    {
        set
        {
            gelatinName = value;
            gelatinNameTxt.text = gelatinName;
        }
    }
    
    [SerializeField]
    private TextMeshProUGUI gelatinDescTxt;
    private string gelatinDesc;
    public string GelatinDesc
    {
        set
        {
            gelatinDesc = value;
            gelatinDescTxt.text = gelatinDesc;
        }
    }

    [SerializeField]
    private Image gelatinImage;

    private Item item;

    // 캐싱
    private ItemDatabase itemDatabase;
    private Inventory inventory;
    private ObjectPoolingManager objectPoolingManager;
    #endregion

    private void Start()
    {
        itemDatabase = ItemDatabase.Instance;
        inventory = Inventory.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    #region 함수
    // 랜덤 젤라틴의 데이터를 가져와 UI 설정
    public void SetUI()
    {
        item = itemDatabase.AllitemDB[Random.Range(0, 15)];

        GelatinName = item.itemName;
        GelatinDesc = item.itemExplain;
        gelatinImage.sprite = item.itemIcon;
    }

    // 획득 버튼 클릭시 인벤토리에 젤라틴 추가
    public void GetButton()
    {
        if (inventory.IsFull())        // 인벤토리에 공간이 없을 때
        {

        }
        else
        {
            FieldItems gelatin = objectPoolingManager.Get(EObjectFlag.gelatin).GetComponent<FieldItems>();

            gelatin.canDetect = false;
            gelatin.SetItem(item);
            gelatin.AddInventory();
        }
    }
    #endregion
}
