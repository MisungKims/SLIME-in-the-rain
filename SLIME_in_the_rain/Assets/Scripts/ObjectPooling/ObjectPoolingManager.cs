/**
 * @brief ������Ʈ Ǯ��
 * @author ��̼�
 * @date 22-06-01
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectFlag // �迭�̳� ����Ʈ�� ����
{
    
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

    public List<ObjectPool> weaponPrefabList = new List<ObjectPool>();

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


        Init();

        InitWeapon();
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// �ʱ⿡ initCount ��ŭ ����
    /// </summary>
    private void Init()
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
        for (int i = 0; i < weaponPrefabList.Count; i++)     // weaponList�� Ž���� �� ���� ������Ʈ�� �̸� ����
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

        if (weaponPrefabList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = weaponPrefabList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(weaponPrefabList[index].copyObj, weaponPrefabList[index].parent.transform);
        }

        return tempGb;
    }


    /// <summary>
    /// �� �� ���� ������Ʈ�� ť�� ������
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
