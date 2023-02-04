using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

//using XROSUI.Scripts;

public enum Enum_XROSUI_Color
{
    Default,
    OnHover,
    OnSelect,
    OnActivate
}

public class Controller_UIEffects : MonoBehaviour
{
    private static readonly Color Color_OnDefault = Color.white;
    private static readonly Color Color_OnHover = new Color(0.929f, 0.094f, 0.278f);
    private static readonly Color Color_OnSelect = new Color(0.019f, 0.733f, 0.827f);
    private static readonly Color Color_OnActivate = Color.cyan;

    public Dictionary<Enum_XROSUI_Color, Color> ColorDictionaryForInteraction =
        new Dictionary<Enum_XROSUI_Color, Color>();

    public Dictionary<Enum_Elements, Color> ColorDictionaryForElemental = new Dictionary<Enum_Elements, Color>();

    // object pooler for popup
    [SerializeField]
    private GameObject defaultPopupPrefab;

    [SerializeField]
    private GameObject smallPopupPrefab;

    [SerializeField]
    private GameObject largePopupPrefab;

    public ObjectPooler popupPooler;
    public int popupPoolerSize = 20;
    private bool _hasCreatedPool = false;

    //public Enum_PopupDisappearStyle popupDisappearStyle;
    //public Color popupColorDefault;
    //public Vector3 popupDirectionDefault;
    //public float popupMoveSpeedDefault;
    //public float popupScaleFactorDefault;

    public bool usePrefabSetting; //If true, use the setting from the popup prefab; otherwise use the following settings

    public Color popupColor;
    public float popupScaleFactor;

    //Settings for the appearing stage
    public bool hasPopupAppearStage;
    public float popupAppear_Time;
    public Vector3 popupAppear_Direction;
    public float popupAppear_MoveSpeed;
    public bool popupAppear_FadeIn;
    public bool popupAppear_Expand;
    public float popupAppear_ExpandSpeed;

    //Settings for the staying stage
    public bool hasPopupStayStage;
    public float popupStay_Time;
    public bool popupStay_Expand;
    public float popupStay_ExpandSpeed;

    //Settings for the disappearing stage
    public bool hasPopupDisappearStage;
    public float popupDisappear_Time;
    public Vector3 popupDisappear_Direction;
    public float popupDisappear_MoveSpeed;
    public bool popupDisappear_FadeOut;
    public bool popupDisappear_Shrink;
    public float popupDisappear_ShrinkSpeed;

    [HideInInspector]
    public float popupMoveYSpeedSmall;

    [HideInInspector]
    public float popupScaleFactorSmall;

    void OnEnable()
    {
        ColorDictionaryForInteraction.Add(Enum_XROSUI_Color.OnHover, Color_OnHover);
        ColorDictionaryForInteraction.Add(Enum_XROSUI_Color.OnSelect, Color_OnSelect);
        ColorDictionaryForInteraction.Add(Enum_XROSUI_Color.OnActivate, Color_OnActivate);
        ColorDictionaryForInteraction.Add(Enum_XROSUI_Color.Default, Color_OnDefault);

        Color elementalColor;
        ColorUtility.TryParseHtmlString("#33CBFC", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.BlueHydro, elementalColor);

        ColorUtility.TryParseHtmlString("#C2C2C2", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.GrayNormal, elementalColor);

        ColorUtility.TryParseHtmlString("#B2EA2A", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.GreenDendro, elementalColor);

        ColorUtility.TryParseHtmlString("#FF783F", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.OrangePyro, elementalColor);

        ColorUtility.TryParseHtmlString("#D697FF", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.PurpleElectro, elementalColor);

        ColorUtility.TryParseHtmlString("#5DE0B4", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.TealAnemo, elementalColor);

        ColorUtility.TryParseHtmlString("#C9FFFF", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.WhiteCryo, elementalColor);

        ColorUtility.TryParseHtmlString("#F4BF41", out elementalColor);
        elementalColor.a = 85f / 255f;
        ColorDictionaryForElemental.Add(Enum_Elements.YellowGeo, elementalColor);
    }

    public Color GetColorForInteraction(Enum_XROSUI_Color colorName)
    {
        //Color c = colorDictionary[colorName.ToString()];
        // var c = colorDictionary2[colorName];
        //return ColorDictionary[colorName.ToString()];
        return ColorDictionaryForInteraction[colorName];
    }

