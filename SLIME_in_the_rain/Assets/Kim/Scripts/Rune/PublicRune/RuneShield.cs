/**
 * @brief 대시 시 실드 룬
 * @author 김미성
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneShield : Rune, IDashRune
{
    GameObject shield;          // 실드 오브젝트

    ObjectPoolingManager objectPoolingManager;
    
    void Start()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    #region 코루틴

    // 대시가 끝났는지 감지하여 shield 오브젝트의 액티브 설정
    IEnumerator DetectDash()
    {
        shield = Slime.Instance.shield;
        shield.SetActive(true);
        shield.transform.localPosition = Vector3.zero;
        
        yield return new WaitForSeconds(1f);

        shield.SetActive(false);
    }
    #endregion

    #region 함수
    public void Dash()
    {
        float amount = statManager.GetIncrementStat("HP", 30);          // TODO : 실드 생성

        StartCoroutine(DetectDash());
    }
    #endregion
}
