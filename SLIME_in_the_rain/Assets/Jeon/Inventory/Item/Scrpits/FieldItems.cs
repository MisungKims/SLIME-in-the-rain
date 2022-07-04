using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : PickUp
{
    #region ����
    public Item item;
    public MeshRenderer gb;
    Inventory inventory;
    Slime slime;
    #endregion

    #region ����Ƽ �Լ�
    protected override void Start()
    {
        inventory = Inventory.Instance;
        slime = Slime.Instance;
        //jellyManager = JellyManager.Instance;
        //objectPoolingManager = ObjectPoolingManager.Instance;

        base.Start();
    }

#endregion

    #region �Լ�
    /// <summary>
    /// ������ ȹ��
    /// </summary>
    public override void Get()
    {
        if (inventory.items.Count < inventory.SlotCount)
        {
            inventory.items.Add(item);
            if (inventory.onChangedItem != null)
            {

                inventory.onChangedItem.Invoke();
            }
            this.gameObject.SetActive(false);
        }
        else
        {
            velocity = -velocity;
            targetPos = Vector3.zero;
            targetPos.x = transform.position.x + (dir.x * velocity);
            targetPos.y = transform.position.y;
            targetPos.z = transform.position.z + (dir.z * velocity);

            transform.position = targetPos;
            Debug.Log("������ ���� ����");
        }
    }

    public void SetItem(Item _item) //������ ����
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemIcon = _item.itemIcon;
        item.itemGB = _item.itemGB;
        item.itemMaterial = _item.itemMaterial;
        gb.material = item.itemMaterial;
    }

    #endregion
}
