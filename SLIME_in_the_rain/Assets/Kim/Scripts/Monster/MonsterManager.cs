/**
 * @brief ���� �Ŵ���
 * @author ��̼�
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static MonsterManager instance = null;
    public static MonsterManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    // ���Ͱ� ���ƴٴ� �� �ִ� ����
    [SerializeField]
    private GameObject range;
    private BoxCollider rangeCollider;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        rangeCollider = range.GetComponent<BoxCollider>();
    }
    #endregion

    // ���Ͱ� ���ƴٴ� �� �ִ� ���� �� ������ ��ġ�� ��ȯ
    public Vector3 GetRandomPosition()
    {
        Vector3 originPosition = range.transform.position;

        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;

        return respawnPosition;
    }
}