    public Color GetColorForElement(Enum_Elements elementName)
    {
        return ColorDictionaryForElemental[elementName];
    }

    #region Popup

    //public void RequestSmallPopup(MonoBehaviour mb, string content, Color color = default)
    //{
    //    this.RequestPopUp(mb.transform, content, color, defaultClosePopupPrefab);
    //}

    //public void RequestSmallPopUp(Transform parent, string content, Color color = default)
    //{
    //    this.RequestPopUp(parent, content, color, defaultClosePopupPrefab);
    //}

    //public void RequestPopUp(Transform parent, string content, Color color = default, GameObject popupPrefab = null)
    //{
    //    if (popupPrefab == null && defaultPopupPrefab == null)
    //    {
    //        Dev.LogWarning("No default popup prefab assigned in Core.Ins.UIEFfects");
    //        return;
    //    }
    //    if (popupPrefab == null)
    //    {
    //        popupPrefab = defaultPopupPrefab;
    //    }

    //    if (color == default)
    //    {
    //        //Dev.Log("default: " + default);
    //        //color = new Color();
    //        color = Color.white;
    //    }
    //    if (!_hasCreatedPool)
    //    {
    //        popupPooler.Create(popupPrefab, popupPoolerSize);
    //        _hasCreatedPool = true;
    //    }

    //    //TODO for Barry: With <T> you may be able to get it so that SpawnFromPool will return UI_FloatingText, allowing us to skip a step
    //    //Ideally, we might just want something with total of 2 lines
    //    //var uiFloatingText = popupPooler.SpawnFromPool<UI_FloatingText>(parent.position, Quaternion.identity);
    //    //uiFloatingText.Setup(content, color);

    //    GameObject popupGo = popupPooler.SpawnFromPool(parent.position, Quaternion.identity);
    //    UI_FloatingText uiFloatingText = popupGo.GetComponent<UI_FloatingText>();

    //    float popupScaleFactor = (popupPrefab == defaultPopupPrefab) ? 
    //        popupScaleFactorDefault : popupScaleFactorSmall;
    //    float speed = (popupPrefab == defaultPopupPrefab) ?
    //        popupMoveSpeedDefault : popupMoveYSpeedSmall;
    //    uiFloatingText.Setup(content, color, parent.gameObject, Vector3.up, speed, popupScaleFactor, Enum_PopupDisappearStyle.FadeOut);
    //    uiFloatingText.pooler = popupPooler;
    //    popupGo.SetActive(true);
    //}

