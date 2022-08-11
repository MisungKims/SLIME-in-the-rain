using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


/// <summary>
/// panel 뜨는거 관리
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
    #region 코루틴
    IEnumerator PanelOnOff()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }
    #endregion

    void ShopOpen() //상점 관련 ui
    {
        ShopCanvas.SetActive(true);
    }
    void TowerOpen()
    {
        TowerCanvas.SetActive(true);    //타워UI ON
    }
    public void PanelCorou()
    {
        StartCoroutine(PanelOnOff());
    }

}
