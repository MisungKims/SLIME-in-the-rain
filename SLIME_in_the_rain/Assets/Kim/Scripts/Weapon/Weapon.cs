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

public class Weapon : MonoBehaviour
{
    #region 변수
    public Stats stats;         // 무기의 스탯

    public List<WeaponRuneInfo> weaponRuneInfos = new List<WeaponRuneInfo>();           // 무기의 룬 정보

    protected Slime slime;

    public Material slimeMat;       // 바뀔 슬라임의 Material

    public EWeaponType weaponType;

    protected Vector3 angle = Vector3.zero;

    float attachSpeed = 10f;


    //무기UI Text 변수
    public string wName = "무기없음";
    public string wColor = "기본색";
    public string wSkill = "스킬없음";

    // 애니메이션
    [SerializeField]
    private Animator anim;
    protected enum AnimState { idle, autoAttack, skill }     // 애니메이션의 상태
    protected AnimState animState = AnimState.idle;

    // 대시
    protected float dashCoolTime;
    protected bool isDash = false;

    // 스킬
    public bool isCanSkill = true;
    private float currentCoolTime;
    public int CurrentCoolTime { get { return (int)currentCoolTime; } }

    // 캐싱
    private WaitForSeconds waitForDash;
    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);

    protected StatManager statManager;
    #endregion

    #region 유니티 함수
    protected virtual void Start()
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
        UseRune();
        ///////////////여기에 추가(무기 바꾼거)
    }

    // 대시 쿨타임 코루틴
    protected IEnumerator DashTimeCount()
    {
        isDash = true;

        yield return waitForDash;

        isDash = false;
    }

    // 스킬 쿨타임 코루틴
    protected IEnumerator SkillTimeCount()
    {
        isCanSkill = false;

        currentCoolTime = (int)slime.Stat.coolTime;
        while (currentCoolTime > 0f)
        {
            currentCoolTime -= Time.deltaTime;

            yield return null;
        }

        isCanSkill = true;
    }

    // 애니메이션이 종료되었는지 확인 후 Idle로 상태 변경
    public IEnumerator CheckAnimEnd(string state)
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

    #region 함수
    // 무기 UI 할당한 정보 넣어주기 
    protected void UIseting(string n, string c, string s) 
    {
        this.wName = n;
        this.wColor = c;
        this.wSkill = s;
    }

    // 평타
    protected virtual void AutoAttack(Vector3 targetPos)
    {
        PlayAnim(AnimState.autoAttack);

        StartCoroutine(CheckAnimEnd("AutoAttack"));
    }

    // 스킬
    protected virtual void Skill(Vector3 targetPos)
    {
        RuneManager.Instance.UseSkillRune();

        PlayAnim(AnimState.skill);

        StartCoroutine(CheckAnimEnd("Skill"));

        StartCoroutine(SkillTimeCount());
    }

    // 대시
    public virtual bool Dash(Slime slime)
    {
        if (isDash)             // 대시 쿨타임이 지나지 않았으면 false 반환
        {
            slime.isDash = false;
            return false;
        }
        else
        {
            RuneManager.Instance.UseDashRune();
            StartCoroutine(DashTimeCount());        // 대시 쿨타임 카운트
            return true;
        }
    }

    // 무기 장착 코루틴을 실행
    public void DoAttach()
    {
        StartCoroutine(AttachToSlime());
    }

    // 룬 사용
    public void UseRune()
    {
        // 무기 룬을 발동시킬 수 있는지 판별 후 발동
        RuneManager.Instance.IsHaveWeaponRune(this);
    }

    // 애니메이션 재생
    protected void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }
    #endregion
}
