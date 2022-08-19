/**
 * @brief ������ ��ġ �ؽ�Ʈ
 * @author ��̼�
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : UpText
{
    #region ����
   
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

    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        text = GetComponent<TextMeshProUGUI>();
    }
    #endregion

}
