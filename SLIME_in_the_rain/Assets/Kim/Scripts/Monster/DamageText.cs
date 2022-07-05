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
    private float moveSpeed = 2f;

    private TextMeshProUGUI text;

   // Color32 color = new Color32(255, 255, 255, 255);
    Color32 red = new Color32(164, 11, 0, 255);

    private int damage;
    public int Damage
    {
        set 
        { 
            damage = value; 
            text.text = damage.ToString();

            text.color = red;
        }
    }
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        text = GetComponent<TextMeshProUGUI>();
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

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.damageText);
    }
    #endregion
}
