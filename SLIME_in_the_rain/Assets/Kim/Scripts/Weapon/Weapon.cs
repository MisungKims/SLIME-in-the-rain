/**
 * @brief ���� ������Ʈ
 * @author ��̼�
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
    #region ����
    public Stats stats;         // ������ ����

    public List<WeaponRuneInfo> weaponRuneInfos = new List<WeaponRuneInfo>();           // ������ �� ����

    protected Slime slime;

    public Material slimeMat;       // �ٲ� �������� Material

    public EWeaponType weaponType;

    protected Vector3 angle = Vector3.zero;

    float attachSpeed = 10f;


    //����UI Text ����
    public string wName = "�������";
    public string wColor = "�⺻��";
    public string wSkill = "��ų����";

    // �ִϸ��̼�
    [SerializeField]
    private Animator anim;
    protected enum AnimState { idle, autoAttack, skill }     // �ִϸ��̼��� ����
    protected AnimState animState = AnimState.idle;

    // ���
    protected float dashCoolTime;
    protected bool isDash = false;

    // ��ų
    public bool isCanSkill = true;
    private float currentCoolTime;
    public int CurrentCoolTime { get { return (int)currentCoolTime; } }

    // ĳ��
    private WaitForSeconds waitForDash;
    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);

    protected StatManager statManager;
    #endregion

    #region ����Ƽ �Լ�
    protected virtual void Start()
    {
        slime = Slime.Instance;
        statManager = StatManager.Instance;

        waitForDash = new WaitForSeconds(dashCoolTime);

        PlayAnim(AnimState.idle);
    }

    #endregion

    #region �ڷ�ƾ
    // ���� ���� �ڷ�ƾ
    IEnumerator AttachToSlime()
    {
        gameObject.layer = 7;       // ������ ����� �������� Ž������ ���ϵ��� ���̾� ����

        while (Vector3.Distance(transform.position, slime.weaponPos.position) >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, slime.weaponPos.position, Time.deltaTime * attachSpeed);

            yield return null;
        }

        slime.ChangeWeapon(this);
        transform.localEulerAngles = angle;
        UseRune();
        ///////////////���⿡ �߰�(���� �ٲ۰�)
    }

    // ��� ��Ÿ�� �ڷ�ƾ
    protected IEnumerator DashTimeCount()
    {
        isDash = true;

        yield return waitForDash;

        isDash = false;
    }

    // ��ų ��Ÿ�� �ڷ�ƾ
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

    // �ִϸ��̼��� ����Ǿ����� Ȯ�� �� Idle�� ���� ����
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

    #region �Լ�
    // ���� UI �Ҵ��� ���� �־��ֱ� 
    protected void UIseting(string n, string c, string s) 
    {
        this.wName = n;
        this.wColor = c;
        this.wSkill = s;
    }

    // ��Ÿ
    protected virtual void AutoAttack(Vector3 targetPos)
    {
        PlayAnim(AnimState.autoAttack);

        StartCoroutine(CheckAnimEnd("AutoAttack"));
    }

    // ��ų
    protected virtual void Skill(Vector3 targetPos)
    {
        RuneManager.Instance.UseSkillRune();

        PlayAnim(AnimState.skill);

        StartCoroutine(CheckAnimEnd("Skill"));

        StartCoroutine(SkillTimeCount());
    }

    // ���
    public virtual bool Dash(Slime slime)
    {
        if (isDash)             // ��� ��Ÿ���� ������ �ʾ����� false ��ȯ
        {
            slime.isDash = false;
            return false;
        }
        else
        {
            RuneManager.Instance.UseDashRune();
            StartCoroutine(DashTimeCount());        // ��� ��Ÿ�� ī��Ʈ
            return true;
        }
    }

    // ���� ���� �ڷ�ƾ�� ����
    public void DoAttach()
    {
        StartCoroutine(AttachToSlime());
    }

    // �� ���
    public void UseRune()
    {
        // ���� ���� �ߵ���ų �� �ִ��� �Ǻ� �� �ߵ�
        RuneManager.Instance.IsHaveWeaponRune(this);
    }

    // �ִϸ��̼� ���
    protected void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }
    #endregion
}
