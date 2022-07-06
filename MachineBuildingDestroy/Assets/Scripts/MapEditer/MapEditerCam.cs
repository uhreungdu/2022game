using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapEditerCam : MonoBehaviour
{
    public MapEditerCamInput mapEditerCamInput; // 플레이어조작을 관리하는 스크립트
    public MapEditerManager mapEditerManager;
    public float speed = 30f;
    public PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        mapEditerCamInput = GetComponent<MapEditerCamInput>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (mapEditerManager.SaveMode == false)
        {
            Movement();
            Rotate();
            CameraPoint();
        }
    }

    public void Movement()
    {
        float yZoom = -mapEditerCamInput._zoom * 160f * Time.deltaTime;
        Vector3 direction = mapEditerCamInput._direction;
        if (yZoom <= 0)
        {
            if (transform.position.y >= 20)
            {
                direction.y = yZoom;
            }
        }
        if (yZoom > 0)
        {
            if (transform.position.y < 100)
            {
                direction.y = yZoom;
            }
        }
  
        transform.position += direction;
        mapEditerCamInput.ResetDirection();

        if (transform.position.y > 100)
            transform.position = new Vector3(transform.position.x, 100, transform.position.z);

        if (transform.position.y < 20)
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
    }
    public void Rotate()
    {
        
    }

    public void CameraPoint()
    {
        if (_playerInput.currentActionMap.name == "Editer")
        {
            int xScreenSize = Screen.width;
            int yScreenSize = Screen.height;
            Vector3 direction = Vector3.zero;
            // if (Mouse.current.position.x.ReadValue() < xScreenSize / 10)
            //     direction.x = -1;
            // if (Mouse.current.position.x.ReadValue() > xScreenSize / 10 * 9)
            //     direction.x = 1;
            // if (Mouse.current.position.y.ReadValue() < yScreenSize / 10)
            //     direction.z = -1;
            // if (Mouse.current.position.y.ReadValue() > yScreenSize / 10 * 9)
            //     direction.z = 1;

            direction = direction.normalized;
            transform.position += (direction * speed * Time.deltaTime);
        }
    }
}
