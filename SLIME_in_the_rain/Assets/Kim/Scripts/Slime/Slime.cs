/**
 * @brief ������ ������Ʈ
 * @author ��̼�
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    #region ����
    #region �̱���
    private static Slime instance = null;
    public static Slime Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private Rigidbody rigid;

    private Animator anim;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;            // �������� Material
    public SkinnedMeshRenderer SkinnedMesh { get { return skinnedMesh; } }

    private Stats stat;
    public Stats Stat { get { return stat; } }

    //////// ����
    [Header("------------ ����")]
    public Transform weaponPos;     // ���� ���� �� ������ parent

    public Weapon currentWeapon;    // ���� ���� ����

    [SerializeField]
    private LayerMask weaponLayer;

    private float detectRadius = 1.1f;      // ���⸦ ������ ����

    Collider[] colliders;


    //////// ���
    [Header("------------ ���")]
    private float originDashDistance = 1.4f;          // ����� �Ÿ�
    private float dashDistance = 1.4f;
    public float DashDistance { set { dashDistance = value; } }
    public float dashTime = 1f;        // ��� ���� �ð�
    public float currentDashTime;
    public bool isDash { get; set; }                // ��� ������?
    bool isCanDash;     // ��� ��������?


    //////// ����
    Vector3 mousePos;

    Vector3 targetPos;
    public Transform target;

    public bool isAttacking;   // ��Ÿ ������?

    public bool isStealth;      // ���� ������?

    // ������
    private bool isStun;


    //////// �̵�
    enum AnimState { idle, move, dash, damaged, die }     // �ִϸ��̼��� ����
    AnimState animState = AnimState.idle;

    Vector3 direction;                  // �̵� ����


    //////// ĳ��
    private WaitForSeconds waitForRotate = new WaitForSeconds(0.01f);       // �������� ȸ���� ��ٸ���
    private WaitForSeconds waitForAttack = new WaitForSeconds(0.2f);       // ������ ��ٸ���

    [SerializeField]
    public StatManager statManager;

    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        isCanDash = true;
    }

    private void Start()
    {
        stat = statManager.myStats;
        StartCoroutine(AutoAttack());
        StartCoroutine(Skill());
    }

    private void Update()
    {
        DetectWeapon();

        SpaceBar();
    }

    void FixedUpdate()
    {
        Move();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectRadius);
    //}
    #endregion

    #region �ڷ�ƾ
    // ���⸦ ��� ���� �� ��Ŭ���ϸ� ��Ÿ
    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (!isAttacking && currentWeapon && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01�� ���

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(stat.attackSpeed);           // �� ������ ���� ���ȿ� ���� ���

                isAttacking = false;
            }

            yield return null;
        }
    }

    // ���⸦ ��� ���� �� ��Ŭ���ϸ� ��ų
    IEnumerator Skill()
    {
        while (true)
        {
            if (IsCanSkill())
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01�� ���

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2�� ���

                isAttacking = false;
            }

            yield return null;
        }
    }

    // ��� �ڷ�ƾ
    IEnumerator DoDash()
    {
        isCanDash = false;

        PlayAnim(AnimState.dash);       // ��� �ִϸ��̼� ����

        currentDashTime = dashTime;
        while (currentDashTime >= 0)
        {
            transform.position += transform.forward * dashDistance * Time.deltaTime;

            currentDashTime -= Time.deltaTime;
        }

        isDash = false;

        yield return new WaitForSeconds(0.5f);

        dashDistance = originDashDistance;
        isCanDash = true;
    }

    // ���� �ڷ�ƾ
    IEnumerator DoStun(float stunTime)
    {
        isStun = true;
        PlayAnim(AnimState.damaged);

        yield return new WaitForSeconds(stunTime);

        isStun = false;
    }
    #endregion

    #region �Լ�
    // �����Ӱ� ������Ʈ ������ �Ÿ��� ����
    float GetDistance(Transform target)
    {
        Vector3 offset = transform.position - target.position;

        return offset.sqrMagnitude;
    }

    // �ִϸ��̼� �÷���
    void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }

    #region ������
    // �������� ������
    void Move()
    {
        if (isAttacking || isDash || isStun) return;

        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");

        if (dirX != 0 || dirZ != 0)
        {
            animState = AnimState.move;

            direction = new Vector3(dirX, 0, dirZ);

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                rigid.rotation = Quaternion.Euler(0, angle, 0);         // ȸ��
            }

            transform.position += direction * stat.moveSpeed * Time.deltaTime;   // �̵�
        }
        else
        {
            animState = AnimState.idle;
        }

        PlayAnim(animState);
    }


    // �����̽��� ������ ������ ���
    void SpaceBar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDash)
        {
            isDash = true;

            if (currentWeapon)
            {
                currentWeapon.SendMessage("Dash", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Dash();
            }
        }
    }

    // ���
    public void Dash()
    {
        // ��ø� �� �� ���� �� return
        if (!isCanDash || isStun)
        {
            isDash = false;
            return;
        }

        StartCoroutine(DoDash());
    }

    
    #endregion

    #region ����
    // ��ų�� ����� �� �ִ���?
    bool IsCanSkill()
    {
        if (!isAttacking && currentWeapon && currentWeapon.isCanSkill && Input.GetMouseButtonDown(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ���콺�� Ŭ���� ��ġ�� �ٶ�
    void LookAtMousePos()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10f;    // ���콺�� ������ ������ ����

        // ������Ʈ�� Ŭ������ �ʾ��� ���� ���콺 ��ġ�� �ٶ󺸰�,
        // ������Ʈ�� Ŭ������ ���� ������Ʈ ��ġ�� �ٶ�
        if (!IsHitObject())        
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);            // ���콺�� ��ġ�� �ٶ�
    }

    // ������Ʈ�� Ŭ���ߴ���?
    bool IsHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("DamagedObject") || hit.transform.CompareTag("Land"))
            {
                targetPos = hit.transform.position;         // �������� �ٶ� ��ġ

                if (hit.transform.gameObject.layer == 8)        // ���� ���̾�� target�� ���� (������ ����)
                {
                    target = hit.transform;
                }

                return true;
            }
        }

        return false;
    }

    #endregion

    #region ����
    // �ֺ��� �ִ� ���� ����
    void DetectWeapon()
    {
        colliders = Physics.OverlapSphere(transform.position, detectRadius, weaponLayer);

        if (colliders.Length == 1)      // ������ ���Ⱑ �� ���� ��
        {
            EquipWeapon(0);
        }
        else if (colliders.Length > 1)
        {
            // ������ ����� �� ���� ����� �Ÿ��� �ִ� ���⸦ ����

            int minIndex = 0;
            float minDis = GetDistance(colliders[0].transform);

            for (int i = 1; i < colliders.Length; i++)          // ����� �Ÿ��� �ִ� ���� ã��
            {
                float distance = GetDistance(colliders[i].transform);

                if (minDis > distance)
                {
                    minDis = distance;
                    minIndex = i;
                }
            }

            EquipWeapon(minIndex);
        }
    }

    // ������ ���� ����
    void EquipWeapon(int index)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentWeapon)
            {
                currentWeapon.gameObject.layer = 6;
                ObjectPoolingManager.Instance.Set(currentWeapon);
                currentWeapon = null;
            }

            colliders[index].SendMessage("DoAttach", SendMessageOptions.DontRequireReceiver);
        }
    }

    // ���� ����
    public void ChangeWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        currentWeapon.transform.parent = weaponPos;
        currentWeapon.transform.localPosition = Vector3.zero;

        statManager.ChangeStats(currentWeapon);            // ������ ������ �������� ����

        ChangeMaterial();               // �������� �� ����
    }

    // �������� ��(���͸���) ����
    void ChangeMaterial()
    {
        if (currentWeapon)
        {
            skinnedMesh.material = currentWeapon.slimeMat;
        }
    }

    #endregion

    //// �������� ����
    //public void Damaged(float amount)
    //{
    //    // ����� = ���� ���ݷ� * (1 - �����)
    //    // ����� = ���� / (1 + ����)

    //    float damageReduction = stat.defensePower / (1 + stat.defensePower);
    //    stat.HP -= amount * (1 - damageReduction);

    //    PlayAnim(AnimState.damaged);
    //}


    //// �������� ��Ÿ�� �������� ����
    //public void AutoAtkDamaged()
    //{
    //    PlayAnim(AnimState.damaged);
    //    Debug.Log("AutoAtkDamaged");
    //    // ����� = ���� ���ݷ� * (1 - �����)
    //    // ����� = ���� / (1 + ����)

    //    //float damageReduction = stat.defensePower / (1 + stat.defensePower);
    //    //stat.HP -= amount * (1 - damageReduction);

    //    //
    //}

    //// �������ǽ�ų�� �������� ����
    //public void SkillDamaged()
    //{
    //    PlayAnim(AnimState.damaged);
    //    Debug.Log("SkillDamaged");
    //}

    public void Damaged(Stats monsterStats, int atkType)
    {
        PlayAnim(AnimState.damaged);
    }

    // ����
    public void Stun(float stunTime)
    {
        StartCoroutine(DoStun(stunTime));

        Debug.Log("Stun");
    }

   
#endregion
}