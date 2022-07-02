using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour, IDamage
{
    #region ����

    private ObjectPoolingManager objectPoolingManager;
    Vector3 spawnPos;

    int jellyIndex = (int)EObjectFlag.jelly;
    int gelatinIndex = (int)EObjectFlag.gelatin;
    int randObj;
    #endregion

    #region ����Ƽ �Լ�

    private void Start()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    #endregion

    #region �Լ�

    // ���� Ȥ�� ����ƾ ����
    void SpawnObject()
    {
        objectPoolingManager.Set(this.gameObject, EObjectFlag.box);

        randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // �������� ����, ����ƾ�� ����

        spawnPos = this.transform.position;
        spawnPos.y += 0.5f;
        objectPoolingManager.Get(EObjectFlag.jelly, spawnPos);
    }

    public void AutoAtkDamaged()
    {
        SpawnObject();
    }
    
    public void SkillDamaged()
    {
        SpawnObject();
    }

    public void Stun(float stunTime)
    {
        SpawnObject();
    }
    #endregion

}
