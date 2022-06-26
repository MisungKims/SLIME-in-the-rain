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

    #endregion

    #region 유니티 함수
    private void Awake()
    {
        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
    }

    #endregion

    #region 코루틴
    IEnumerator FireAnimation()
    {
        Vector3 targetPos1 = transform.localEulerAngles;
        targetPos1.z = -17f;

        float time = 0.3f;
        float speed = 1000f;

        while (time >= 0f)
        {
            transform.rotation *= Quaternion.Euler(0f, 0f, Time.deltaTime * speed);

            time -= Time.deltaTime;
            speed -= Time.deltaTime * 5f;

            yield return null;
        }

        transform.localEulerAngles = angle;
    }
    #endregion


    #region 함수

    /// <summary>
    /// 평타
    /// </summary>
    public override void AutoAttack(Vector3 targetPos)
    {
        // 화살 생성 뒤 마우스 방향을 바라봄
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, Vector3.zero).transform.LookAt(targetPos);

        //StartCoroutine(FireAnimation());
    }

    /// <summary>
    /// 스킬
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        // 부채꼴로 화살을 발사

        float angle = 45;
        float interval = 10f;

        if (interval > 0)
        {
            for (float y = 180 - angle; y <= 180 + angle; y += interval)
            {
                GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow);

                arrow.transform.position = this.transform.position;
                arrow.transform.eulerAngles = Vector3.up * y;
            }
        }
    }

    /// <summary>
    /// 대시
    /// </summary>
    public override void Dash()
    {
        Debug.Log("Dash");
    }

    #endregion
}
