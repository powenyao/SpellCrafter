using TMPro;
using UnityEngine;

public class UI_FloatingText : MonoBehaviour, IPooledObject
{
    private IObjectPooler pooler;
    [SerializeField] private float disappearTime = 1f;
    [SerializeField] private float disappearSpeed = 20f;
    
    [SerializeField] private TMP_Text _textMesh;
    private Color _textColor = Color.magenta;
    private float _disappearTimer;
    private Vector3 _moveVector;
    private static int _sortingOrder;
    private Enum_PopupDisappearStyle _disappearStyle;

    private Vector3 moveDirection;
    private float moveYSpeed;
    private float scaleFactor;

    private Vector3 originalScale;
    private Vector3 unnormalizedScale;
    
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
        _disappearTimer = disappearTime;
    }

    public void Setup(string content, Color color, GameObject attachedObj, 
        Vector3 direction, float speed, float newScaleFactor, Enum_PopupDisappearStyle disappearStyle)
    {
        _textMesh.SetText(content);
        _textMesh.color = color;
        _textColor = color;
        moveDirection = direction;
        moveYSpeed = speed;
        _moveVector = CalcMoveVector(attachedObj);
        scaleFactor = newScaleFactor;
        _disappearStyle = disappearStyle;

        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z) * scaleFactor;
    }

    private Vector3 CalcMoveVector(GameObject attachedObj)
    {
        Vector3 moveVector = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        if (attachedObj.TryGetComponent(out Collider newCollider))
        {
            Vector3 size = newCollider.bounds.size;
            moveVector = new Vector3(size.x  * moveDirection.x, size.y * moveDirection.y, size.z * moveDirection.z);
        }

        return moveVector * moveYSpeed;
    }

    private void Update()
    {
        PopupAnimation();
    }

    private void PopupAnimation()
    {
        //Dev.Log("Popup: " + gameObject.name + " [Speed: " + moveYSpeed + "] [Scale Factor: " + scaleFactor + "]");
        // Animation 1: float upwards
        transform.position += _moveVector * Time.deltaTime;
        _moveVector -= _moveVector * 10f * Time.deltaTime;

        // Animation 2: shrink & expand
        if (_disappearTimer > disappearTime * 0.5f)
        {
            // first half of the popup lifetime
            float increaseScaleAmount = 1f;
            unnormalizedScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // second half of the popup lifetime
            if (_disappearStyle == Enum_PopupDisappearStyle.FadeOut)
            {
                float decreaseScaleAmount = 1f;
                unnormalizedScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
            else
            {
                _textColor.a -= disappearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;
            }
        }

        _textMesh.transform.localScale = new Vector3(unnormalizedScale.x, unnormalizedScale.y, unnormalizedScale.z) * scaleFactor;

        _disappearTimer -= Time.deltaTime;
        
        if (_disappearTimer < 0)
        {
            // Animation 3: fade out
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;
            if (_textColor.a < 0)
            {
                Complete();
            }
        }
    }

    //anything required to reset this Pooled Object for use again should be done here
    private void Complete()
    {
        pooler.ReturnToPool(gameObject);
        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        unnormalizedScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
    }
}

public enum Enum_PopupDisappearStyle
{
    FadeOut,
    Transparent,
    None
}