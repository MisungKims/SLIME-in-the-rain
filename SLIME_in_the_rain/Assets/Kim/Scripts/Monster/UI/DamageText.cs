/**
 * @brief ������ ��ġ �ؽ�Ʈ
 * @author ��̼�
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : FadeOutText
{
    #region ����
    private float moveSpeed = 200f;

    private TextMeshProUGUI text;

    Color32 red = new Color32(164, 11, 0, 255);

    private float damage;
    public float Damage
    {
        set 
        { 
            damage = value; 
            text.text = damage % 1 == 0 ? damage.ToString() : damage.ToString("f1");

            text.color = red;
        }
    }
    private UIObjectPoolingManager uiPoolingManager;
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        text = GetComponent<TextMeshProUGUI>();
        uiPoolingManager = UIObjectPoolingManager.Instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

       transform.position = Vector3.zero;
    }

    void Update()
    {
       transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));         // ���� �ö�
    }
    #endregion

    #region �ڷ�ƾ
    // Fade Out �� ������Ʈ Ǯ�� ��ȯ
    protected override IEnumerator ActiveFalse()
    {
        yield return StartCoroutine(FadeOut());

        uiPoolingManager.Set(this.gameObject, EUIFlag.damageText);
    }
    #endregion
}
