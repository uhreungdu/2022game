using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmove : MonoBehaviour
{
    public CharacterController controller;
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Animator playeranimator; // 애니메이션
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
        playerInput = GetComponent <PlayerInput>();
        playeranimator = GetComponent<Animator>();
        jumpower = 3f;
    }
    void Update()
    {
        Vector3 direction = new Vector3(playerInput.rotate, 0f, playerInput.move).normalized;
        Vector3 jumpmove = new Vector3(playerInput.rotate, 0f, playerInput.move).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnsmoothvelocity, turnsmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle,0f);

            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            //Debug.Log(moveDir.normalized);
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

        Vector3 Origindirection = new Vector3(playerInput.rotate, 0f, playerInput.move);
        playeranimator.SetFloat("Move", Origindirection.magnitude);
    }
}
