/**
 * @brief ��ȭ ���� �ڽ�
 * @author ��̼�
 * @date 22-07-20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour, IDamage
{
    #region ����

    public bool isDamaged;

    private ObjectPoolingManager objectPoolingManager;
    private Vector3 spawnPos;

    private int randObj;

    private Animator anim;

    private GameObject pickUpObj;

    [SerializeField]
    private GameObject destroyBox;

    // �̴ϸ�
    [SerializeField]
    private MinimapWorldObject minimapObj;
    #endregion

    #region ����Ƽ �Լ�

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        objectPoolingManager = ObjectPoolingManager.Instance;
    }

    private void OnEnable()
    {
        isDamaged = false;
    }
    #endregion

    IEnumerator TakeDamaged()
    {
        anim.SetBool("TakeDamaged", true);

        yield return new WaitForSeconds(1f);

        destroyBox.SetActive(false);

        while (pickUpObj.activeSelf)
        {
            yield return null;
        }

        Minimap.Instance.RemoveMinimapIcon(minimapObj);     // �̴ϸʿ��� ����

        objectPoolingManager.Set(this.gameObject, EObjectFlag.box);
    }


    #region �Լ�

    // ���� Ȥ�� ����ƾ ����
    void SpawnObject()
    {
        if (isDamaged) return;

        isDamaged = true;

        // TODO : ����ġ ����
        randObj = Random.Range(0, 3);       // �������� ����, ����ƾ, ���⸦ ����

        spawnPos = this.transform.position;
        spawnPos.y += 0.5f;
        
        switch (randObj)
        {
            case 0:     // ����
                pickUpObj = objectPoolingManager.Get(EObjectFlag.jelly, spawnPos);
                break;
            case 1:     // ����ƾ
                pickUpObj = objectPoolingManager.Get(EObjectFlag.gelatin, spawnPos);
                break;
            case 2:     // ����
                pickUpObj = objectPoolingManager.Get(EObjectFlag.weapon, spawnPos);
                break;
            default:
                break;
        }

        StartCoroutine(TakeDamaged());
    }

    public void AutoAtkDamaged()
    {
        SpawnObject();
    }
    
    public void SkillDamaged()
    {
        SpawnObject();
    }

    public void Stun(float stunTime)
    {
        SpawnObject();
    }
    #endregion

}
