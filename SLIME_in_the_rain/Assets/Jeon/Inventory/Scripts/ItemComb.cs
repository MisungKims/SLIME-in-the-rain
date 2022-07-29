using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemEft/Gelatin/Comb")]
public class ItemComb : ItemEffect
{
    CombinationUI combinationUI;

    private GameObject Combination;

    public override bool ExecuteRole(int _slotNum) //누를시 -> 현재 무기만 들어감, 분해 누르고 누를때 무기ui가 안쪽으로 이동해서 분해정보 받아와서 분해 하도록 만들기
    {
        Combination = GameObject.Find("Combination");
        combinationUI = CombinationUI.Instance;
        if (Combination.transform.GetChild(1).gameObject.activeSelf)
        {
            if (combinationUI.gelatin1It != Inventory.Instance.items[_slotNum])
            {
            combinationUI.inputEndCount(_slotNum);
            }
        }
            return false;
    }
}
