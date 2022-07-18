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

    private Animator anim;

    private GameObject pickUpObj;
    #endregion

    #region 유니티 함수

    private void Start()
    {
        anim = GetComponent<Animator>();
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    #endregion
    
    IEnumerator TakeDamaged()
    {
        anim.SetBool("TakeDamaged", true);

        while (pickUpObj.activeSelf)
        {
            yield return null;
        }

        objectPoolingManager.Set(this.gameObject, EObjectFlag.box);
    }


    #region 함수

    // 젤리 혹은 젤라틴 스폰
    void SpawnObject()
    {
        randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // 랜덤으로 젤리, 젤라틴을 정함

        spawnPos = this.transform.position;
        spawnPos.y += 0.5f;
        pickUpObj = objectPoolingManager.Get((EObjectFlag)randObj, spawnPos);

        StartCoroutine(TakeDamaged());
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
