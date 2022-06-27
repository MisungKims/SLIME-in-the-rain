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


    // 대시
    protected float dashCoolTime;
    protected bool isDash = false;


    // 캐싱
    private WaitForSeconds waitForDash;
    #endregion

    #region 유니티 함수
    void Start()
    {
        slime = Slime.Instance;

        waitForDash = new WaitForSeconds(dashCoolTime);


    }

    #endregion

    #region 코루틴
    // 무기 장착 코루틴
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

    // 대시 쿨타임 코루틴
    protected IEnumerator DashTimeCount()
    {
        isDash = true;

        yield return waitForDash;

        isDash = false;
    }
    #endregion

    #region 함수
    public abstract void AutoAttack(Vector3 targetPos);
    public abstract void Skill(Vector3 targetPos);
    public abstract void Dash(Slime slime);


    // 무기 장착 코루틴을 실행
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
