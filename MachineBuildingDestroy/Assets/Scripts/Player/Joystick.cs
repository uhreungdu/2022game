using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField]
    private RectTransform joystick;
    private RectTransform _rectTransform;

    private CanvasScaler _cs;
    private float _rectSize;

    public Vector2 moveVector;
    private bool _isInput = false;

    private void Awake()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
            return;
        }

        _rectTransform = GetComponent<RectTransform>();
        _cs = GetComponentInParent<CanvasScaler>();

        // Calculate ratio for dynamic screen resolution
        var wRatio = Screen.width / _cs.referenceResolution.x;
        var hRatio = Screen.height / _cs.referenceResolution.y;
        var ratio = wRatio * (1f - _cs.matchWidthOrHeight) + hRatio * _cs.matchWidthOrHeight;
        // This component is regular quadrilateral, so real width and height are same
        _rectSize = _rectTransform.rect.width * ratio;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MoveJoystick(eventData);
        _isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveJoystick(eventData);    
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        joystick.anchoredPosition = Vector2.zero;
        moveVector = Vector2.zero;
        _isInput = false;
    }

    private void Update()
    {
        //if (_isInput)
       // {
            
       // }
    }

    private void MoveJoystick(PointerEventData eventData)
    {
        var inputDir = eventData.position - (Vector2)_rectTransform.position;
        var clampedDir = inputDir;

        if (inputDir.magnitude > _rectSize / 2f)
            clampedDir = inputDir.normalized * (_rectSize / 2f);
        
        moveVector = clampedDir / (_rectSize/2f);
        joystick.anchoredPosition = moveVector * 200f;
    }
}
