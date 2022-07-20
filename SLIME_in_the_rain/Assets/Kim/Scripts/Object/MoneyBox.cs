/**
 * @brief 재화 스폰 박스
 * @author 김미성
 * @date 22-07-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour, IDamage
{
    #region 변수

    private bool isDamaged;

    private ObjectPoolingManager objectPoolingManager;
    Vector3 spawnPos;

    int jellyIndex = (int)EObjectFlag.jelly;
    int gelatinIndex = (int)EObjectFlag.gelatin;
    int randObj;

    private Animator anim;

    private GameObject pickUpObj;
    #endregion

    #region 유니티 함수

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    private void OnEnable()
    {
        isDamaged = false;
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
        if (isDamaged) return;

        isDamaged = true;

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
