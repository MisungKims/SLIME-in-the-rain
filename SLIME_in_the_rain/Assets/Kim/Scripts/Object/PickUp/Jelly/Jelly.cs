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
    private JellyGrade jellyGrade;
    private int jellyAmount;
    private MeshRenderer meshRenderer;

    // 캐싱
    JellyManager jellyManager;
    #endregion

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        meshRenderer = GetComponent<MeshRenderer>();

        jellyManager = JellyManager.Instance;
    }

    protected override void OnEnable()
    {
        InitJelly();

        base.OnEnable();
    }

    #endregion

    #region 함수

    void InitJelly()
    {
        jellyGrade = JellyManager.Instance.GetRandomJelly();
        meshRenderer.material = jellyGrade.mat;
        jellyAmount = jellyGrade.jellyAmount;
    }

    // 젤리 획득
    public override void Get()
    {
        jellyManager.JellyCount += jellyAmount;

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.jelly);       // 오브젝트 풀에 반환
    }

    #endregion
}
