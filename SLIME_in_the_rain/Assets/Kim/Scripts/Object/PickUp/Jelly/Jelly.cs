/**
 * @brief 젤리 오브젝트
 * @author 김미성
 * @date 22-07-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : PickUp
{
    #region 변수
    [SerializeField]
    private int jellyAmount = 50;

    // 캐싱
    JellyManager jellyManager;
    ObjectPoolingManager objectPoolingManager;
    #endregion

    #region 유니티 함수

    private void Start()
    {
        jellyManager = JellyManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
    }
    #endregion

    #region 함수

    // 젤리 획득
    public override void Get()
    {
        jellyManager.JellyCount += jellyAmount;

        objectPoolingManager.Set(this.gameObject, EObjectFlag.jelly);       // 오브젝트 풀에 반환
    }

    #endregion
}
