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

    float attachSpeed = 10f;
    #endregion

    #region ����Ƽ �Լ�
    void Start()
    {
        slime = Slime.Instance;
    }

    void Update()
    {
        if (SlimeInstance().currentWeapon && slime.currentWeapon.Equals(this))
        {
            if (Input.GetMouseButtonDown(0))
            {
                AutoAttack();
            }

            if (Input.GetMouseButtonDown(2))
            {
                Skill();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator AttachToSlime()
    {
        gameObject.layer = 7;       // ������ ����� �������� Ž������ ���ϵ��� ���̾� ����

        while (Vector3.Distance(transform.position, slime.weaponPos.position) >= 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, slime.weaponPos.position, Time.deltaTime * attachSpeed);

            yield return null;
        }

        slime.ChangeWeapon(this);
        transform.localEulerAngles = angle;
    }
    #endregion

    #region �Լ�
    public abstract void AutoAttack();
    public abstract void Skill();
    public abstract void Dash();


    /// <summary>
    /// ���� ���� �ڷ�ƾ�� ����
    /// </summary>
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
