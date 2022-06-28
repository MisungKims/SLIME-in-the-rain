/**
 * @brief ������Ʈ Ǯ��
 * @author ��̼�
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // �迭�̳� ����Ʈ�� ����
{
    arrow
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

    public List<ObjectPool> poolingList = new List<ObjectPool>();

    public List<ObjectPool> weaponPollingList = new List<ObjectPool>();

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

        InitWeapon();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    private void InitObject()
    {
        for (int i = 0; i < poolingList.Count; i++)     // poolingList�� Ž���� �� ������Ʈ�� �̸� ����
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
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    private void InitWeapon()
    {
        for (int i = 0; i < weaponPollingList.Count; i++)     // weaponList�� Ž���� �� ���� ������Ʈ�� �̸� ����
        {
            for (int j = 0; j < weaponPollingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(weaponPollingList[i].copyObj, weaponPollingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                weaponPollingList[i].queue.Enqueue(tempGb);
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

        if (poolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = poolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
        }

        return tempGb;
    }

    public GameObject Get(EObjectFlag flag, Vector3 pos, Vector3 rot)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (poolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = poolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(poolingList[index].copyObj, poolingList[index].parent.transform);
        }

        tempGb.transform.position = pos;
        tempGb.transform.eulerAngles = rot;

        return tempGb.gameObject;
    }


    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(GameObject gb, EObjectFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        poolingList[index].queue.Enqueue(gb);
    }


    /// <summary>
    /// ���� ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EWeaponType type)
    {
        int index = (int)type;
        GameObject tempGb;

        if (weaponPollingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = weaponPollingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(weaponPollingList[index].copyObj, weaponPollingList[index].parent.transform);
        }

        return tempGb;
    }


    /// <summary>
    /// �� �� ���� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(Weapon gb)
    {
        int index = (int)gb.weaponType;
        gb.transform.SetParent(weaponPollingList[index].parent.transform);
        gb.gameObject.SetActive(false);

        weaponPollingList[index].queue.Enqueue(gb.gameObject);
    }
    #endregion
}
