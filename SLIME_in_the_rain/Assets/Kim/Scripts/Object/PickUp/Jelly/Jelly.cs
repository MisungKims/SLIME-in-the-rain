/**
 * @brief ���� ������Ʈ
 * @author ��̼�
 * @date 22-07-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Jelly : PickUp
{
    #region ����
    private JellyGrade jellyGrade;
    private int jellyAmount;
    private MeshRenderer meshRenderer;

    // ĳ��
    JellyManager jellyManager;
    #endregion

    #region ����Ƽ �Լ�
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

    #region �Լ�

    void InitJelly()
    {
        jellyGrade = JellyManager.Instance.GetRandomJelly();
        meshRenderer.material = jellyGrade.mat;
        jellyAmount = jellyGrade.jellyAmount;
    }

    // ���� ȹ��
    public override void Get()
    {
        jellyManager.JellyCount += jellyAmount;

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.jelly);       // ������Ʈ Ǯ�� ��ȯ
    }

    #endregion
}
