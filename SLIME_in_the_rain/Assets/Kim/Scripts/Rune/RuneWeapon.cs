using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneWeapon : Rune
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
    public override void Use()
    {
        UseWeaponRune();

        throw new System.NotImplementedException();
    }

    protected virtual bool UseWeaponRune()
    {
        // ���� ���Ⱑ type1 Ȥ�� type2 �� ���ٸ� ����� �� �ֵ���
        EWeaponType currentWeapon = slime.currentWeapon.weaponType;
        if (currentWeapon.Equals(weaponType1) || currentWeapon.Equals(weaponType2))
        {
            return true;
        }
        else return false;
    }
    #endregion

}
