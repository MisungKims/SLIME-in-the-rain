using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemEft/Weapon/swap")]
public class ItemWeaponSwap : ItemEffect
{
    DissolutionUI dissolutionUI;


    public override bool ExecuteRole(int _slotNum) //누를시 -> 현재 무기만 들어감, 분해 누르고 누를때 무기ui가 안쪽으로 이동해서 분해정보 받아와서 분해 하도록 만들기
    {
        Dissolution = GameObject.Find("Dissolution");
        dissolutionUI = DissolutionUI.Instance;
        if (!Dissolution.transform.GetChild(0).gameObject.activeSelf)
        {
            Debug.Log("true");
            Slime.Instance.EquipWeapon(weapon);
            return true;
        }
        else
        {
            Debug.Log("false");
            dissolutionUI.DissolutionWeapon(_slotNum);
            return false;
        }
        return false;
    }
}
