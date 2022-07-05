/**
 * @brief ������Ʈ Ǯ��
 * @author ��̼�
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
    #region ����
    // �̱���
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get { return instance; }
    }


    public List<ObjectPool> objectPoolingList = new List<ObjectPool>();

    public List<ObjectPool> projectilePoolingList = new List<ObjectPool>();
    
    public List<ObjectPool> weaponPoolingList = new List<ObjectPool>();

    #endregion

    #region ����Ƽ �Լ�
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
    #endregion
}
