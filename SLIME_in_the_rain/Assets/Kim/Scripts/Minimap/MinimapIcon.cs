using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinimapIcon : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI textUI;
    public RectTransform rectTransform;
    public RectTransform iconRectTransform;

    public void SetIcon(Sprite icon) { image.sprite = icon; }
    public void SetColor(Color color) { image.color = color; }
    public void SetText(string text) 
    {
        if (!string.IsNullOrEmpty(text))
        {
            textUI.enabled = true;
            textUI.text = text;
        }
    }

    public void SetTextSize(int size) { textUI.fontSize = size; }
}
