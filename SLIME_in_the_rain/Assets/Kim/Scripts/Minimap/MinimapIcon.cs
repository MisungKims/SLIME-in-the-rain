using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public Image image;
    public RectTransform rectTransform;
    public RectTransform iconRectTransform;

    public void SetIcon(Sprite icon) { image.sprite = icon; }
    public void SetColor(Color color) { image.color = color; }
}
