using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Town_Prefab_onClick : MonoBehaviour
{
    public void Remain()
    {
        TextMeshProUGUI remain = this.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();
        Debug.Log(this);

        remain.text = (int.Parse(remain.text) - 1).ToString();
        if (int.Parse(remain.text) <= 0)
        {
            this.transform.GetComponent<Button>().interactable = false;
        }



        
    }

    private void Start()
    {
        this.transform.GetComponent<Button>().onClick.AddListener(Remain);
    }

}
