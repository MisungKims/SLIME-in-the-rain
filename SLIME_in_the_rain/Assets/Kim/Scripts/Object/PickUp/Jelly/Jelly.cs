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
    [SerializeField]
    private int jellyAmount = 50;

    // ĳ��
    JellyManager jellyManager;
    ObjectPoolingManager objectPoolingManager;
    #endregion

    #region ����Ƽ �Լ�

    private void Start()
    {
        jellyManager = JellyManager.Instance;
        objectPoolingManager = ObjectPoolingManager.Instance;
    }
    #endregion

    #region �Լ�

    // ���� ȹ��
    public override void Get()
    {
        jellyManager.JellyCount += jellyAmount;

        objectPoolingManager.Set(this.gameObject, EObjectFlag.jelly);       // ������Ʈ Ǯ�� ��ȯ
    }

    #endregion
}
