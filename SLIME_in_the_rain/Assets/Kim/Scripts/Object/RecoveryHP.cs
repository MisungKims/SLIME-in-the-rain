using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHP : MonoBehaviour
{
    #region ����
    private Slime slime;
    private Outline outline;

    [SerializeField]
    private float speed = 0.08f;        // HP ȸ�� �ӵ�

    private bool isUsed = false;

    // ������ ������ �ʿ��� ����
    private float distance;
    Vector3 offset;

    // ĳ��
    private StatManager statManager;
    private WaitForSeconds waitForSeconds;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(speed);
    }

    private void OnEnable()
    {
        isUsed = false;

        StartCoroutine(DetectSlime());
    }


    #endregion

    #region �ڷ�ƾ
    // ������ Ž�� �ڷ�ƾ
    IEnumerator DetectSlime()
    {
        outline = GetComponent<Outline>();
        slime = Slime.Instance;
        statManager = StatManager.Instance;

        // �����Ӱ��� �Ÿ��� Ž��
        while (!isUsed)
        {
            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // ������ ������ ������ �Ÿ�

            if (distance < 2f)
            {
                if (!outline.enabled) outline.enabled = true;           // �Ÿ��� ����� �� �ܰ��� ǥ��

                if (Input.GetKeyDown(KeyCode.G))                // GŰ�� ������ HP ȸ��
                {
                    StartCoroutine(Recovery());
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
        isUsed = true;
        outline.enabled = false;

        while (statManager.myStats.HP < statManager.myStats.maxHP)
        {
            statManager.AddHP(1f);

            yield return waitForSeconds;
        }

        statManager.myStats.HP = statManager.myStats.maxHP;
    }
    #endregion
}
