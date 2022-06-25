using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region 변수
    public Slime slime;

    public Material slimeMat;       // 바뀔 슬라임의 Material


    // 슬라임 감지에 필요한 변수
    float velocity;
    float acceleration = 0.2f;
    float distance;
    Vector3 dirWeaon;
    bool isDetect = true;

    float attachSpeed = 10f;
    #endregion

    #region 유니티 함수
    void Start()
    {
        StartCoroutine(DetectSlime());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AutoAttack();
        }
        else if (Input.GetMouseButtonDown(2))
        {
            Skill();
        }
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 슬라임 탐지 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectSlime()
    {
        // 슬라임과의 거리가 1일 때 G Key를 누르면 Slime에게 붙음
        while (isDetect)
        {
            dirWeaon = (slime.weaponPos.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);          // 한 프레임으로 가속도 계산

            distance = Vector3.Distance(transform.position, slime.transform.position);
            if (distance <= 1.0f)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    StartCoroutine(AttachToSlime());
                }
            }
            else
            {
                velocity = 0.0f;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 슬라임에게 붙음
    /// </summary>
    /// <returns></returns>
    IEnumerator AttachToSlime()
    {
        //Vector3 weaponPos = new Vector3(transform.position.x + (dirWeaon.x * velocity),
        //                        transform.position.y,
        //                        transform.position.z + (dirWeaon.z * velocity));

        

        while (Vector3.Distance(transform.position, slime.weaponPos.position) >= 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, slime.weaponPos.position, Time.deltaTime * attachSpeed);

            yield return null;
        }

        isDetect = false;
        slime.ChangeWeapon(this);
    }
    #endregion

    #region 함수
    /// <summary>
    /// 평타
    /// </summary>
    protected virtual void AutoAttack()
    {
        
    }

    /// <summary>
    /// 스킬
    /// </summary>
    protected virtual void Skill()
    {

    }
    #endregion

}
