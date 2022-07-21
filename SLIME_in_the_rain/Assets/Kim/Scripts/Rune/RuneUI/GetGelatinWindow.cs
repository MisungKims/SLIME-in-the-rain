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
    #endregion


    #region �Լ�
    // ���� ����ƾ�� �����͸� ������ UI ����
    public void SetUI()
    {
        item = ItemDatabase.Instance.AllitemDB[Random.Range(0, 15)];

        GelatinName = item.itemName;
        GelatinDesc = item.itemExplain;
        gelatinImage.sprite = item.itemIcon;
    }

    // ȹ�� ��ư Ŭ����
    public void GetButton()
    {
        if (Inventory.Instance.IsFull())        // �κ��丮�� ������ ���� ��
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
