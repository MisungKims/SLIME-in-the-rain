using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatsUIManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static StatsUIManager instance = null;
    public static StatsUIManager Instance
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
    //�ٲ� textMesh
    [Header("����UI")]
    public TextMeshProUGUI weaponTitleC;
    public TextMeshProUGUI weaponColorC;
    public TextMeshProUGUI weaponSkillC;
    [Header("����UI")]
    public TextMeshProUGUI statHPC;
    public TextMeshProUGUI statATKC;
    public TextMeshProUGUI statDEFC;
    public TextMeshProUGUI statATKSPDC;
    public TextMeshProUGUI statMOVESPDC;
    public TextMeshProUGUI statCOOLC;


    private Slime slime;
    private StatManager statManager;
  


    #endregion

    #region ����Ƽ �޼ҵ�
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
    }
    private void Start()
    {
        slime = Slime.Instance;
        statManager = StatManager.Instance;
    }
    private void Update()
    {
        if (slime.currentWeapon != null)
        {
        inputUI();

        }
    }

    #endregion
    void inputUI() //���� ���ui
    {
        
        weaponTitleC.text = slime.currentWeapon.wName;
        weaponColorC.text = slime.currentWeapon.wColor;
        weaponSkillC.text = slime.currentWeapon.wSkill;
    }

}
