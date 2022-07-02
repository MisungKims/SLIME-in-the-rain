using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour, IDamage
{
    #region 변수

    private ObjectPoolingManager objectPoolingManager;
    Vector3 spawnPos;

    int jellyIndex = (int)EObjectFlag.jelly;
    int gelatinIndex = (int)EObjectFlag.gelatin;
    int randObj;
    #endregion

    #region 유니티 함수

    private void Start()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    #endregion

    #region 함수

    // 젤리 혹은 젤라틴 스폰
    void SpawnObject()
    {
        objectPoolingManager.Set(this.gameObject, EObjectFlag.box);

        randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // 랜덤으로 젤리, 젤라틴을 정함

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
