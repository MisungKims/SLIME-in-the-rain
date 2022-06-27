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
    Vector3 lookRot;


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
    }

    /// <summary>
    /// 스킬
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        // 부채꼴로 화살을 발사

        float angle = 45;           // 각도
        float interval = 10f;       // 간격

        for (float y = 180 - angle; y <= 180 + angle; y += interval)
        {
            GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow);

            arrow.transform.position = this.transform.position;

            arrow.transform.LookAt(targetPos);                  // 마우스 클릭 위치로 바라보게 한 다음  

            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.y += y + 180;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;     // 각도를 조절해 부채꼴처럼 보이도록 함
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
