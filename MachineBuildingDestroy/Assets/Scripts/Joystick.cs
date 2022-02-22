using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField]
    private RectTransform joystick;
    private RectTransform joystickBack;

    [SerializeField, Range(10f, 200f)]
    private float moveRange;

    public Vector2 moveVector;
    private bool IsInput;

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
        if (IsInput)
        {
            
        }
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
    }
}