    public void RequestPopUp(Transform parent, string content, 
        Color color = default, GameObject popupPrefab = null, Enum_PopupPrefabSize size = Enum_PopupPrefabSize.None)
    {
        if (defaultPopupPrefab == null)
        {
            Dev.LogWarning(
                "[Controller_UIEffects.cs] RequestPopup > No default popup prefab assigned in Core.Ins.UIEFfects");
            return;
        }

        if (popupPrefab == null)
        {
            switch (size)
            {
                case Enum_PopupPrefabSize.Default:
                {
                    usePrefabSetting = true;
                    popupPrefab = defaultPopupPrefab;
                    break;
                }
                case Enum_PopupPrefabSize.Small:
                {
                    usePrefabSetting = true;
                    popupPrefab = smallPopupPrefab;
                    break;
                }
                case Enum_PopupPrefabSize.Large:
                {
                    usePrefabSetting = true;
                    popupPrefab = largePopupPrefab;
                    break;
                }
                case Enum_PopupPrefabSize.None:
                {
                    popupPrefab = defaultPopupPrefab;
                    break;
                }
            }
        }

        if (color == default)
        {
            //Dev.Log("default: " + default);
            //color = new Color();
            color = popupColor;
        }

        if (!_hasCreatedPool)
        {
            popupPooler.Create(popupPrefab, popupPoolerSize);
            _hasCreatedPool = true;
        }

        var popupGo = popupPooler.SpawnFromPool(parent.position, Quaternion.identity);
        var uiFloatingText = popupGo.GetComponent<UI_FloatingText>();

        if (usePrefabSetting)
        {
            uiFloatingText.SetupBasicInfo(content, parent.gameObject, true, color);
            uiFloatingText.SetupAppearInfo();
            uiFloatingText.SetupStayInfo();
            uiFloatingText.SetupDisappearInfo();
        }
        else
        {
            uiFloatingText.SetupBasicInfo(content, parent.gameObject, false, color, popupScaleFactor);
            if (hasPopupAppearStage)
            {
                uiFloatingText.SetupAppearInfo(popupAppear_Time, popupAppear_Direction, popupAppear_MoveSpeed,
                    popupAppear_FadeIn, popupAppear_Expand, popupAppear_ExpandSpeed);
            }
            if (hasPopupStayStage)
            {
                uiFloatingText.SetupStayInfo(popupStay_Time, popupStay_Expand, popupStay_ExpandSpeed);
            }
            if (hasPopupDisappearStage)
            {
                uiFloatingText.SetupDisappearInfo(popupDisappear_Time, popupDisappear_Direction, popupDisappear_MoveSpeed,
                    popupDisappear_FadeOut, popupDisappear_Shrink, popupDisappear_ShrinkSpeed);
            }
        }

        popupGo.SetActive(true);
    }

//    public void RequestPopUp(Transform parent, string content, Color color = default, GameObject popupPrefab = null,
//        bool useDefaultPopupSetting = false, Enum_PopupDisappearStyle disappearStyle = Enum_PopupDisappearStyle.None,
//        Vector3 moveDirection = default, float moveYSpeed = default, float scaleFactor = default)
//    {
//        if (popupPrefab == null && defaultPopupPrefab == null)
//        {
//            Dev.LogWarning(
//                "[Controller_UIEffects.cs] RequestPopup > No default popup prefab assigned in Core.Ins.UIEFfects");
//            return;
//        }

//        if (popupPrefab == null)
//        {
//            popupPrefab = defaultPopupPrefab;
//        }

//        if (color == default)
//        {
//            //Dev.Log("default: " + default);
//            //color = new Color();
//            color = popupColorDefault;
//        }

//        if (moveDirection == default)
//        {
//            moveDirection = popupDirectionDefault;
//        }

//        if (moveYSpeed == default)
//        {
//            moveYSpeed = popupMoveSpeedDefault;
//        }

//        if (scaleFactor == default)
//        {
//            scaleFactor = popupScaleFactorDefault;
//        }

//        if (disappearStyle == Enum_PopupDisappearStyle.None)
//        {
//            disappearStyle = popupDisappearStyle;
//        }

//        if (!_hasCreatedPool)
//        {
//            popupPooler.Create(popupPrefab, popupPoolerSize);
//            _hasCreatedPool = true;
//        }

//        //TODO for Barry: With <T> you may be able to get it so that SpawnFromPool will return UI_FloatingText, allowing us to skip a step
//        //Ideally, we might just want something with total of 2 lines
//        //var uiFloatingText = popupPooler.SpawnFromPool<UI_FloatingText>(parent.position, Quaternion.identity);
//        //uiFloatingText.Setup(content, color);

//        var popupGo = popupPooler.SpawnFromPool(parent.position, Quaternion.identity);
//        var uiFloatingText = popupGo.GetComponent<UI_FloatingText>();
//        if (useDefaultPopupSetting)
//        {
////            uiFloatingText.Setup(content, Color.white, parent.gameObject, Vector3.up, 10f, 0.1f, Enum_PopupDisappearStyle.FadeOut);
//            uiFloatingText.Setup(content, Color.white, this.gameObject, Vector3.up, popupMoveSpeedDefault,
//                popupScaleFactorDefault, Enum_PopupDisappearStyle.FadeOut);
//        }
//        else
//        {
////            uiFloatingText.Setup(content, color, parent.gameObject, moveDirection, moveYSpeed, scaleFactor, disappearStyle);
//            uiFloatingText.Setup(content, color, this.gameObject, moveDirection, moveYSpeed, scaleFactor,
//                disappearStyle);
//        }
        
//        //uiFloatingText.pooler = popupPooler;
//        popupGo.SetActive(true);
//    }

    #endregion Popup
}

public enum Enum_PopupPrefabSize
{
    Default,
    Small,
    Large,
    None
}