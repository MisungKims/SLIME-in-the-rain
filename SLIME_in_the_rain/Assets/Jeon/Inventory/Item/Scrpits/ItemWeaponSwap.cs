using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemEft/Weapon/swap")]
public class ItemWeaponSwap : ItemEffect
{
    DissolutionUI dissolutionUI;


    public override bool ExecuteRole(int _slotNum) //������ -> ���� ���⸸ ��, ���� ������ ������ ����ui�� �������� �̵��ؼ� �������� �޾ƿͼ� ���� �ϵ��� �����
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
