using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class thirdpersonmove : MonoBehaviourPun
{
    public CharacterController controller;
    public GamePlayerInput gamePlayerInput; // �÷��̾������� �����ϴ� ��ũ��Ʈ
    public PlayerState playerState;
    public Transform cam;

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

    public GameObject getobj;
    public GameObject ItemObj;

    public bool activeattack { get; private set; }
    public bool collidingbuilding = false;
    public bool keepactiveattack { get; private set; }
    public bool stiffen { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("CamPos").GetComponent<Transform>();
        gamePlayerInput = GetComponent<GamePlayerInput>();
        playerState = GetComponent<PlayerState>();
        jumpower = 6f;
        Debug.Log(Application.platform);
        playerState.isAimming = false;
        playerState.nowEquip = false;
    }

    void FixedUpdate()
    {
        Jump();
        Dash();
        Movement();
    }
    void Update()
    {
        
    }

    public void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(gamePlayerInput.rotate, 0f, gamePlayerInput.move).normalized;
        Vector3 jumpmove = Vector3.zero;
        if (photonView.IsMine)
        {
            if (direction.magnitude >= 0.1f && !keepactiveattack && !stiffen && !playerState.dead)
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
            //print(Origindirection.magnitude);
        }
    }

    public void Jump()
    {
        if (gamePlayerInput.jump && controller.isGrounded && !stiffen)
        {
            yvelocity = jumpower;
        }
    }

    public void Dash()
    {
        if (gamePlayerInput.dash && controller.isGrounded && !stiffen && !playerState.dead)
        {
            if (speed <= Maxspeed)
            {
                speed += 6f * Time.deltaTime;
            }
        }

        if (!gamePlayerInput.dash && controller.isGrounded || stiffen || playerState.dead)
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

    public void SetStiffen(int set)
    {
        if (set >= 1)
            stiffen = true;
        else if (set < 1)
            stiffen = false;
    }

    public void Equip_item()
    {
        if (!playerState.isAimming)
            return;
        if (playerState.Item == item_box_make.item_type.potion)
        {
            getobj = Resources.Load<GameObject>("potion");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 move_item = gameObject.transform.position + new Vector3(3, 5, 0);
            Debug.Log(move_item);
            ItemObj.transform.Translate(move_item);
            playerState.nowEquip = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            item_box_make itemBoxMake = other.gameObject.GetComponent<item_box_make>();
            if (PhotonNetwork.IsMasterClient)
                playerState.SetItem(itemBoxMake.now_type);
            playerState.isAimming = true;
        }
        else if (other.gameObject.tag == "Coin")
        {
            print("coin");
            playerState.AddPoint(1);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else
            {
                other.gameObject.GetComponent<Renderer>().enabled = false;
                other.gameObject.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            collidingbuilding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            collidingbuilding = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /*
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
        
        // if (hit.gameObject.tag == "Coin")
        // {
        //     playerState.AddPoint(1);
        //     Destroy(hit.gameObject);
        // }
        */
    }
}