using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_FloatingText : MonoBehaviour, IPooledObject
{
    private IObjectPooler pooler;
    //[SerializeField] private float disappearTime = 1f;
    //[SerializeField] private float disappearSpeed = 20f;
    
    [SerializeField] private TMP_Text _textMesh;
    [SerializeField] private Color _textColor = Color.magenta;
    //private float _disappearTimer;
    //private Vector3 _moveVector;
    private static int _sortingOrder;
    //private Enum_PopupDisappearStyle _disappearStyle;

    //private Vector3 moveDirection;
    //private float moveYSpeed;
    [SerializeField] private float scaleFactor = 1f;

    private GameObject attachedObj;

    private Vector3 originalScale;
    private Vector3 unnormalizedScale;

    [SerializeField] private bool hasAppearStage;// = true;
    [SerializeField] private float appearTime;// = 0.3f;
    private float appearTimer;
    [SerializeField] private Vector3 appearDirection;// = Vector3.up;
    [SerializeField] private float appearMoveSpeed;// = 1f;
    private Vector3 appearMoveVector;
    [SerializeField] private bool appearFadeIn;// = true;
    [SerializeField] private bool appearExpand;// = true;
    [SerializeField] private float appearExpandSpeed;// = 1f;

    [SerializeField] private bool hasStayStage;// = true;
    [SerializeField] private float stayTime;// = 0.3f;
    private float stayTimer;
    [SerializeField] private bool stayExpand;// = true;
    [SerializeField] private float stayExpandSpeed;// = 1f;

    [SerializeField] private bool hasDisappearStage;// = true;
    [SerializeField] private float disappearTime;// = 0.3f;
    private float disappearTimer;
    [SerializeField] private Vector3 disappearDirection;// = Vector3.down;
    [SerializeField] private float disappearMoveSpeed;// = 1f;
    private Vector3 disappearMoveVector;
    [SerializeField] private bool disappearFadeOut;// = true;
    [SerializeField] private bool disappearShrink;// = true;
    [SerializeField] private float disappearShrinkSpeed;// = 1f;

    private Enum_PopupStage stage;
    private bool usePrefabDefaultSetting;

    private void Awake()
    {
        if (!_textMesh)
        {
            _textMesh = transform.GetComponent<TMP_Text>();
            Dev.LogWarning("[UI_FloatingText.cs] Awake > No text mesh pro");
        }
        originalScale = _textMesh.transform.localScale;
        unnormalizedScale = originalScale;
    }
    public void OnObjectSpawn(IObjectPooler newPooler)
    {
        this.pooler = newPooler;
        // prevent former spawned text obscuring later spawned text
        //_textMesh.sortingOrder = ++_sortingOrder;
    }

    public void SetupBasicInfo(string content, GameObject attachedObj, bool usePrefabDefaultSetting, 
        Color color = default, float scaleFactor = default)
    {
        _textMesh.SetText(content);
        this.attachedObj = attachedObj;

        this.usePrefabDefaultSetting = usePrefabDefaultSetting;

        if (color != default)
        {
            _textColor = color;
        }
        _textMesh.color = _textColor;

        if (scaleFactor != default)
        {
            this.scaleFactor = scaleFactor;
        }

        _textMesh.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z) * scaleFactor;
    }

    public void SetupAppearInfo()
    {
        if (!hasAppearStage)
        {
            return;
        }

        appearTimer = appearTime;

        appearMoveVector = CalcMoveVector(appearDirection, appearMoveSpeed);

        if (appearFadeIn)
        {
            _textColor.a = 0f;
            _textMesh.color = _textColor;
        }

        if (appearExpand)
        {
            _textMesh.transform.localScale = Vector3.zero;
        }

        stage = Enum_PopupStage.Appear;

        GetPopupPositionOnColliderSurface(appearDirection);
    }

    public void SetupAppearInfo(float time, Vector3 direction, float moveSpeed, 
        bool fadeIn, bool expand, float expandSpeed)
    {
        hasAppearStage = true;

        appearTime = time;
        appearTimer = appearTime;

        appearMoveVector = CalcMoveVector(direction, moveSpeed);

        appearFadeIn = fadeIn;
        if (fadeIn)
        {
            _textColor.a = 0f;
            _textMesh.color = _textColor;
        }

        appearExpand = expand;
        appearExpandSpeed = expandSpeed;
        if (expand)
        {
            _textMesh.transform.localScale = Vector3.zero;
        }

        stage = Enum_PopupStage.Appear;

        GetPopupPositionOnColliderSurface(direction);
    }

    public void SetupStayInfo()
    {
        if (!hasStayStage)
        {
            return;
        }

        stayTimer = stayTime;

        if (!hasAppearStage)
        {
            stage = Enum_PopupStage.Stay;
        }
    }

    public void SetupStayInfo(float time, bool expand, float expandSpeed)
    {
        hasStayStage = true;
        stayTime = time;
        stayTimer = stayTime;
        stayExpand = expand;
        stayExpandSpeed = expandSpeed;

        if (!hasAppearStage)
        {
            stage = Enum_PopupStage.Stay;
        }
    }

    public void SetupDisappearInfo()
    {
        if (!hasDisappearStage)
        {
            return;
        } 

        disappearTimer = disappearTime;

        disappearMoveVector = CalcMoveVector(disappearDirection, disappearMoveSpeed);

        if (!hasAppearStage && !hasStayStage)
        {
            stage = Enum_PopupStage.Disappear;
        }
    }

    public void SetupDisappearInfo(float time, Vector3 direction, float moveSpeed,
        bool fadeOut, bool shrink, float shrinkSpeed)
    {
        hasDisappearStage = true;

        disappearTime = time;
        disappearTimer = disappearTime;

        disappearMoveVector = CalcMoveVector(direction, moveSpeed);

        disappearFadeOut = fadeOut;

        disappearShrink = shrink;
        disappearShrinkSpeed = shrinkSpeed;

        if (!hasAppearStage && !hasStayStage)
        {
            stage = Enum_PopupStage.Disappear;
        }
    }

    //public void Setup(string content, Color color, GameObject attachedObj, 
    //    Vector3 direction, float speed, float newScaleFactor, Enum_PopupDisappearStyle disappearStyle)
    //{
    //    _textMesh.SetText(content);
    //    _textMesh.color = color;
    //    _textColor = color;
    //    moveDirection = direction;
    //    moveYSpeed = speed;
    //    _moveVector = CalcMoveVector(attachedObj);
    //    scaleFactor = newScaleFactor;
    //    _disappearStyle = disappearStyle;

    //    transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z) * scaleFactor;
    //}

    private Vector3 CalcMoveVector(Vector3 direction, float speed)
    {
        Vector3 moveVector = new Vector3(direction.x, direction.y, direction.z);

        return moveVector * speed;
    }

    private void Update()
    {
        //PopupAnimation();

        if (hasAppearStage && stage == Enum_PopupStage.Appear)
        {
            PopupAppearAnimation();
        }

        if (hasStayStage && stage == Enum_PopupStage.Stay)
        {
            PopupStayAnimation();
        }

        if (hasDisappearStage && stage == Enum_PopupStage.Disappear)
        {
            PopupDisappearAnimation();
        }

        StageControl();
    }

    private void StageControl()
    {
        if (appearTimer < 0)
        {
            if (hasStayStage)
            {
                stage = Enum_PopupStage.Stay;
            }
            else if (hasDisappearStage)
            {
                stage = Enum_PopupStage.Disappear;
            }
            else
            {
                Complete();
            }
        }

        if (stayTimer < 0)
        {
            if (hasDisappearStage)
            {
                stage = Enum_PopupStage.Disappear;
            }
            else
            {
                Complete();
            }
        }

        if (disappearTimer < 0)
        {
            Complete();
        }
    }

    private void PopupAppearAnimation()
    {
        _textMesh.transform.position += appearMoveVector * Time.deltaTime;

        if (appearFadeIn)
        {
            _textColor.a += 1f / appearTime * Time.deltaTime;
            _textMesh.color = _textColor;
        }

        if (appearExpand)
        {
            unnormalizedScale += Vector3.one * appearExpandSpeed * Time.deltaTime;
            _textMesh.transform.localScale = NormalizeScale();
        }

        appearTimer -= Time.deltaTime;

        if (appearTimer < 0)
        {
            _textColor.a = 1f;
            _textMesh.color = _textColor;
        }
    }

    private void PopupStayAnimation()
    {
        if (stayExpand)
        {
            unnormalizedScale += Vector3.one * stayExpandSpeed * Time.deltaTime;
            _textMesh.transform.localScale = NormalizeScale();
        }

        stayTimer -= Time.deltaTime;
    }

    private void PopupDisappearAnimation()
    {
        _textMesh.transform.position += disappearMoveVector * Time.deltaTime;

        if (disappearFadeOut)
        {
            _textColor.a -= 1f / disappearTime * Time.deltaTime;
            _textMesh.color = _textColor;
        }

        if (disappearShrink)
        {
            unnormalizedScale -= Vector3.one * disappearShrinkSpeed * Time.deltaTime;
            _textMesh.transform.localScale = NormalizeScale();
        }

        disappearTimer -= Time.deltaTime;
    }

    //anything required to reset this Pooled Object for use again should be done here
    private void Complete()
    {
        _textMesh.transform.localPosition = Vector3.zero;
        _textMesh.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        unnormalizedScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        pooler.ReturnToPool(gameObject);

        if (!usePrefabDefaultSetting)
        {
            hasAppearStage = false;
            hasStayStage = false;
            hasDisappearStage = false;
        }

        //Dev.Log("Caller: " + attachedObj.name + "; Object: " + name);
    }

    private Vector3 NormalizeScale()
    {
        return new Vector3(unnormalizedScale.x, unnormalizedScale.y, unnormalizedScale.z) * scaleFactor;
    }

    private float GetTargetDistance(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            return Vector3.Distance(hit.transform.position, transform.position);
        }
        else
        {
            return 0;
        }
    }

    private void GetPopupPositionOnColliderSurface(Vector3 direction)
    {
        if (attachedObj != null)
        {
            if (attachedObj.TryGetComponent(out Collider newCollider))
            {
                Ray ray = new Ray(transform.position, direction);
                ray.origin = ray.GetPoint(10);
                ray.direction = -ray.direction;

                RaycastHit[] hits = new RaycastHit[10];
                Physics.RaycastNonAlloc(ray, hits);
                List<RaycastHit> list = hits.ToList().OrderBy(o => GetTargetDistance(o)).ToList();

                foreach (var hit in list)
                {
                    if (hit.transform == null)
                    {
                        //Dev.Log("Here!");
                        continue;
                    }
                    if (ReferenceEquals(hit.transform.gameObject, attachedObj))
                    {
                        transform.position = hit.point;
                        break;
                    }
                }
            }
        }
    }

    //private void PopupAnimation()
    //{
    //    //Dev.Log("Popup: " + gameObject.name + " [Speed: " + moveYSpeed + "] [Scale Factor: " + scaleFactor + "]");
    //    // Animation 1: float upwards
    //    transform.position += _moveVector * Time.deltaTime;
    //    _moveVector -= _moveVector * 20f * Time.deltaTime;

    //    // Animation 2: shrink & expand
    //    if (_disappearTimer > disappearTime * 0.5f)
    //    {
    //        // first half of the popup lifetime
    //        float increaseScaleAmount = 1f;
    //        unnormalizedScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
    //    }
    //    else
    //    {
    //        // second half of the popup lifetime
    //        if (_disappearStyle == Enum_PopupDisappearStyle.FadeOut)
    //        {
    //            float decreaseScaleAmount = 1f;
    //            unnormalizedScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
    //        }
    //        else
    //        {
    //            _textColor.a -= disappearSpeed * Time.deltaTime;
    //            _textMesh.color = _textColor;
    //        }
    //    }

    //    _textMesh.transform.localScale = new Vector3(unnormalizedScale.x, unnormalizedScale.y, unnormalizedScale.z) * scaleFactor;

    //    _disappearTimer -= Time.deltaTime;

    //    if (_disappearTimer < 0)
    //    {
    //        // Animation 3: fade out
    //        _textColor.a -= disappearSpeed * Time.deltaTime;
    //        _textMesh.color = _textColor;
    //        if (_textColor.a < 0)
    //        {
    //            Complete();
    //        }
    //    }
    //}
}

public enum Enum_PopupStage
{
    Appear,
    Stay,
    Disappear
}

public enum Enum_PopupDisappearStyle
{
    FadeOut,
    Transparent,
    None
}