using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditerCam : MonoBehaviour
{
    public MapEditerCamInput mapEditerCamInput; // 플레이어조작을 관리하는 스크립트
    public float speed = 30f;

    public MapEditerManager mapEditerManager;
    // Start is called before the first frame update
    void Start()
    {
        mapEditerCamInput = GetComponent<MapEditerCamInput>();
    }

    // Update is called once per frame
    void Update()
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
        float yZoom = -mapEditerCamInput.zoom * speed * 2;
        Vector3 direction = new Vector3(mapEditerCamInput.rotate, 0, mapEditerCamInput.move).normalized;
        
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
  
        transform.position += (direction * speed * Time.deltaTime);
        
        if (transform.position.y > 100)
            transform.position = new Vector3(transform.position.x, 100, transform.position.z);

        if (transform.position.y < 20)
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
    }
    public void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            transform.rotation = Quaternion.Euler(0f, 90, 0f);
        if (Input.GetKeyDown(KeyCode.E))
            transform.rotation = Quaternion.Euler(0f, -90, 0f);
    }

    public void CameraPoint()
    {
        float xScreenSize = Screen.width;
        float yScreenSize = Screen.height;
        // yScreenHalfSize = Camera.main.orthographicSize;
        // xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;
        // Debug.Log("화면크기 : " + xScreenHalfSize + ", " + yScreenHalfSize);
        Vector3 direction = Vector3.zero;
        if (Input.mousePosition.x < Screen.width / 10)
            direction.x = -1;
        if (Input.mousePosition.x > Screen.width / 10 * 9)
            direction.x = 1;
        if (Input.mousePosition.y < Screen.height / 10)
            direction.z = -1;
        if (Input.mousePosition.y > Screen.height / 10 * 9)
            direction.z = 1;
        direction = direction.normalized;
        transform.position += (direction * speed * Time.deltaTime);
        
    }
}
