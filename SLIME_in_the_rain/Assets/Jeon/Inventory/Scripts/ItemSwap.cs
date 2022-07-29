using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemEft/Weapon/swap")]
public class ItemSwap : ItemEffect
{
    DissolutionUI dissolutionUI;
    
  
    private GameObject Dissolution;
    public override bool ExecuteRole(int _slotNum) //������ -> ���� ���⸸ ��, ���� ������ ������ ����ui�� �������� �̵��ؼ� �������� �޾ƿͼ� ���� �ϵ��� �����
    {
        Dissolution = GameObject.Find("Dissolution");
        dissolutionUI = DissolutionUI.Instance;
       if(Dissolution.transform.GetChild(1).gameObject.activeSelf)
        {
            dissolutionUI.DissolutionWeapon(_slotNum);
            return false;
        }
        else
        {
            Slime.Instance.EquipWeapon(weapon);
            return true;
        }
    }
}
