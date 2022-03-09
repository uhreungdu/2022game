using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class thirdpersonmove : MonoBehaviourPun
{
    public CharacterController controller;
    public PlayerInput playerInput; // 플레이어조작을 관리하는 스크립트
    public Transform cam;
    private Animator playeranimator;

    public float speed = 6f;
    float yvelocity = 0;
    public float Cgravity = 4f;
    public float tempgravity = -50.0f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    float turnsmoothvelocity;
    public float jumpower = 10f;

    public float pushPower = 2.0F;

    /*
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float ang_x;
    public float ang_y;
    public float ang_z;
    */

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("CamPos").GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();
        playeranimator = GetComponent<Animator>();
        jumpower = 6f;
        Debug.Log(Application.platform);
    }
    void Update()
    {
        Jump();
        Dash();
        Movement();
    }

    public void Movement()
    {
        /*
        Debug.Log(cam.eulerAngles);
        Debug.Log(cam.position);
        pos_x = cam.position.x;
        pos_y = cam.position.y;
        pos_z = cam.position.z;
        ang_x = cam.eulerAngles.x;
        ang_y = cam.eulerAngles.y;
        ang_z = cam.eulerAngles.z;
        */

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;
        //Vector3 jumpmove = new Vector3(horizontal,0f,vertical).normalized;
        Vector3 direction = new Vector3(playerInput.rotate, 0f, playerInput.move).normalized;
        Vector3 jumpmove = new Vector3(playerInput.rotate, 0f, playerInput.move).normalized;
        if (photonView.IsMine)
        {
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothvelocity, turnsmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                jumpmove = moveDir.normalized;

                //Debug.Log(realmove.y);
            }
            jumpmove.y = yvelocity;

            controller.Move(jumpmove * speed * Time.deltaTime);

            yvelocity += tempgravity * Time.deltaTime;
            //Debug.Log(jumpmove);
            if (controller.isGrounded)
            {
                yvelocity = 0;
            }
            // Vector3 Origindirection = new Vector3(playerInput.rotate, 0f, playerInput.move);
            // playeranimator.SetFloat("Move", Origindirection.magnitude);
        }
    }

    public void Jump()
    {
        if (playerInput.jump && controller.isGrounded)
        {
            yvelocity = jumpower;
        }
    }

    public void Dash()
    {
        if (playerInput.dash && controller.isGrounded)
        {
            if (speed <= 18f)
            {
                speed += 6f * Time.deltaTime;
            }
        }
        if (!playerInput.dash && controller.isGrounded)
        {
            if (speed > 6f)
            {
                speed -= 9f * Time.deltaTime;
            }
            else
            {
                speed = 6f;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;

        if (hit.gameObject.tag == "Coin")
        {
            gameObject.GetComponentInParent<PlayerState>().AddPoint(1);
            Destroy(hit.gameObject);
        }

    }

}
