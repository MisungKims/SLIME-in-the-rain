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
    Vector3 mouseWorldPosition;

    Vector3 mousePos;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        weaponType = EWeaponType.bow;
        angle = new Vector3(0f, -90f, 0f);
    }

    #endregion

    #region �ڷ�ƾ
    IEnumerator Fire()
    {
        Vector3 rot = GetMousePosRot();

        //Slime.Instance.rigid.rotation = Quaternion.Euler(rot);
        //Slime.Instance.transform.LookAt(mousePos);

       yield return new WaitForSeconds(0.1f);

        ObjectPoolingManager.Instance.Get(EObjectFlag.arrow, transform.position, rot).GetComponent<Arrow>().targetPos = mousePos;
    }
    #endregion


    #region �Լ�

    /// <summary>
    /// ��Ÿ
    /// </summary>
    public override void AutoAttack()
    {
        Debug.Log("AutoAttack");

        DetectObject();
        //StartCoroutine(Fire());
        

        

    }

    /// <summary>
    /// ��ų
    /// </summary>
    public override void Skill()
    {
        Debug.Log("Skill");
    }

    /// <summary>
    /// ���
    /// </summary>
    public override void Dash()
    {
        Debug.Log("Dash");
    }

    void DetectObject()
    {
        mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Land"))
            {
                //Debug.Log(hit.transform.gameObject);
                Debug.Log(mousePos);

                Vector3 target = Vector3.zero;
                target.y = mousePos.y;

                //Slime.Instance.rigid.rotation = Quaternion.Euler(GetMousePosRot());
                Slime.Instance.transform.LookAt(target);
                //StartCoroutine(Fire());

            }
        }
    }

    Vector3 GetMousePosRot()
    {
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos + Vector3.forward * 10f);

        // Atan2�� �̿��ϸ� ���̿� �غ�(tan)���� ����(Radian)�� ���� �� ����
        // Mathf.Rad2Deg�� ���ؼ� ����(Radian)���� ������(Degree)���� ��ȯ
        float angle = Mathf.Atan2(
            this.transform.position.y - mouseWorldPosition.y,
            this.transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;

        // angle�� 0~180�� ������ ����
        float final = -(angle + 90f);
        // �α׸� ���ؼ� �� Ȯ��
        //Debug.Log(angle + " / " + final);

        return new Vector3(0f, final, 0f);
    }

    #endregion
}
