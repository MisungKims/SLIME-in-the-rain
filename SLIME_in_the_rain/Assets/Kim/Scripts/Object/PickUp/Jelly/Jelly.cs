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
    // ĳ��
    JellyManager jellyManager;
    ObjectPoolingManager objectPoolingManager;
    #endregion

    #region ����Ƽ �Լ�

    protected override void Start()
    {
        jellyManager = JellyManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;

        base.Start();
    }
    #endregion

    #region �Լ�

    // ���� ȹ��
    public override void Get()
    {
        jellyManager.JellyCount++;

        objectPoolingManager.Set(this.gameObject, EObjectFlag.jelly);       // ������Ʈ Ǯ�� ��ȯ
    }

    #endregion
}