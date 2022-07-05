/**
 * @brief ����
 * @author ��̼�
 * @date 22-07-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

public enum EMonsterAnim
{
    idle,
    walk,
    run,
    attack1,
    attack2,
    hit,
    stun,
    die
}

public class Monster : MonoBehaviour, IDamage
{
    #region ����
    private Animator anim;

    [SerializeField]
    private Stats stats;

    // ������ ����
    private float angleRange = 90f;
    Vector3 direction;
    float dotValue = 0f;
    private LayerMask slimeLayer = 9;
    private Transform target;

    // ����
    private bool isAttacking = false;
    int randAttack;

    private bool isStun = false;

    // ĳ��
    StatManager statManager;
    #endregion

    #region ����Ƽ �Լ�

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        statManager = StatManager.Instance;

        PlayAnim(EMonsterAnim.idle);

        StartCoroutine(DetectSlime());
    }
    #endregion

    #region �ڷ�ƾ

    // ������ ���� �ڷ�ƾ
    IEnumerator DetectSlime()
    {
        while (true)
        {
            if (!isAttacking)           // ���� ���� �ƴ� ��
            {
                // �� �ȿ� ���� ������ �ݶ��̴��� ���Ͽ� ����
                Collider[] colliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);

                if (colliders.Length > 0)
                {
                    dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));             // ������ ���� �ڻ��ΰ�
                    direction = colliders[0].transform.position - transform.position;      // ���Ϳ��� �������� ���� ����

                    if (direction.magnitude < stats.attackRange)         // Ž���� ������Ʈ�� ��ä���� �߽����� �Ÿ��� �� 
                    {
                        // Ž���� ������Ʈ�� �����ȿ� �������� ���� ����
                        if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
                        {
                            target = colliders[0].transform;
                            Debug.Log("Detect slime");
                        }
                    }
                }

            }
            yield return null;
        }
    }

    // �ִϸ��̼��� ����Ǿ����� Ȯ�� �� Idle�� ���� ����
    public IEnumerator CheckAnimEnd(string state)
    {
        string name = "Base Layer." + state;
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(name) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                PlayAnim(EMonsterAnim.idle);
                break;
            }
            yield return null;
        }
    }

    // ���� �ڷ�ƾ
    IEnumerator DoStun(float time)
    {
        isStun = true;
        PlayAnim(EMonsterAnim.stun);

        yield return new WaitForSeconds(time);

        isStun = false;
        PlayAnim(EMonsterAnim.idle);
    }
    #endregion

    #region �Լ�
    #region ������
    // �������� ��Ÿ�� �������� ����
    public void AutoAtkDamaged()
    {
        PlayAnim(EMonsterAnim.hit);
        StartCoroutine(CheckAnimEnd("GetHit"));

        stats.HP -= statManager.GetAutoAtkDamage();

        Debug.Log(name + " ��Ÿ " + statManager.GetAutoAtkDamage());
    }

    // �������ǽ�ų�� �������� ����
    public void SkillDamaged()
    {
        PlayAnim(EMonsterAnim.hit);
        StartCoroutine(CheckAnimEnd("GetHit"));

        stats.HP -= statManager.GetSkillDamage();

        Debug.Log(name + " ��ų " + statManager.GetSkillDamage());
    }

    // ����
    public void Stun(float stunTime)
    {
        StartCoroutine(DoStun(stunTime));
    }
    #endregion

    // ����
    void Attack()
    {
        if (!target) return;

        randAttack = Random.Range((int)EMonsterAnim.attack1, (int)EMonsterAnim.attack2 + 1);
        PlayAnim((EMonsterAnim)randAttack);
        if (randAttack == (int)EMonsterAnim.attack1) StartCoroutine(CheckAnimEnd("Attack01"));
        else StartCoroutine(CheckAnimEnd("Attack02"));
    }

    // �ִϸ��̼� �÷���
    void PlayAnim(EMonsterAnim animState)
    {
        anim.SetInteger("animation", (int)animState);
    }
    #endregion

    // ����Ƽ �����Ϳ� ��ä���� �׷��� �޼ҵ�
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0f, 0f, 1f, 0.2f);
        // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, stats.attackRange);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, stats.attackRange);
    }
}
