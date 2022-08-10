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
    float equipTime;

    private Outline outline;

    //����UI Text ����
    public string wName = "�������";
    public string wColor = "�⺻��";
    public string wSkill = "��ų����";

    // �ִϸ��̼�
    [SerializeField]
    private Animator anim;
    protected enum AnimState { idle, autoAttack, skill }     // �ִϸ��̼��� ����
    protected AnimState animState = AnimState.idle;

    private Camera cam;
    private Vector3 hitPos;
    protected Vector3 targetPos;


    // ���
    public float dashCoolTime;
    public float maxDashCoolTime;
    protected bool isDash = false;

    // ��ų
    public bool isCanSkill = true;
    private float currentCoolTime;
    public float CurrentCoolTime { get { return currentCoolTime; } }

    // ĳ��
    private WaitForSeconds waitForDash;
    private WaitForSeconds waitForRotate = new WaitForSeconds(0.01f);       // �������� ȸ���� ��ٸ���

    protected StatManager statManager;
    #endregion

    #region ����Ƽ �Լ�
    protected virtual void Awake()
    {
        slime = Slime.Instance;
        statManager = StatManager.Instance;
        cam = Camera.main;
        outline = GetComponent<Outline>();

        waitForDash = new WaitForSeconds(dashCoolTime);
    }

    private void OnEnable()
    {
        dashCoolTime = 0f;
        currentCoolTime = 0f;
    }

    protected virtual void Start()
    {
        PlayAnim(AnimState.idle);
    }

    #endregion

    #region �ڷ�ƾ
    // ���� ���� �ڷ�ƾ
    IEnumerator AttachToSlime()
    {
        outline.enabled = false;
        gameObject.layer = 7;       // ������ ����� �������� Ž������ ���ϵ��� ���̾� ����

        equipTime = 0.5f;
        while (Vector3.Distance(transform.position, slime.weaponPos.position) >= 0.1f && equipTime > 0f)
        {
            transform.position = Vector3.Lerp(transform.position, slime.weaponPos.position, Time.deltaTime * attachSpeed);
            equipTime -= Time.deltaTime;

            yield return null;
        }

        ChangeWeapon();
    }

    public void ChangeWeapon()
    {
        slime.ChangeWeapon(this);
        transform.localEulerAngles = angle;
        UseRune();
    }

    // ��� ��Ÿ�� �ڷ�ƾ
    protected IEnumerator DashTimeCount()
    {
        isDash = true;

        yield return waitForDash;

        dashCoolTime = maxDashCoolTime;
        while (dashCoolTime > 0f)
        {
            dashCoolTime -= Time.deltaTime;

            yield return null;
        }

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
        LookAtMousePos();

        yield return waitForRotate;

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

    // ���콺 Ŭ�� ��ġ�� �ٶ�
    void LookAtMousePos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitResult;
        if (Physics.Raycast(ray, out hitResult))
        {
            hitPos = hitResult.point;
            hitPos.y = transform.position.y;

           targetPos = hitPos - transform.position;

            slime.transform.forward = targetPos;

            if (hitResult.transform.gameObject.layer == 8)
            {
                slime.target = hitResult.transform;        // ���� ���̾�� target�� ���� (���� ���� ����)
            }
        }
    }

    // ��Ÿ
    protected virtual void AutoAttack()
    {
        PlayAnim(AnimState.autoAttack);

        StartCoroutine(CheckAnimEnd("AutoAttack"));
    }

    // ��ų
    protected virtual void Skill()
    {
        PlayAnim(AnimState.skill);

        RuneManager.Instance.UseSkillRune();

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
