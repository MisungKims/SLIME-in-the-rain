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
    protected Slime slime;

    // ������ ������ �ʿ��� ����
    public bool canDetect = true;
    protected float velocity;
    protected float acceleration = 0.2f;
    protected float distance;
    protected Vector3 dir;
    protected Vector3 targetPos;
    private Vector3 offset;
    private bool isDetect = false;
    #endregion

    #region ����Ƽ �Լ�
    protected virtual void Awake()
    {
        slime = Slime.Instance;
    }

    protected virtual void OnEnable()
    {
        canDetect = true;
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
        slime = Slime.Instance;

        // �����Ӱ��� �Ÿ��� Ž��
        while (canDetect)
        {
            dir = (slime.transform.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);      // ���ӵ�

            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // ������ ������ ������ �Ÿ�

            // �Ÿ��� 1�� ���ų� ���� �� �������� ��ġ�� �̵� (����ٴ�)
            if (distance <= 1f)
            {
                if (!isDetect) isDetect = true;

                targetPos = Vector3.zero;
                targetPos.x = transform.position.x + (dir.x * velocity);
                targetPos.y = transform.position.y;
                targetPos.z = transform.position.z + (dir.z * velocity);

                transform.position = targetPos;

                // �Ÿ��� ���� ����� �� ������ ȹ��
                if (distance < 0.5f) Get();
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