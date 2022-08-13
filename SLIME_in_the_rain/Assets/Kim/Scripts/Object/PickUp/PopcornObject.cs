/**
 * @brief ���� �� ����ó�� Ƣ�� ������Ʈ
 * @author ��̼�
 * @date 22-08-13
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornObject : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 pos;

    [SerializeField]
    private float force = 250.0f;
    [SerializeField]
    private float yPos = 2f;

    #region ����Ƽ �Լ�
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (rigid)
        {
            rigid.AddForce(Vector3.up * force, ForceMode.Force);
            rigid.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rigid && other.transform.CompareTag("Land"))
        {
            rigid.useGravity = false;
            rigid.constraints = RigidbodyConstraints.FreezeAll;

            pos = transform.position;
            pos.y = yPos;
            transform.position = pos;
        }
    }
    #endregion
}
