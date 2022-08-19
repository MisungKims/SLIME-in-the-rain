/**
 * @brief ���� �ö󰡴� �ؽ�Ʈ
 * @author ��̼�
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
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);         // ���� �ö�
    }


    #region �ڷ�ƾ
    // Fade Out �� ������Ʈ Ǯ�� ��ȯ
    protected override IEnumerator ActiveFalse()
    {
        yield return StartCoroutine(FadeOut());

        uiPoolingManager.Set(this.gameObject, EUIFlag.damageText);
    }
    #endregion
}
