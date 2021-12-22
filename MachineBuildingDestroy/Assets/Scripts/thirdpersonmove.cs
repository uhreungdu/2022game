using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmove : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    float yvelocity = 0;
    public float Cgravity = 9.8f;
    public float tempgravity = -20.0f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    float turnsmoothvelocity;
    public float jumpower = 50f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpower = 5f;
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;
        Vector3 jumpmove = new Vector3(horizontal,0f,vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnsmoothvelocity, turnsmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            //Debug.Log(moveDir.normalized);
            jumpmove = moveDir.normalized;
            if(Input.GetButtonDown("Jump")&&controller.isGrounded)
            {
                yvelocity = jumpower;
            }
            jumpmove.y = yvelocity;
            yvelocity += tempgravity*Time.deltaTime;
            //Debug.Log(realmove.y);
        }
        Debug.Log(jumpmove);
        controller.Move(jumpmove*speed*Time.deltaTime);
    }
}
