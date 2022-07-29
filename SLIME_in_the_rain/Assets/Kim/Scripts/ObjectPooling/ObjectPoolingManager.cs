/**
 * @brief ������Ʈ Ǯ��
 * @author ��̼�
 * @date 22-07-18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag
{
    box,
    jelly,
    gelatin,
    minimapIcon
}

public class ObjectPoolingManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get { return instance; }
    }
    #endregion



    public List<ObjectPool> objectPoolingList = new List<ObjectPool>();

    public List<ObjectPool> projectilePoolingList = new List<ObjectPool>();
    
    public List<ObjectPool> weaponPoolingList = new List<ObjectPool>();

    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitObject();
        InitProjectile();
        InitWeapon();
    }
    #endregion

    #region �Լ�

    // ������Ʈ�� initCount ��ŭ ����
    private void InitObject()
    {
        for (int i = 0; i < objectPoolingList.Count; i++)     // poolingList�� Ž���� �� ������Ʈ�� �̸� ����
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

    // ����ü�� initCount ��ŭ ����
    private void InitProjectile()
    {
        for (int i = 0; i < projectilePoolingList.Count; i++)     // poolingList�� Ž���� �� ������Ʈ�� �̸� ����
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

    // ���⸦ initCount ��ŭ ����
    private void InitWeapon()
    {
        for (int i = 0; i < weaponPoolingList.Count; i++)     // weaponList�� Ž���� �� ���� ������Ʈ�� �̸� ����
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
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EObjectFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (objectPoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = objectPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(objectPoolingList[index].copyObj, objectPoolingList[index].parent.transform);
        }

        if(flag.Equals(EObjectFlag.gelatin))
        {
            tempGb.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.AllitemDB[Random.Range(0, 15)]);
        }

        return tempGb;
    }

    /// <summary>
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EObjectFlag flag, Vector3 pos)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (objectPoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = objectPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(objectPoolingList[index].copyObj, objectPoolingList[index].parent.transform);
        }

        if (flag.Equals(EObjectFlag.gelatin))       // ��ȯ�Ϸ��� ������Ʈ�� ����ƾ�� �� ������ ���� �ʿ�
        {
            tempGb.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.AllitemDB[Random.Range(0, 15)]);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }

    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(GameObject gb, EObjectFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        if(flag.Equals(EObjectFlag.minimapIcon)) gb.transform.SetParent(objectPoolingList[index].parent.transform);     // �̴ϸ� �������� �θ� ���� �ʿ�

        objectPoolingList[index].queue.Enqueue(gb);
    }

    /// <summary>
    /// ����ü ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EProjectileFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (projectilePoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = projectilePoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(projectilePoolingList[index].copyObj, projectilePoolingList[index].parent.transform);
        }

        return tempGb;
    }

    /// <summary>
    /// ����ü ������Ʈ�� ��ġ, ȸ���� �����Ͽ� ��ȯ
    /// </summary>
    public GameObject Get(EProjectileFlag flag, Vector3 pos, Vector3 rot)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (projectilePoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = projectilePoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(projectilePoolingList[index].copyObj, projectilePoolingList[index].parent.transform);
        }

        tempGb.transform.position = pos;
        tempGb.transform.eulerAngles = rot;

        return tempGb.gameObject;
    }


    /// <summary>
    /// �� �� ����ü ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(GameObject gb, EProjectileFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        projectilePoolingList[index].queue.Enqueue(gb);
    }

    /// <summary>
    /// ���� ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EWeaponType type)
    {
        int index = (int)type;
        GameObject tempGb;

        if (weaponPoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = weaponPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(weaponPoolingList[index].copyObj, weaponPoolingList[index].parent.transform);
        }

        return tempGb;
    }

    /// <summary>
    /// �� �� ���� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(Weapon gb)
    {
        int index = (int)gb.weaponType;
        gb.transform.SetParent(weaponPoolingList[index].parent.transform);
        gb.gameObject.SetActive(false);

        weaponPoolingList[index].queue.Enqueue(gb.gameObject);
    }


    public FieldItems Get2(string type)
    {
        GameObject tempGb;

        if (objectPoolingList[2].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = objectPoolingList[2].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(objectPoolingList[2].copyObj, objectPoolingList[2].parent.transform);
        }

        FieldItems tempfield;
        tempfield = tempGb.GetComponent<FieldItems>();

        for (int i = 0; i < ItemDatabase.Instance.AllitemDB.Count; i++)
        {
            if (ItemDatabase.Instance.AllitemDB[i].itemName == type)
            {
                tempfield.SetItemPool(ItemDatabase.Instance.AllitemDB[i]);
            }
        }

        return tempfield;
    }

    #endregion
}
