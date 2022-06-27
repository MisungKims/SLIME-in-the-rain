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

    // 애니메이션
    [SerializeField]
    private Animator anim;
    protected enum AnimState { idle, autoAttack, skill }     // 애니메이션의 상태
    protected AnimState animState = AnimState.idle;

    // 대시
    protected float dashCoolTime;
    protected bool isDash = false;


    // 캐싱
    private WaitForSeconds waitForDash;

    protected StatManager statManager;
    #endregion

    #region 유니티 함수
    void Start()
    {
        slime = Slime.Instance;
        statManager = StatManager.Instance;

        waitForDash = new WaitForSeconds(dashCoolTime);

        PlayAnim(AnimState.idle);
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
    protected virtual void AutoAttack(Vector3 targetPos)
    {
        PlayAnim(AnimState.autoAttack);
        StartCoroutine(CheckAnimEnd("AutoAttack"));
    }

    public abstract void Skill(Vector3 targetPos);
    public abstract void Dash(Slime slime);


    // 무기 장착 코루틴을 실행
    public void DoAttach()
    {
        StartCoroutine(AttachToSlime());
    }

    // 애니메이션 재생
    protected void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }

    // 애니메이션이 종료되었는지 확인 후 Idle로 상태 변경
    IEnumerator CheckAnimEnd(string state)
    {
        string name = "Base Layer." + state;
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(name) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
            yield return null;
        }

        PlayAnim(AnimState.idle);
    }
    #endregion

}
