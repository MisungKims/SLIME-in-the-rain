using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DissolutionUI : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static DissolutionUI instance = null;
    public static DissolutionUI Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Image weaponImage;
    public Image gelatin1;
    public Image gelatin2;
    public TextMeshProUGUI weaponTitleC;
    private GameObject Bag;
    private Slot SelectSlot;

    public void DissolutionWeapon(int _slotNum)
    {
        Bag = GameObject.Find("Bag");

        SelectSlot = Bag.transform.GetChild(_slotNum).GetComponent<Slot>();

        weaponImage.sprite = SelectSlot.transform.GetChild(0).GetComponent<Image>().sprite;

        weaponTitleC.text = SelectSlot.item.itemExplain;
    }





}
