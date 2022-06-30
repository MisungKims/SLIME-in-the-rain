using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneWeapon : Rune, IWeaponRune
{
    #region ����
    protected EWeaponType weaponType1;
    protected EWeaponType weaponType2;

    private Slime slime;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        slime = Slime.Instance;
    }
    #endregion


    #region �Լ�
    public virtual bool Use(Weapon weapon)
    {
        if (UseWeaponRune(weapon))          // ���� �ߵ��� �� �ִ��� �Ǻ�
        {
            weapon.IsHaveRune = true;       // ���Ⱑ ���� Ư���� ����� �� �ֵ���
            return true;
        }
        else return false;
    }

    // �� ���� �ߵ��ϴ� ��������?
    protected bool UseWeaponRune(Weapon weapon)
    {
        // ���Ⱑ type1 Ȥ�� type2 �� ���ٸ� ����� �� �ֵ���
        EWeaponType currentWeapon = weapon.weaponType;
        if (currentWeapon.Equals(weaponType1) || currentWeapon.Equals(weaponType2))
        {
            Use(slime.currentWeapon);
            return true;
        }
        else return false;
    }
    #endregion

}
