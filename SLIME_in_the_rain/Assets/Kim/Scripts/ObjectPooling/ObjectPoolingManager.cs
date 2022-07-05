/**
 * @brief 오브젝트 풀링
 * @author 김미성
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag
{
    shield,
    box,
    jelly,
    gelatin,
    damageText
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


    public List<ObjectPool> objectPoolingList = new List<ObjectPool>();

    public List<ObjectPool> projectilePoolingList = new List<ObjectPool>();
    
    public List<ObjectPool> weaponPoolingList = new List<ObjectPool>();

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

        InitObject();
        InitProjectile();
        InitWeapon();
    }
    #endregion

    #region 함수

    // 오브젝트를 initCount 만큼 생성
    private void InitObject()
    {
        for (int i = 0; i < objectPoolingList.Count; i++)     // poolingList를 탐색해 각 오브젝트를 미리 생성
        {
            for (int j = 0; j < objectPoolingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(objectPoolingList[i].copyObj, objectPoolingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                objectPoolingList[i].queue.Enqueue(tempGb);
            }
        }
    }

    // 투사체를 initCount 만큼 생성
    private void InitProjectile()
    {
        for (int i = 0; i < projectilePoolingList.Count; i++)     // poolingList를 탐색해 각 오브젝트를 미리 생성
        {
            for (int j = 0; j < projectilePoolingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(projectilePoolingList[i].copyObj, projectilePoolingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                projectilePoolingList[i].queue.Enqueue(tempGb);
            }
        }
    }

    // 무기를 initCount 만큼 생성
    private void InitWeapon()
    {
        for (int i = 0; i < weaponPoolingList.Count; i++)     // weaponList를 탐색해 각 무기 오브젝트를 미리 생성
        {
            for (int j = 0; j < weaponPoolingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(weaponPoolingList[i].copyObj, weaponPoolingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                weaponPoolingList[i].queue.Enqueue(tempGb);
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

        if (objectPoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = objectPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(objectPoolingList[index].copyObj, objectPoolingList[index].parent.transform);
        }

        return tempGb;
    }

    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public GameObject Get(EObjectFlag flag, Vector3 pos)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (objectPoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = objectPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(objectPoolingList[index].copyObj, objectPoolingList[index].parent.transform);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }

    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(GameObject gb, EObjectFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        objectPoolingList[index].queue.Enqueue(gb);
    }

    /// <summary>
    /// 투사체 오브젝트를 반환
    /// </summary>
    public GameObject Get(EProjectileFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (projectilePoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = projectilePoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(projectilePoolingList[index].copyObj, projectilePoolingList[index].parent.transform);
        }

        return tempGb;
    }

    /// <summary>
    /// 투사체 오브젝트의 위치, 회전값 조절하여 반환
    /// </summary>
    public GameObject Get(EProjectileFlag flag, Vector3 pos, Vector3 rot)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (projectilePoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = projectilePoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(projectilePoolingList[index].copyObj, projectilePoolingList[index].parent.transform);
        }

        tempGb.transform.position = pos;
        tempGb.transform.eulerAngles = rot;

        return tempGb.gameObject;
    }


    /// <summary>
    /// 다 쓴 투사체 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(GameObject gb, EProjectileFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        projectilePoolingList[index].queue.Enqueue(gb);
    }

    /// <summary>
    /// 무기 오브젝트를 반환
    /// </summary>
    public GameObject Get(EWeaponType type)
    {
        int index = (int)type;
        GameObject tempGb;

        if (weaponPoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = weaponPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(weaponPoolingList[index].copyObj, weaponPoolingList[index].parent.transform);
        }

        return tempGb;
    }

    /// <summary>
    /// 다 쓴 무기 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(Weapon gb)
    {
        int index = (int)gb.weaponType;
        gb.transform.SetParent(weaponPoolingList[index].parent.transform);
        gb.gameObject.SetActive(false);

        weaponPoolingList[index].queue.Enqueue(gb.gameObject);
    }
    #endregion
}
