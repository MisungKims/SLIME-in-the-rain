/**
 * @brief Ȱ ��ũ��Ʈ
 * @author ��̼�
 * @date 22-06-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    #region ����
    Vector3 lookRot;


    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
    }

    #endregion

    #region �ڷ�ƾ
    IEnumerator FireAnimation()
    {
        Vector3 targetPos1 = transform.localEulerAngles;
        targetPos1.z = -17f;

        float time = 0.3f;
        float speed = 1000f;

        while (time >= 0f)
        {
            transform.rotation *= Quaternion.Euler(0f, 0f, Time.deltaTime * speed);

            time -= Time.deltaTime;
            speed -= Time.deltaTime * 5f;

            yield return null;
        }

        transform.localEulerAngles = angle;
    }

    #endregion


    #region �Լ�

    /// <summary>
    /// ��Ÿ
    /// </summary>
    public override void AutoAttack(Vector3 targetPos)
    {
        // ȭ�� ���� �� ���콺 ������ �ٶ�
        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, Vector3.zero).transform.LookAt(targetPos);
    }

    /// <summary>
    /// ��ų
    /// </summary>
    public override void Skill(Vector3 targetPos)
    {
        // ��ä�÷� ȭ���� �߻�

        float angle = 45;           // ����
        float interval = 10f;       // ����

        for (float y = 180 - angle; y <= 180 + angle; y += interval)
        {
            GameObject arrow = ObjectPoolingManager.Instance.Get(EObjectFlag.arrow);

            arrow.transform.position = this.transform.position;

            arrow.transform.LookAt(targetPos);                  // ���콺 Ŭ�� ��ġ�� �ٶ󺸰� �� ����  

            lookRot = arrow.transform.eulerAngles;
            lookRot.x = 0;
            lookRot.y += y + 180;
            lookRot.z = 0;

            arrow.transform.eulerAngles = lookRot;     // ������ ������ ��ä��ó�� ���̵��� ��
        }
    }

    /// <summary>
    /// ���
    /// </summary>
    public override void Dash()
    {
        Debug.Log("Dash");
    }

    #endregion
}
