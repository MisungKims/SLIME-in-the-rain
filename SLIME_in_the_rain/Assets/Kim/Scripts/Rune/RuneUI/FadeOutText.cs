using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    #region ����

    Material material;

    float alpha;                // ����

    public bool isAgain;       // �ٽ� ���̵� �ƿ� �ؾ��ϴ���?

    private WaitForSeconds waitFor1s = new WaitForSeconds(1);

    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        material = GetComponent<TextMeshProUGUI>().fontMaterial;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
    }
    #endregion

    #region �ڷ�ƾ
    // Fade Out �ڷ�ƾ
    IEnumerator FadeOut()
    {
        material.SetColor("_FaceColor", Color.Lerp(Color.clear, Color.white, 1));

        yield return waitFor1s;

        alpha = 1;
        while (alpha > 0)
        {
            if (isAgain)  // �ٽ� ó������ fade out ����
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

        gameObject.SetActive(false);
    }
    #endregion
}
