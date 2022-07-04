/**
 * @brief ��� �� �ǵ� ��
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneShield : Rune, IDashRune
{
    GameObject shield;          // �ǵ� ������Ʈ

    ObjectPoolingManager objectPoolingManager;
    
    void Start()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    #region �ڷ�ƾ

    // ��ð� �������� �����Ͽ� shield ������Ʈ�� ��Ƽ�� ����
    IEnumerator DetectDash()
    {
        shield = objectPoolingManager.Get(EObjectFlag.shield);
        shield.transform.localPosition = Vector3.zero;
        
        yield return new WaitForSeconds(1f);

        objectPoolingManager.Set(shield, EObjectFlag.shield);
    }
    #endregion

    #region �Լ�
    public void Dash()
    {
        float amount = statManager.GetIncrementStat("HP", 30);          // TODO : �ǵ� ����

        StartCoroutine(DetectDash());
    }
    #endregion
}
