/**
 * @brief ü�� ȸ�� ��
 * @author ��̼�
 * @date 22-08-06
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHPMap : MapManager
{
    [SerializeField]
    private RecoveryHP recoveryHPObject;

    private void Start()
    {
        StartCoroutine(DetectUsed());
    }

    IEnumerator DetectUsed()
    {
        while (!recoveryHPObject.IsUsed)
        {
            yield return null;
        }

        ClearMap();
    }    
}
