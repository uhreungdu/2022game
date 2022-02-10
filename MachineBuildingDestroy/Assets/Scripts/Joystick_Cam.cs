using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;


public class Joystick_Cam : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private RectTransform joystick;
    private RectTransform joystickBack;

    [SerializeField, Range(10f, 200f)]
    private float moveRange;

    public Vector2 moveVector;
    [SerializeField]
    private bool IsInput;
    public CinemachineFreeLook cinevirtual;

    private void Awake()
    {
        joystickBack = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("BeginDrag");

        MoveJoystick(eventData);
        IsInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        MoveJoystick(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        joystick.anchoredPosition = Vector2.zero;
        moveVector = Vector2.zero;
        IsInput = false;
    }

    void Update()
    {
        cinevirtual.m_XAxis.m_InputAxisValue = moveVector.x;
        cinevirtual.m_YAxis.m_InputAxisValue = moveVector.y;


    }

    public void MoveJoystick(PointerEventData eventData)
    {
        var inputDir = eventData.position - joystickBack.anchoredPosition;
        var clampedDir = inputDir;
        if (inputDir.magnitude > moveRange)
            clampedDir = inputDir.normalized * moveRange;
        else
            clampedDir = inputDir;
        joystick.anchoredPosition = clampedDir;
        moveVector = clampedDir / moveRange;
        //moveVector.x = moveVector.x * 180;
    }
}
