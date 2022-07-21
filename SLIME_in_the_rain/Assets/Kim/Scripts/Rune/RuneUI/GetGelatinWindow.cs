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
    #endregion


    #region �Լ�
    public void SetUI()
    {
        //FieldItems gelatin = ObjectPoolingManager.Instance.Get(EObjectFlag.gelatin).GetComponent<FieldItems>();
        //gelatin.gameObject.SetActive(false);

        Item item = ItemDatabase.Instance.RandomGelatin();

        GelatinName = item.itemName;
        GelatinDesc = item.itemExplain;
        gelatinImage.sprite = item.itemIcon;
    }

    // ȹ�� ��ư Ŭ����
    public void GetButton()
    {

    }
    #endregion
}
