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

    public Transform weaponPos;

    public Stats stats;

    public Weapon currentWeapon;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;            // �������� Material

    Vector3 mousePos;

    Vector3 targetPos;

    bool isAttacking;   // ���� ������?

    //////// �̵�
    enum AnimState { idle, move, attack, damaged, die }     // �ִϸ��̼��� ����
    AnimState animState = AnimState.idle;           

    Vector3 direction;                  // �̵� ����

    //////// ���
    //bool isDash = false;
    //float dashSpeed = 100f;       // ��� �ӵ�
    //float defaultTime = 0.1f;
    //float dashTime;
    //float dashDefaultCoolTime = 1f;
    //float dashCoolTime;

    //////// ����
    [SerializeField]
    private LayerMask weaponLayer;

    private float detectRadius = 1.1f;

    Collider[] colliders;

    [HideInInspector]
    public Rigidbody rigid;
    Animator anim;
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
    }

    private void Start()
    {
        StartCoroutine(AutoAttack());
        StartCoroutine(Skill());
        StartCoroutine(Dash());
    }

    private void Update()
    {
        DetectWeapon();
    }

    void FixedUpdate()
    {
        Move();
        
        anim.SetInteger("animation", (int)animState);
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

                yield return new WaitForSeconds(0.01f);

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(0.2f);

                isAttacking = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ���⸦ ��� ���� �� ��Ŭ���ϸ� ��Ÿ
    /// </summary>
    IEnumerator Skill()
    {
        while (true)
        {
            if (currentWeapon && Input.GetMouseButtonDown(1))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return new WaitForSeconds(0.01f);

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(0.2f);

                isAttacking = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ���⸦ ��� ���� �� ��Ŭ���ϸ� ��Ÿ
    /// </summary>
    IEnumerator Dash()
    {
        while (true)
        {
            if (currentWeapon && Input.GetKeyDown(KeyCode.Space))
            {
                currentWeapon.SendMessage("Dash", SendMessageOptions.DontRequireReceiver);
            }

            yield return null;
        }
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    void InitStats()
    {
        stats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
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

    #region ������
    /// <summary>
    /// �������� ������
    /// </summary>
    void Move()
    {
        if (isAttacking) return;

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
            rigid.position += direction * stats.moveSpeed * Time.deltaTime;   // �̵�
        }
        else
        {
            animState = AnimState.idle;
        }

    }

    ///// <summary>
    ///// �����̽� �ٸ� ������ ���
    ///// </summary>
    //void Dash()
    //{
    //    if (currentWeapon) return;          // ���� ���� �߿��� ������ ���

    //    if (Input.GetKeyDown(KeyCode.Space) && dashCoolTime <= 0)
    //    {
    //        isDash = true;
    //    }

    //    if (dashTime <= 0)
    //    {
    //        defaultTime = 0.1f;

    //        if (isDash)
    //        {
    //            animState = AnimState.idle;

    //            rigid.position += Vector3.forward * dashSpeed * Time.deltaTime;

    //            dashTime = defaultTime;
    //            dashCoolTime = dashDefaultCoolTime;
    //        }
    //    }
    //    else
    //    {
    //        dashTime -= Time.deltaTime;
    //        defaultTime = dashTime;
    //    }

    //    if (dashCoolTime > 0f)
    //    {
    //        dashCoolTime -= Time.deltaTime;
    //    }

    //    isDash = false;
    //}
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

        weapon.transform.parent = weaponPos;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localEulerAngles = Vector3.zero;
        stats = weapon.stats;

        ChangeMaterial();
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

    #endregion
}