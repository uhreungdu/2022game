using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;


public class Joystick_Cam : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _rectTransform;
    private CanvasScaler _cs;
    private float _rectSize;
    public Vector2 moveVector;

    private Vector2 _startPoint;
    private float _lastCamX;
    private float _lastCamY;

    private bool _isInput;
    public CinemachineFreeLook cinevirtual;

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

        cinevirtual.m_YAxis.m_InputAxisName = "";
        cinevirtual.m_XAxis.m_InputAxisName = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPoint = eventData.position;
        _isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveJoystick(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _lastCamX = cinevirtual.m_XAxis.Value;
        _lastCamY = cinevirtual.m_YAxis.Value;
        moveVector = Vector2.zero;
        _isInput = false;
    }

    void Update()
    {
        if (_isInput)
        {
            cinevirtual.m_XAxis.Value = _lastCamX + 180f * moveVector.x;
            cinevirtual.m_YAxis.Value = _lastCamY + moveVector.y;
        }
        else
        {
            cinevirtual.m_XAxis.Value = _lastCamX;
            cinevirtual.m_YAxis.Value = _lastCamY;
        }
    }

    public void MoveJoystick(PointerEventData eventData)
    {
        var inputDir = eventData.position - _startPoint;
        var clampedDir = inputDir;

        if (inputDir.magnitude > _rectSize / 2f)
            clampedDir = inputDir.normalized * (_rectSize / 2f);
        
        moveVector = clampedDir / (_rectSize/2f);
        Debug.Log(moveVector);
    }
}
