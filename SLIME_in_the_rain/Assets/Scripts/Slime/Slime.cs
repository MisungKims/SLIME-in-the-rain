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

    public bool isAttacking;   // ��Ÿ ������?


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
            if (!isAttacking && currentWeapon && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01�� ���

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                /// TODO : ��� �ð��� �� ������ ���ӿ� �°� ����
                //yield return new WaitForSeconds(statManager.myStats.attackSpeed);
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
            if (!isAttacking && currentWeapon && Input.GetMouseButtonDown(1))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01�� ���

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2�� ���

                isAttacking = false;

                yield return new WaitForSeconds(statManager.myStats.coolTime - 0.2f);
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
    /// �����Ӱ� ������Ʈ ������ �Ÿ��� ����
    /// </summary>
    /// <param name="target">�Ÿ��� ���� ������Ʈ</param>
    float GetDistance(Transform targetPos)
    {
        Vector3 offset = transform.position - targetPos.position;

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
            rigid.position += direction * statManager.myStats.moveSpeed * Time.deltaTime;   // �̵�
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

        statManager.ChangeStats(currentWeapon);            // ������ ������ �������� ����

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

    

    #endregion
}