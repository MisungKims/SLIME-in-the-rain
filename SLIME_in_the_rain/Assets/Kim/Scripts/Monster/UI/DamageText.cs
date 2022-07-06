/**
 * @brief 데미지 수치 텍스트
 * @author 김미성
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : FadeOutText
{
    #region 변수
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

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));         // 위로 올라감
    }
    #endregion

    #region 코루틴
    // Fade Out 뒤 오브젝트 풀에 반환
    protected override IEnumerator ActiveFalse()
    {
        yield return StartCoroutine(FadeOut());

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.damageText);
    }
    #endregion
}
