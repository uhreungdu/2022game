using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmove : MonoBehaviour
{
    public CharacterController controller;
    public PlayerInput playerInput; // �÷��̾������� �����ϴ� ��ũ��Ʈ
    public Transform cam;

    public float speed = 6f;
    float yvelocity = 0;
    public float Cgravity = 9.8f;
    public float tempgravity = -50.0f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    float turnsmoothvelocity;
    public float jumpower = 10f;

    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float ang_x;
    public float ang_y;
    public float ang_z;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("CamPos").GetComponent<Transform>();
        playerInput = GetComponent <PlayerInput>();
        jumpower = 3f;
    }
    void Update()
    {
        // Debug.Log(cam.eulerAngles);
        //Debug.Log(cam.position);
        pos_x = cam.position.x;
        pos_y = cam.position.y;
        pos_z = cam.position.z;
        ang_x = cam.eulerAngles.x;
        ang_y = cam.eulerAngles.y;
        ang_z = cam.eulerAngles.z;
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
