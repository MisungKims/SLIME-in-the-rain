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
    #endregion


    #region 함수
    // 랜덤 젤라틴의 데이터를 가져와 UI 설정
    public void SetUI()
    {
        item = ItemDatabase.Instance.AllitemDB[Random.Range(0, 15)];

        GelatinName = item.itemName;
        GelatinDesc = item.itemExplain;
        gelatinImage.sprite = item.itemIcon;
    }

    // 획득 버튼 클릭시
    public void GetButton()
    {
        if (Inventory.Instance.IsFull())        // 인벤토리에 공간이 없을 때
        {

        }
        else
        {
            FieldItems gelatin = ObjectPoolingManager.Instance.Get(EObjectFlag.gelatin).GetComponent<FieldItems>();
            gelatin.canDetect = false;
            gelatin.SetItem(item);

            
        }
    }
    #endregion
}
