using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    #region ����
    public Item item;
    public MeshRenderer gb;
    #endregion


    public void SetItem(Item _item) //������ ����
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemIcon = _item.itemIcon;
        item.itemGB = _item.itemGB;
        item.itemMaterial = _item.itemMaterial;
        gb.material = item.itemMaterial;
    }
   


    #region ����
    private Slime slime;
    private Inventory inventory;

    // ������ ������ �ʿ��� ����
    float velocity;
    float acceleration = 0.2f;
    float distance;
    Vector3 dir;
    Vector3 targetPos;
    Vector3 offset;

    // ĳ��
    //  JellyManager jellyManager;
    

    #region ����Ƽ �Լ�


    void Start()
    {
        inventory = Inventory.Instance;
        slime = Slime.Instance;

        // jellyManager = JellyManager.Instance;

        StartCoroutine(DetectSlime());
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ������ Ž�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectSlime()
    {
        // �����Ӱ��� �Ÿ��� Ž��
        while (true)
        {
            dir = (slime.transform.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);      // ���ӵ�

            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // ������ ������ ������ �Ÿ�

            // �Ÿ��� 1�� ���ų� ���� �� �������� ��ġ�� �̵�
            if (distance <= 1.0f)
            {
                targetPos = Vector3.zero;
                targetPos.x = transform.position.x + (dir.x * velocity);
                targetPos.y = transform.position.y;
                targetPos.z = transform.position.z + (dir.z * velocity);

                transform.position = targetPos;

                // �Ÿ��� ���� ����� �� ������ ȹ��
                if (distance < 0.35f)
                {
                        Get();
                }
            }
            else
            {
                velocity = 0.0f;
            }

            yield return null;
        }
    }
    #endregion
    #endregion

    #region �Լ�
    /// <summary>
    /// ������ ȹ��
    /// </summary>
    void Get()
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
    #endregion
}
