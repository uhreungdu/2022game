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

    private void Awake()
    {
        joystickBack = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("BeginDrag");

        var inputDir = eventData.position - joystickBack.anchoredPosition;
        var clampedDir = inputDir;
        if(inputDir.magnitude > moveRange)
        {
            clampedDir = inputDir.normalized * moveRange;
        }
        else
        {
            clampedDir = inputDir;
        }
        joystick.anchoredPosition = clampedDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        var inputDir = eventData.position - joystickBack.anchoredPosition;
        var clampedDir = inputDir;
        if (inputDir.magnitude > moveRange)
        {
            clampedDir = inputDir.normalized * moveRange;
        }
        else
        {
            clampedDir = inputDir;
        }
        joystick.anchoredPosition = clampedDir;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        joystick.anchoredPosition = Vector2.zero;
    }
}
