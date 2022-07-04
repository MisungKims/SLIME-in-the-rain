using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    #region 변수
    public Item item;
    public MeshRenderer gb;
    #endregion


    public void SetItem(Item _item) //아이템 셋팅
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemIcon = _item.itemIcon;
        item.itemGB = _item.itemGB;
        item.itemMaterial = _item.itemMaterial;
        gb.material = item.itemMaterial;
    }
   


    #region 감지
    private Slime slime;
    private Inventory inventory;

    // 슬라임 감지에 필요한 변수
    float velocity;
    float acceleration = 0.2f;
    float distance;
    Vector3 dir;
    Vector3 targetPos;
    Vector3 offset;

    // 캐싱
    //  JellyManager jellyManager;
    

    #region 유니티 함수


    void Start()
    {
        inventory = Inventory.Instance;
        slime = Slime.Instance;

        // jellyManager = JellyManager.Instance;

        StartCoroutine(DetectSlime());
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 슬라임 탐지 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectSlime()
    {
        // 슬라임과의 거리를 탐지
        while (true)
        {
            dir = (slime.transform.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);      // 가속도

            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // 젤리와 슬라임 사이의 거리

            // 거리가 1과 같거나 작을 때 슬라임의 위치로 이동
            if (distance <= 1.0f)
            {
                targetPos = Vector3.zero;
                targetPos.x = transform.position.x + (dir.x * velocity);
                targetPos.y = transform.position.y;
                targetPos.z = transform.position.z + (dir.z * velocity);

                transform.position = targetPos;

                // 거리가 많이 가까울 때 아이템 획득
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

    #region 함수
    /// <summary>
    /// 아이템 획득
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
            Debug.Log("아이템 공간 없음");
        }
    }
    #endregion
}
