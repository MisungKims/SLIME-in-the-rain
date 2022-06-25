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
    public Transform weaponPos;

    public Stats stats;

    public Weapon currentWeapon;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;

    // �̵�
    enum AnimState { idle, move, attack, damaged, die }     // �ִϸ��̼��� ����
    AnimState animState = AnimState.idle;           

    Vector3 direction;                  // �̵� ����

    // ���
    bool isDash = false;
    float dashSpeed = 100f;       // ��� �ӵ�
    float defaultTime = 0.1f;
    float dashTime;
    float dashDefaultCoolTime = 1f;
    float dashCoolTime;

    Rigidbody rigid;
    Animator anim;
    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        InitStats();
    }

    private void Update()
    {
        Dash();
    }

    void FixedUpdate()
    {
        Move();
        
        anim.SetInteger("animation", (int)animState);
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
    /// �������� ������
    /// </summary>
    void Move()
    {
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

    /// <summary>
    /// �����̽� �ٸ� ������ ���
    /// </summary>
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCoolTime <= 0)
        {
            isDash = true;
        }

        if (dashTime <= 0)
        {
            defaultTime = 0.1f;

            if (isDash)
            {
                animState = AnimState.idle;

                rigid.position += Vector3.forward * dashSpeed * Time.deltaTime;

                dashTime = defaultTime;
                dashCoolTime = dashDefaultCoolTime;
            }
        }
        else
        {
            dashTime -= Time.deltaTime;
            defaultTime = dashTime;
        }

        if (dashCoolTime > 0f)
        {
            dashCoolTime -= Time.deltaTime;
        }

        isDash = false;
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
    #endregion
}