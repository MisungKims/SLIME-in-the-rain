/**
 * @brief 위로 올라가는 텍스트
 * @author 김미성
 * @date 22-08-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpText : FadeOutText
{
    private float moveSpeed = 200f;
    private UIObjectPoolingManager uiPoolingManager;

    protected override void Awake()
    {
        base.Awake();

        uiPoolingManager = UIObjectPoolingManager.Instance;
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);         // 위로 올라감
    }


    #region 코루틴
    // Fade Out 뒤 오브젝트 풀에 반환
    protected override IEnumerator ActiveFalse()
    {
        yield return StartCoroutine(FadeOut());

        uiPoolingManager.Set(this.gameObject, EUIFlag.damageText);
    }
    #endregion
}
