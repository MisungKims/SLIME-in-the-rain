using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemEft/Gelatin/Comb")]
public class ItemComb : ItemEffect
{
    CombinationUI combinationUI;

    private GameObject Combination;

    public override bool ExecuteRole(int _slotNum) //������ -> ���� ���⸸ ��, ���� ������ ������ ����ui�� �������� �̵��ؼ� �������� �޾ƿͼ� ���� �ϵ��� �����
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
