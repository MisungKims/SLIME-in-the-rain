using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region ����
    public Slime slime;

    public Material slimeMat;       // �ٲ� �������� Material


    // ������ ������ �ʿ��� ����
    float velocity;
    float acceleration = 0.2f;
    float distance;
    Vector3 dirWeaon;
    bool isDetect = true;

    float attachSpeed = 10f;
    #endregion

    #region ����Ƽ �Լ�
    void Start()
    {
        StartCoroutine(DetectSlime());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AutoAttack();
        }
        else if (Input.GetMouseButtonDown(2))
        {
            Skill();
        }
    }
    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ������ Ž�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectSlime()
    {
        // �����Ӱ��� �Ÿ��� 1�� �� G Key�� ������ Slime���� ����
        while (isDetect)
        {
            dirWeaon = (slime.weaponPos.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);          // �� ���������� ���ӵ� ���

            distance = Vector3.Distance(transform.position, slime.transform.position);
            if (distance <= 1.0f)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    StartCoroutine(AttachToSlime());
                }
            }
            else
            {
                velocity = 0.0f;
            }

            yield return null;
        }
    }

    /// <summary>
    /// �����ӿ��� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator AttachToSlime()
    {
        //Vector3 weaponPos = new Vector3(transform.position.x + (dirWeaon.x * velocity),
        //                        transform.position.y,
        //                        transform.position.z + (dirWeaon.z * velocity));

        

        while (Vector3.Distance(transform.position, slime.weaponPos.position) >= 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, slime.weaponPos.position, Time.deltaTime * attachSpeed);

            yield return null;
        }

        isDetect = false;
        slime.ChangeWeapon(this);
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ��Ÿ
    /// </summary>
    protected virtual void AutoAttack()
    {
        
    }

    /// <summary>
    /// ��ų
    /// </summary>
    protected virtual void Skill()
    {

    }
    #endregion

}
