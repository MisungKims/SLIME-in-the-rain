/**
 * @brief 오브젝트 풀링
 * @author 김미성
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // 배열이나 리스트의 순서
{
    
}

public class ObjectPoolingManager : MonoBehaviour
{
    #region 변수
    // 싱글톤
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get { return instance; }
    }

    public List<ObjectPool> poolingList = new List<ObjectPool>();

    public List<ObjectPool> weaponPrefabList = new List<ObjectPool>();

    #endregion

    #region 유니티 함수
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }


        Init();

        InitWeapon();
    }
    #endregion

    #region 함수
    /// <summary>
    /// 초기에 initCount 만큼 생성
    /// </summary>
    private void Init()
    {
        for (int i = 0; i < poolingList.Count; i++)     // poolingList를 탐색해 각 오브젝트를 미리 생성
        {
            for (int j = 0; j < poolingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(poolingList[i].copyObj, poolingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                poolingList[i].queue.Enqueue(tempGb);
            }
        }
    }

    /// <summary>
    /// 초기에 initCount 만큼 생성
    /// </summary>
    private void InitWeapon()
    {
        for (int i = 0; i < weaponPrefabList.Count; i++)     // weaponList를 탐색해 각 무기 오브젝트를 미리 생성
        {
            for (int j = 0; j < weaponPrefabList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(weaponPrefabList[i].copyObj, weaponPrefabList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                weaponPrefabList[i].queue.Enqueue(tempGb);
            }
        }
    }


    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public GameObject Get(EObjectFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (poolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = poolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
        }

        return tempGb;
    }


    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(GameObject gb, EObjectFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        poolingList[index].queue.Enqueue(gb);
    }


    /// <summary>
    /// 무기 오브젝트를 반환
    /// </summary>
    public GameObject Get(EWeaponType type)
    {
        int index = (int)type;
        GameObject tempGb;

        if (weaponPrefabList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = weaponPrefabList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(weaponPrefabList[index].copyObj, weaponPrefabList[index].parent.transform);
        }

        return tempGb;
    }


    /// <summary>
    /// 다 쓴 무기 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(Weapon gb)
    {
        int index = (int)gb.weaponType;
        gb.transform.SetParent(weaponPrefabList[index].parent.transform);
        gb.gameObject.SetActive(false);

        weaponPrefabList[index].queue.Enqueue(gb.gameObject);
    }
    #endregion
}
