using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHP : MonoBehaviour
{
    #region ����
    private Slime slime;
    private Outline outline;

    private StatManager statManager;

    // ������ ������ �ʿ��� ����
    private float distance;
    Vector3 offset;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        StartCoroutine(DetectSlime());
    }


    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ������ Ž�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectSlime()
    {
        outline = GetComponent<Outline>();
        slime = Slime.Instance;
        statManager = StatManager.Instance;

        // �����Ӱ��� �Ÿ��� Ž��
        while (true)
        {
            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // ������ ������ ������ �Ÿ�

            if (distance < 2f)
            {
                if (!outline.enabled) outline.enabled = true;

                if (Input.GetKeyDown(KeyCode.G))
                {
                    StartCoroutine(Recovery());
                    Debug.Log("G");
                }
            }
            else
            {
                if (outline.enabled) outline.enabled = false;
            }

            yield return null;
        }
    }

    // �ִ� ü�±��� ȸ��
    IEnumerator Recovery()
    {
        while (statManager.myStats.HP < statManager.myStats.maxHP)
        {
            statManager.AddHP(0.1f);

            yield return new WaitForSeconds(0.3f);
        }

        statManager.myStats.HP = statManager.myStats.maxHP;
    }
    #endregion
}
