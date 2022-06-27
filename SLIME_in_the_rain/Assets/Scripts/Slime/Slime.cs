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


    //////// ����
    private Stats originStats;      // �⺻ �������� ����
    public Stats myStats;           // ���� �������� ����
    private Stats extraStats;       // ����ƾ, �� ������ �߰��� ��


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
    float dashDistance = 1.3f;          // ����� �Ÿ�
    public float dashTime = 1f;        // ��� ���� �ð�
    public float currentDashTime;
    public bool isDash { get; set; }                // ��� ������?
    bool isCanDash;     // ��� ��������?


    //////// ����
    Vector3 mousePos;

    Vector3 targetPos;

    bool isAttacking;   // ��Ÿ ������?


    //////// �̵�
    enum AnimState { idle, move, dash, damaged, die }     // �ִϸ��̼��� ����
    AnimState animState = AnimState.idle;

    Vector3 direction;                  // �̵� ����


    //////// ĳ��
    WaitForSeconds waitForRotate = new WaitForSeconds(0.01f);       // �������� ȸ���� ��ٸ���
    WaitForSeconds waitForAttack = new WaitForSeconds(0.2f);       // ������ ��ٸ���

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

        InitStats();

        isCanDash = true;
    }

    private void Start()
    {
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���⸦ ��� ���� �� ��Ŭ���ϸ� ��Ÿ
    /// </summary>
    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (currentWeapon && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01�� ���

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2�� ���

                isAttacking = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ���⸦ ��� ���� �� ��Ŭ���ϸ� ��ų
    /// </summary>
    IEnumerator Skill()
    {
        while (true)
        {
            if (currentWeapon && Input.GetMouseButtonDown(1))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01�� ���

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2�� ���

                isAttacking = false;

                yield return new WaitForSeconds(myStats.coolTime - 0.2f);
            }

            yield return null;
        }
    }

    IEnumerator DoDash()
    {
        isCanDash = false;

        PlayAnim(AnimState.dash);       // ��� �ִϸ��̼� ����

        currentDashTime = dashTime;
        while (currentDashTime >= 0)
        {
            transform.position += transform.forward * dashDistance * Time.deltaTime * 0.8f;

            currentDashTime -= Time.deltaTime;
        }

        isDash = false;

        yield return new WaitForSeconds(0.5f);

        isCanDash = true;
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    void InitStats()
    {
        originStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        myStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f);
    }

    /// <summary>
    /// �����Ӱ� ������Ʈ ������ �Ÿ��� ����
    /// </summary>
    /// <param name="target">�Ÿ��� ���� ������Ʈ</param>
    float GetDistance(Transform targetPos)
    {
        Vector3 offset = transform.position - targetPos.position;

        return offset.sqrMagnitude;
    }

    /// <summary>
    /// �ִϸ��̼� �÷���
    /// </summary>
    /// <param name="state"></param>
    void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }

    #region ������
    // �������� ������
    void Move()
    {
        if (isAttacking || isDash) return;

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
            rigid.position += direction * myStats.moveSpeed * Time.deltaTime;   // �̵�
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
        if (!isCanDash)
        {
            isDash = false;
            return;
        }

        StartCoroutine(DoDash());
    }

    
    #endregion

    #region ����

    void LookAtMousePos()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10f;    // ���콺�� ������ ������ ����

        if (!IsHitMonster())         // ���͸� Ŭ������ �ʾ��� ��
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);            // ���콺�� ��ġ�� �ٶ�
    }

    /// <summary>
    /// ���͸� Ŭ���ߴ���?
    /// </summary>
    /// <returns></returns>
    bool IsHitMonster()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Monster"))                // ���� Ŭ�� ��
            {
                targetPos = hit.transform.position;         // �������� �ٶ� ��ġ
                return true;
            }
        }

        return false;
    }
    #endregion

    #region ����
    /// <summary>
    /// �ֺ��� �ִ� ���� ����
    /// </summary>
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

    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="index"></param>
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

    
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        currentWeapon.transform.parent = weaponPos;
        currentWeapon.transform.localPosition = Vector3.zero;

        ChangeStats(currentWeapon);            // ������ ������ �������� ����

        ChangeMaterial();               // �������� �� ����
    }


    /// <summary>
    /// �������� ��(���͸���) ����
    /// </summary>
    void ChangeMaterial()
    {
        if (currentWeapon)
        {
            skinnedMesh.material = currentWeapon.slimeMat;
        }
    }

    #endregion

    #region ����
    /// <summary>
    /// ���� ���� �� �ش� ������ �������� ����
    /// </summary>
    void ChangeStats(Weapon weapon)
    {
        myStats.HP = weapon.stats.HP + extraStats.HP;
        myStats.coolTime = weapon.stats.coolTime + extraStats.coolTime;
        myStats.moveSpeed = weapon.stats.moveSpeed + extraStats.moveSpeed;
        myStats.attackSpeed = weapon.stats.attackSpeed + extraStats.attackSpeed;
        myStats.attackPower = weapon.stats.attackPower + extraStats.attackPower;
        myStats.defensePower = weapon.stats.defensePower + extraStats.defensePower;
    }

    /// <summary>
    /// HP ���� �߰�
    /// </summary>
    /// <param name="amount">�߰��� HP�� ��</param>
    public void AddHP(float amount)
    {
        extraStats.HP += amount;
        if (currentWeapon)
        {
            myStats.HP = currentWeapon.stats.HP + extraStats.HP;
        }
        else
        {
            myStats.HP = originStats.HP + extraStats.HP;
        }
    }

    public void AddCoolTime(float amount)
    {
        extraStats.coolTime += amount;
        if (currentWeapon)
        {
            myStats.coolTime = currentWeapon.stats.coolTime + extraStats.coolTime;
        }
        else
        {
            myStats.coolTime = originStats.coolTime + extraStats.coolTime;
        }
    }

    public void AddMoveSpeed(float amount)
    {
        extraStats.moveSpeed += amount;
        if (currentWeapon)
        {
            myStats.moveSpeed = currentWeapon.stats.moveSpeed + extraStats.moveSpeed;
        }
        else
        {
            myStats.moveSpeed = originStats.moveSpeed + extraStats.moveSpeed;
        }
    }

    public void AddAttackSpeed(float amount)
    {
        extraStats.attackSpeed += amount;
        if (currentWeapon)
        {
            myStats.attackSpeed = currentWeapon.stats.attackSpeed + extraStats.attackSpeed;
        }
        else
        {
            myStats.attackSpeed = originStats.attackSpeed + extraStats.attackSpeed;
        }
    }

    public void AddAttackPower(float amount)
    {
        extraStats.attackPower += amount;
        if (currentWeapon)
        {
            myStats.attackPower = currentWeapon.stats.attackPower + extraStats.attackPower;
        }
        else
        {
            myStats.attackPower = originStats.attackPower + extraStats.attackPower;
        }
    }

    public void AddDefensePower(float amount)
    {
        extraStats.defensePower += amount;
        if (currentWeapon)
        {
            myStats.defensePower = currentWeapon.stats.defensePower + extraStats.defensePower;
        }
        else
        {
            myStats.defensePower = originStats.defensePower + extraStats.defensePower;
        }
    }
    #endregion

    #endregion
}