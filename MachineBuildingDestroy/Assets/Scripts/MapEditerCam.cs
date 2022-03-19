using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditerCam : MonoBehaviour
{
    public CharacterController controller;
    public MapEditerCamInput mapEditerCamInput; // 플레이어조작을 관리하는 스크립트
    public float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mapEditerCamInput = GetComponent<MapEditerCamInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        Vector3 direction = new Vector3(mapEditerCamInput.rotate, 0f, mapEditerCamInput.move).normalized;
        controller.Move(direction * speed * Time.deltaTime);
    }
}
