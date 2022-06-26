using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    #region 변수
    Rigidbody rigid;
    public Vector3 targetPos;

    // 캐싱
    WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    WaitForSeconds waitFor5s = new WaitForSeconds(5f);
    #endregion

    #region 유니티 함수

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(TimeOut());
    }

    private void Update()
    {
        //transform.Translate(targetPos * 10f * Time.deltaTime);
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }
    #endregion

    #region 코루틴
    IEnumerator TimeOut()
    {
        yield return waitFor5s;

        ObjectPoolingManager.Instance.Set(this.gameObject, EObjectFlag.arrow);
    }

    #endregion

    #region 함수

    #endregion

}
