/**
 * @brief 발사체 오브젝트
 * @author 김미성
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region 변수
    // 캐싱
    WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    WaitForSeconds waitFor2s = new WaitForSeconds(2f);
    #endregion

    #region 유니티 함수
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

    #region 코루틴
    /// <summary>
    /// 3초 후에 없어짐
    /// </summary>
    /// <returns></returns>
    IEnumerator Remove()
    {
        yield return waitFor2s;

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.arrow);
    }

    #endregion

    #region 함수
    // 데미지를 입힘
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
