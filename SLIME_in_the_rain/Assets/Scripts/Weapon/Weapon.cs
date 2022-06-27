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

public abstract class Weapon : MonoBehaviour
{
    #region ����
    public Stats stats;

    private Slime slime;

    public Material slimeMat;       // �ٲ� �������� Material

    public EWeaponType weaponType;

    protected Vector3 angle = Vector3.zero;

    Vector3 mouseWorldPosition;

    float attachSpeed = 10f;


    // ���
    protected float dashCoolTime;
    protected bool isDash = false;


    // ĳ��
    private WaitForSeconds waitForDash;
    #endregion

    #region ����Ƽ �Լ�
    void Start()
    {
        slime = Slime.Instance;

        waitForDash = new WaitForSeconds(dashCoolTime);


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
    }

    // ��� ��Ÿ�� �ڷ�ƾ
    protected IEnumerator DashTimeCount()
    {
        isDash = true;

        yield return waitForDash;

        isDash = false;
    }
    #endregion

    #region �Լ�
    public abstract void AutoAttack(Vector3 targetPos);
    public abstract void Skill(Vector3 targetPos);
    public abstract void Dash(Slime slime);


    // ���� ���� �ڷ�ƾ�� ����
    public void DoAttach()
    {
        StartCoroutine(AttachToSlime());
    }


    Slime SlimeInstance()
    {
        if (!slime)
        {
            slime = Slime.Instance;
        }

        return slime;
    }
    #endregion

}
