/**
 * @brief �� �Ŵ���
 * @author ��̼�
 * @date 22-06-29
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    #region ����
    [SerializeField]
    private List<Rune> runes = new List<Rune>();        // ��ü ���� ����Ʈ

    private Rune[] myRunes = new Rune[3];       // �� ��
    int runeCount = 0;

    int rand;
    #endregion

    #region �Լ�
    // �������� ���� ��ȯ
    public Rune GetRandomRune()
    {
        rand = Random.Range(0, runes.Count);

        return runes[rand];
    }

    // ���� �߰�
    public void AddMyRune(Rune rune)
    {
        if (runeCount > 2) return;

        myRunes[runeCount] = rune;
        runeCount++;
    }
    #endregion
}
