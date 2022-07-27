using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    #region ����
    #region �̱���
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

    private bool isZoomIn = true;

    [SerializeField]
    private GameObject slimeIconZoomIn;    // ��� ���¿��� �� �߰��� ���� �������� ������

    [SerializeField]
    private MinimapIcon miniMapIconPrefab;         // ������ ������ ������

    private Dictionary<MinimapWorldObject, MinimapIcon> miniMapWorldObjectLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();


    [SerializeField]
    private float mul = 8f;

   [SerializeField]
    private float zoom = 2.3f;     // ���� �� ����

    private MinimapWorldObject slimeObj;
    private Vector3 slimePos;

    [SerializeField]
    private Mask mask;
   [SerializeField]
    private RectTransform maskTransform;
    [SerializeField]
    private RectTransform mapTransform;
    [SerializeField]
    private RectTransform zoomInTransform;      // ���� ���� ���� Mask ũ��
    [SerializeField]
    private RectTransform zoomOutTransform;      // �ܾƿ� ���� ���� Mask ũ��


    // ĳ��
    private MinimapWorldObject minimapWorldObject;
    private MinimapIcon minimapIcon;
    private Vector2 iconPosition;

    private MinimapIcon newIcon;
    #endregion

    #region ����Ƽ �Լ�
    public void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        ZoomIn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isZoomIn) ZoomOut();
            else ZoomIn();
        }

        MoveMinimap();
        UpdateMinimapIcons();
    }
    #endregion

    #region �Լ�
    // ���
    void ZoomIn()
    {
        isZoomIn = true;
        slimeIconZoomIn.SetActive(true);
        mask.enabled = true;

        maskTransform.anchoredPosition = zoomInTransform.anchoredPosition;
        maskTransform.sizeDelta = zoomInTransform.sizeDelta;
        mapTransform.anchoredPosition = Vector2.zero;
    }

    // Ȯ��
    void ZoomOut()
    {
        isZoomIn = false;
        slimeIconZoomIn.SetActive(false);
        mask.enabled = false;

        maskTransform.anchoredPosition = zoomOutTransform.anchoredPosition;
        maskTransform.sizeDelta = zoomOutTransform.sizeDelta;
        mapTransform.anchoredPosition = Vector2.zero;
    }

    // ��� ������ �� �̴ϸ� ��ü�� ������
    void MoveMinimap()
    {
        if (isZoomIn)
        {
            this.transform.localScale = Vector3.one * zoom;

            slimePos = slimeObj.transform.position;
            slimePos.y = slimePos.z;
            slimePos.z = 0;

            this.transform.localPosition = -slimePos * mul * zoom;
        }
        else
        {
            this.transform.localScale = Vector3.one;
        } 
    }

    // �������� ��ġ �ٲ�
    void UpdateMinimapIcons()
    {
        foreach (var kvp in miniMapWorldObjectLookup)
        {
            minimapWorldObject = kvp.Key;

            if (isZoomIn)       // ��һ��� �϶����� �������� �������� �ƴ� �͸� ��ġ ����
            {
                if (!minimapWorldObject.Equals(slimeObj))
                {
                    minimapIcon = kvp.Value;

                    iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
                    minimapIcon.rectTransform.anchoredPosition = iconPosition * mul;
                }
            }
            else
            {
                minimapIcon = kvp.Value;

                iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
                minimapIcon.rectTransform.anchoredPosition = iconPosition * mul;
            }
        }
    }

    // ������Ʈ�� ��ġ�� ������
    Vector2 WorldPositionTomapPostion(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z);

        return pos;
    }

    // �̴ϸ� ������ ���
    public void RegisterMinimapWorldObject(MinimapWorldObject obj)
    {
        //MinimapIcon miniMapIcon = Instantiate(miniMapIconPrefab);
        //newIcon = ObjectPoolingManager.Instance.Get(EObjectFlag.minimapIcon).GetComponent<MinimapIcon>();
        newIcon = Instantiate(miniMapIconPrefab);
        newIcon.transform.SetParent(this.transform);
        newIcon.SetIcon(obj.Icon);
        newIcon.SetColor(obj.IconColor);
        miniMapWorldObjectLookup[obj] = newIcon;

        if (obj.CompareTag("Slime")) slimeObj = obj;
    }
    #endregion
}
