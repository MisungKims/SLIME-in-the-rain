/**
 * @brief 활 스크립트
 * @author 김미성
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    #region 변수
    Vector3 mouseWorldPosition;

    Vector3 mousePos;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
    }

    #endregion

    #region 코루틴
    IEnumerator Fire()
    {
        Vector3 rot = GetMousePosRot();

        //Slime.Instance.rigid.rotation = Quaternion.Euler(rot);
        //Slime.Instance.transform.LookAt(mousePos);

       yield return new WaitForSeconds(0.1f);

        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, rot).GetComponent<Arrow>().targetPos = mousePos;
    }
    #endregion


    #region 함수

    /// <summary>
    /// 평타
    /// </summary>
    public override void AutoAttack()
    {
        Debug.Log("AutoAttack");

        DetectObject();
        //StartCoroutine(Fire());
        

        

    }

    /// <summary>
    /// 스킬
    /// </summary>
    public override void Skill()
    {
        Debug.Log("Skill");
    }

    /// <summary>
    /// 대시
    /// </summary>
    public override void Dash()
    {
        Debug.Log("Dash");
    }

    void DetectObject()
    {
        mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Land"))
            {
                //Debug.Log(hit.transform.gameObject);
                Debug.Log(mousePos);

                Vector3 target = Vector3.zero;
                target.y = mousePos.y;

                //Slime.Instance.rigid.rotation = Quaternion.Euler(GetMousePosRot());
                Slime.Instance.transform.LookAt(target);
                //StartCoroutine(Fire());

            }
        }
    }

    Vector3 GetMousePosRot()
    {
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos + Vector3.forward * 10f);

        // Atan2를 이용하면 높이와 밑변(tan)으로 라디안(Radian)을 구할 수 있음
        // Mathf.Rad2Deg를 곱해서 라디안(Radian)값을 도수법(Degree)으로 변환
        float angle = Mathf.Atan2(
            this.transform.position.y - mouseWorldPosition.y,
            this.transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;

        // angle이 0~180의 각도라서 보정
        float final = -(angle + 90f);
        // 로그를 통해서 값 확인
        //Debug.Log(angle + " / " + final);

        return new Vector3(0f, final, 0f);
    }

    #endregion
}
