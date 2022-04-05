using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// using UnityEngine.UI;

public class MapEditerUI : MonoBehaviour
{
    public Camera _mainCamera; //기준 카메라
    public PlayerInput _PlayerInput;

    private void FixedUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //포인터 위치에 UI가 없음
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); //마우스 위치를 Ray로 반환
            // if (Physics.Raycast(ray, Mathf.Infinity))
            // {
            //     //레이캐스팅
            //     Debug.Log("Cube Clicked"); //큐브 클릭됨
            // }
            _PlayerInput.SwitchCurrentActionMap("Editer");
            print(_PlayerInput.currentActionMap.name);
        }
        else
        {
            //UI 클릭됨
            Debug.Log("UI Clicked");
            _PlayerInput.SwitchCurrentActionMap("UI");
            print(_PlayerInput.currentActionMap.name);
        }
    }
}