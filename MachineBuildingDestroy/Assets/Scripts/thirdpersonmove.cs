using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmove : MonoBehaviour
{
    public CharacterController controller;
    public PlayerInput playerInput; // 플레이어조작을 관리하는 스크립트
    public Transform cam;

    public float speed = 6f;
    float yvelocity = 0;
    public float Cgravity = 9.8f;
    public float tempgravity = -50.0f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    float turnsmoothvelocity;
    public float jumpower = 10f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        playerInput = GetComponent <PlayerInput>();
        jumpower = 3f;
    }
    void Update()
    {
        Debug.Log(cam.eulerAngles);
        Debug.Log(cam.position);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;
        //Vector3 jumpmove = new Vector3(horizontal,0f,vertical).normalized;
        Vector3 direction = new Vector3(playerInput.rotate, 0f, playerInput.move).normalized;
        Vector3 jumpmove = new Vector3(playerInput.rotate, 0f, playerInput.move).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnsmoothvelocity, turnsmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            jumpmove = moveDir.normalized;
            
            //Debug.Log(realmove.y);
        }
        if(Input.GetButtonDown("Jump")&&controller.isGrounded)
        {
            yvelocity = jumpower;
        }
        jumpmove.y = yvelocity;
        yvelocity += tempgravity*Time.deltaTime;
        //Debug.Log(jumpmove);
        controller.Move(jumpmove*speed*Time.deltaTime);
    }
}
