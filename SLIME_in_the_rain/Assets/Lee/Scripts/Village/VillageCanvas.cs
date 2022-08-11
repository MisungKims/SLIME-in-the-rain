using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


/// <summary>
/// panel �ߴ°� ����
/// </summary>
public class VillageCanvas : MonoBehaviour
{
    //public 
    public GameObject ShopCanvas;
    public GameObject TowerCanvas;

    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (TowerCollider.onStay)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                switch (TowerCollider.thisObject.tag)
                {
                    case "Shop":
                        ShopOpen();
                        break;
                    case "Tower":
                        TowerOpen();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #region �ڷ�ƾ
    IEnumerator PanelOnOff()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }
    #endregion

    void ShopOpen() //���� ���� ui
    {
        ShopCanvas.SetActive(true);
    }
    void TowerOpen()
    {
        TowerCanvas.SetActive(true);    //Ÿ��UI ON
    }
    public void PanelCorou()
    {
        StartCoroutine(PanelOnOff());
    }

}
