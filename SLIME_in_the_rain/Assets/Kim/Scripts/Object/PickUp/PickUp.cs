/**
 * @brief �Ⱦ� ������ ������Ʈ
 * @author ��̼�
 * @date 22-07-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    #region ����
    private Slime slime;

    // ������ ������ �ʿ��� ����
    float velocity;
    float acceleration = 0.2f;
    float distance;
    Vector3 dir;
    Vector3 targetPos;
    Vector3 offset;
    #endregion

    #region ����Ƽ �Լ�

    protected virtual void Start()
    {
        slime = Slime.Instance;

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
        // �����Ӱ��� �Ÿ��� Ž��
        while (true)
        {
            dir = (slime.transform.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);      // ���ӵ�

            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // ������ ������ ������ �Ÿ�

            // �Ÿ��� 1�� ���ų� ���� �� �������� ��ġ�� �̵�
            if (distance <= 1.0f)
            {
                targetPos = Vector3.zero;
                targetPos.x = transform.position.x + (dir.x * velocity);
                targetPos.y = transform.position.y;
                targetPos.z = transform.position.z + (dir.z * velocity);

                transform.position = targetPos;

                // �Ÿ��� ���� ����� �� ������ ȹ��
                if (distance < 0.35f) Get();
            }
            else
            {
                velocity = 0.0f;
            }

            yield return null;
        }
    }
    #endregion

    #region �Լ�

    // ������ ȹ��
    public abstract void Get();

    #endregion
}
