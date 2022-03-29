using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class thirdpersonmove : MonoBehaviourPun
{
    public CharacterController controller;
    public PlayerInput playerInput; // �÷��̾������� �����ϴ� ��ũ��Ʈ
    public PlayerState playerState;
    public Transform cam;
    private Animator playeranimator;

    public float speed = 6f;
    public float Maxspeed = 18f;
    float yvelocity = 0;
    public float Cgravity = 4f;
    public float tempgravity = -50.0f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    float turnsmoothvelocity;
    public float jumpower = 10f;

    public float pushPower = 2.0F;

    public bool isAimming;
    public bool nowEquip;
    public GameObject getobj;
    public GameObject ItemObj;
    
    public bool activeattack { get; private set; }
    public bool keepactiveattack { get; private set; }


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
        playeranimator = GetComponentInChildren<Animator>();
        playerState = GetComponent<PlayerState>();
        jumpower = 6f;
        Debug.Log(Application.platform);
        isAimming = false;
        nowEquip = false;
    }
    void Update()
    {
        Jump();
        Dash();
        Movement();
        BasicAttackMove(0);
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
            if (!keepactiveattack)
            {
                if (direction.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothvelocity,
                        turnsmoothTime);
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

                // �ִϸ��̼��� ���� ����
                Vector3 Origindirection = new Vector3(playerInput.rotate, 0f, playerInput.move);
                if (Origindirection.magnitude >= 1)
                {
                    Origindirection.Normalize();
                }

                Origindirection = Origindirection * speed / Maxspeed;

                playeranimator.SetFloat("Move", Origindirection.magnitude);
                //print(Origindirection.magnitude);
            }
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
            if (speed <= Maxspeed)
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

    public void SetActiveAttack(int set)
    {
        if (set >= 1)
            activeattack = true;
        else if (set < 1)
            activeattack = false;
    }

    public void SetKeepActiveAttack(int set)
    {
        if (set >= 1)
            keepactiveattack = true;
        else if (set < 1)
            keepactiveattack = false;
    }

    public void BasicAttackMove(int num)
    {
        if (activeattack == true)
        {
            Vector3 attackVector = transform.forward;
            controller.Move(attackVector * 10f * Time.deltaTime);
        }
    }

    public void Equip_item()
    {
        if(!isAimming)
            return;
        if (playerState.Item == item_box_make.item_type.potion)
        {
            getobj = Resources.Load<GameObject>("potion");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 move_item = gameObject.transform.position + new Vector3(3, 5, 0);
            Debug.Log(move_item);
            ItemObj.transform.Translate(move_item);
            nowEquip = true;
        }
    }

    public void Throw_item()
    {
        if (ItemObj == null)
            return;
        Rigidbody Item_Ridid = ItemObj.GetComponent<Rigidbody>();
        ItemObj.transform.DetachChildren();
        Vector3 throw_Angle;
        throw_Angle = transform.forward * 50f;
        throw_Angle.y = 25f;
        Item_Ridid.AddForce(throw_Angle * 50f, ForceMode.Impulse);
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            item_box_make itemBoxMake = other.gameObject.GetComponent<item_box_make>();
            if (itemBoxMake.effect_switch == true)
            {
                playerState.SetItem(itemBoxMake.now_type);
                isAimming = true;
                //Destroy(other.gameObject);
            }
        }
        
        // if (other.gameObject.tag == "Coin")
        // {
        //     playerState.AddPoint(1);
        //     Destroy(other.gameObject);
        // }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //
        // if (hit.gameObject.tag == "Item")
        // {
        //     item_box_make itemBoxMake = hit.gameObject.GetComponent<item_box_make>();
        //     if (itemBoxMake.effect_switch == true)
        //     {
        //         playerState.SetItem(itemBoxMake.now_type);
        //         Destroy(hit.gameObject);
        //     }
        // }
        
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
        
        if (hit.gameObject.tag == "Coin")
        {
            playerState.AddPoint(1);
            Destroy(hit.gameObject);
        }
    }

}
