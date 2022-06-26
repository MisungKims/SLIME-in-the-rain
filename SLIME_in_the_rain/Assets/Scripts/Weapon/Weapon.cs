/**
 * @brief 무기 오브젝트
 * @author 김미성
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    dagger,
    sword,
    iceStaff,
    fireStaff,
    bow
}

public abstract class Weapon : MonoBehaviour
{
    #region 변수
    public Stats stats;

    private Slime slime;

    public Material slimeMat;       // 바뀔 슬라임의 Material

    public EWeaponType weaponType;

    protected Vector3 angle = Vector3.zero;

    Vector3 mouseWorldPosition;

    float attachSpeed = 10f;
    #endregion

    #region 유니티 함수
    void Start()
    {
        slime = Slime.Instance;
    }

    void Update()
    {
        if (SlimeInstance().currentWeapon && slime.currentWeapon.Equals(this))
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 무기 장착 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttachToSlime()
    {
        gameObject.layer = 7;       // 장착된 무기는 슬라임이 탐지하지 못하도록 레이어 변경

        while (Vector3.Distance(transform.position, slime.weaponPos.position) >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, slime.weaponPos.position, Time.deltaTime * attachSpeed);

            yield return null;
        }

        slime.ChangeWeapon(this);
        transform.localEulerAngles = angle;
    }
    #endregion

    #region 함수
    public abstract void AutoAttack(Vector3 targetPos);
    public abstract void Skill(Vector3 targetPos);
    public abstract void Dash();


    /// <summary>
    /// 무기 장착 코루틴을 실행
    /// </summary>
    public void DoAttach()
    {
        StartCoroutine(AttachToSlime());
    }


    Slime SlimeInstance()
    {
        if (!slime)
        {
            slime = Slime.Instance;
        }

        return slime;
    }
    #endregion

}
