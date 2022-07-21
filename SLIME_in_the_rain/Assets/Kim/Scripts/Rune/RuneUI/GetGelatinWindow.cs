using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetGelatinWindow : MonoBehaviour
{
    #region ����
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

    // ĳ��
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

    #region �Լ�
    // ���� ����ƾ�� �����͸� ������ UI ����
    public void SetUI()
    {
        item = itemDatabase.AllitemDB[Random.Range(0, 15)];

        GelatinName = item.itemName;
        GelatinDesc = item.itemExplain;
        gelatinImage.sprite = item.itemIcon;
    }

    // ȹ�� ��ư Ŭ���� �κ��丮�� ����ƾ �߰�
    public void GetButton()
    {
        if (inventory.IsFull())        // �κ��丮�� ������ ���� ��
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
