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
    [Header("����UI")]//�ٲ� textMesh
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
    private Stats nowStats;
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
        inputStatsUI();
        if (slime.currentWeapon != null)
        {
            inputWeaponUI();
        }
        else
        {
            weaponTitleC.text = "�������";
            weaponColorC.text = "�⺻��";
            weaponSkillC.text = "��ų����";
        }
    }

    #endregion
    #region �޼ҵ�
    void inputWeaponUI() //���� ���ui
    {
        weaponTitleC.text = slime.currentWeapon.wName;
        weaponColorC.text = slime.currentWeapon.wColor;
        weaponSkillC.text = slime.currentWeapon.wSkill;
    }
    void inputStatsUI() //���� ���ui
    {
        statHPC.text = statManager.myStats.HP.ToString();
        statATKC.text = statManager.myStats.attackPower.ToString();
        statDEFC.text = statManager.myStats.defensePower.ToString();
        statATKSPDC.text = statManager.myStats.attackSpeed.ToString();
        statMOVESPDC.text = statManager.myStats.moveSpeed.ToString();
        statCOOLC.text = statManager.myStats.coolTime.ToString();
    }
    #endregion
}