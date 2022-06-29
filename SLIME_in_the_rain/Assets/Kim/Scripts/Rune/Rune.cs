/**
 * @brief ������ ������Ʈ
 * @author ��̼�
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Rune : MonoBehaviour
{
    #region ����
    // ���� �̸�
    [SerializeField]
    private string runeName;
    public string RuneName { get { return runeName; } }

    // ���� ����
    [SerializeField]
    private string runeDescription;
    public string RuneDescription { get { return runeDescription; } }

    // ���� �̹���
    [SerializeField]
    private Sprite runeSprite;
    public Sprite RuneSprite { get { return runeSprite; } }
    #endregion

    #region �Լ�
    public abstract void Use();     // ���� ���
    #endregion
}
