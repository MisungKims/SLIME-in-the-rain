/**
 * @brief 페이드 아웃하는 텍스트
 * @author 김미성
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    #region 변수
    Material material;

    float alpha;                // 투명도

    public bool isAgain;       // 다시 페이드 아웃 해야하는지?

    private WaitForSeconds waitFor1s = new WaitForSeconds(1);

    #endregion

    #region 유니티 함수
    protected virtual void Awake()
    {
        material = GetComponent<TextMeshProUGUI>().fontMaterial;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(ActiveFalse());
    }
    #endregion

    #region 코루틴
    // Fade Out 코루틴
    protected IEnumerator FadeOut()
    {
        material.SetColor("_FaceColor", Color.Lerp(Color.clear, Color.white, 1));

        yield return waitFor1s;

        alpha = 1;
        while (alpha > 0)
        {
            if (isAgain)  // 다시 처음부터 fade out 시작
            {
                alpha = 1;
                isAgain = false;
                material.SetColor("_FaceColor", Color.Lerp(Color.clear, Color.white, alpha));

                yield return waitFor1s;
            }

            material.SetColor("_FaceColor", Color.Lerp(Color.clear, Color.white, alpha));

            yield return null;

            alpha -= Time.deltaTime;
        }
    }

    protected virtual IEnumerator ActiveFalse()
    {
        yield return StartCoroutine(FadeOut());

        gameObject.SetActive(false);
    }
    #endregion

    // 텍스트를 보여줌
    public void ShowText()
    {
        if (gameObject.activeSelf) isAgain = true;
        else gameObject.SetActive(true);
    }
}
