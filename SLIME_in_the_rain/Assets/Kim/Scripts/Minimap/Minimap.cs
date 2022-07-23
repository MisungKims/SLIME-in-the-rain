using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    // https://www.youtube.com/watch?v=oIt9ZAQ_lU0

    #region ΩÃ±€≈Ê
    private static Minimap instance = null;
    public static Minimap Instance
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

    [SerializeField]
    Terrain terrain;

    [SerializeField]
    RectTransform scrollViewRectTransform;

    [SerializeField]
    RectTransform contentRectTransform;

    [SerializeField]
    MinimapIcon miniMapIconPrefab;

    Matrix4x4 transformationMatrix;

    Dictionary<MinimapWorldObject, MinimapIcon> miniMapWorldObjectLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();

    [SerializeField]
    private RectTransform range;

    public void Awake()
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
       // CalcuateTransformationMatrix();
    }

    private void Update()
    {
        UpdateMinimapIcons();
    }

    void UpdateMinimapIcons()
    {
        foreach (var kvp in miniMapWorldObjectLookup)
        {
            var minimapWorldObject = kvp.Key;
            var minimapIcon = kvp.Value;

            var mapPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
            minimapIcon.rectTransform.anchoredPosition = mapPosition;

            var rotation = minimapWorldObject.transform.rotation.eulerAngles * -1;
            minimapIcon.iconRectTransform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.forward);
        }
    }

    Vector2 WorldPositionTomapPostion(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z);
      //  return pos;
        return transformationMatrix.MultiplyPoint3x4(pos);
    }

    public void RegisterMinimapWorldObject(MinimapWorldObject minimapWorldObject)
    {
        var miniMapIcon = Instantiate(miniMapIconPrefab);
        miniMapIcon.transform.SetParent(contentRectTransform);
        miniMapIcon.SetIcon(minimapWorldObject.Icon);
        miniMapIcon.SetColor(minimapWorldObject.IconColor);
        miniMapIcon.SetText(minimapWorldObject.text);
        miniMapIcon.SetTextSize(minimapWorldObject.textSize);
        miniMapWorldObjectLookup[minimapWorldObject] = miniMapIcon;
    }

    void CalcuateTransformationMatrix()
    {
        var miniMapDimensions = contentRectTransform.rect.size;
        var terrainDimensions = range.sizeDelta;
        
        var scaleRatio = miniMapDimensions / terrainDimensions;
        var transition = -miniMapDimensions / 2;

        transformationMatrix = Matrix4x4.TRS(transition, Quaternion.identity, scaleRatio);
    }

}
