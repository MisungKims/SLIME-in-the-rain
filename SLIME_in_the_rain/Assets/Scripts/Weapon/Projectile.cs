/**
 * @brief �߻�ü ������Ʈ
 * @author ��̼�
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region ����
    // ĳ��
    WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    WaitForSeconds waitFor2s = new WaitForSeconds(2f);
    #endregion

    #region ����Ƽ �Լ�
    private void OnEnable()
    {
        StartCoroutine(Remove());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamagedObject"))
        {
            DoDamage(other);
        }
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// 3�� �Ŀ� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator Remove()
    {
        yield return waitFor2s;

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.arrow);
    }

    #endregion

    #region �Լ�
    // �������� ����
    void DoDamage(Collider other)
    {
        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.arrow);

        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
        }
    }
    #endregion
}
