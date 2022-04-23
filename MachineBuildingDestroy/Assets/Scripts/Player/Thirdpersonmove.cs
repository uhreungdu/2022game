using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class Thirdpersonmove : MonoBehaviourPun
{
    public CharacterController _characterController;
    public GamePlayerInput gamePlayerInput; // �÷��̾������� �����ϴ� ��ũ��Ʈ
    public PlayerState playerState;
    public Transform cam;
    public LayerMask _fieldLayer;
    private PlayerAllAttackAfterCast _playerAllAttackAfterCast;

    public float speed = 6f;
    public float Maxspeed = 18f;
    public float yvelocity = 0;
    public float Cgravity = 4f;
    private float tempgravity = -1.0f;
    private float jumppower = 0.5f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    float turnsmoothvelocity;

    public float pushPower = 2.0F;

    public GameObject getobj;
    public GameObject ItemObj;

    public bool activeattack { get; private set; }
    public bool collidingbuilding = false;
    public bool keepactiveattack { get; private set; }
    public bool stiffen { get; private set; }

    public bool landing { get; private set; }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("CamPos").GetComponent<Transform>();
        _playerAllAttackAfterCast = GetComponent<PlayerAllAttackAfterCast>();
        gamePlayerInput = GetComponent<GamePlayerInput>();
        playerState = GetComponent<PlayerState>();
        Debug.Log(Application.platform);
        playerState.isAimming = false;
        playerState.nowEquip = false;
    }

    void FixedUpdate()
    {
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
            if (direction.magnitude >= 0.1f &&
                !_playerAllAttackAfterCast.AllAfterAfterCast() && 
                !stiffen && !playerState.dead && !landing
               )
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothvelocity,
                    turnsmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                jumpmove = moveDir.normalized;

                //Debug.Log(realmove.y);
            }

            jumpmove *= speed * Time.deltaTime;
            jumpmove.y = yvelocity;
            _characterController.Move(jumpmove);

            if (_playerAllAttackAfterCast.PlayerActivejumpColliderCheck())
            {
                yvelocity = 0;
                yvelocity += (tempgravity) * Time.deltaTime;
            }
            else 
                yvelocity += tempgravity * Time.deltaTime;
            //Debug.Log(jumpmove);
            if (IsGrounded())
            {
                yvelocity = 0;
            }
        }
    }

    public void Jump()
    {
        if (IsGrounded() && !stiffen && !landing)
        {
            yvelocity = jumppower;
        }
    }

    public void Dash()
    {
        if (gamePlayerInput.dash && IsGrounded() && !stiffen && !playerState.dead && !landing)
        {
            if (speed <= Maxspeed)
            {
                speed += 6f * Time.deltaTime;
            }
        }

        if (!gamePlayerInput.dash || !IsGrounded() || stiffen || playerState.dead || landing)
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
    
    public void Setlanding(int set)
    {
        if (set >= 1)
            landing = true;
        else if (set < 1)
            landing = false;
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

    public void ThrowPotion()
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

    public bool IsGrounded()
    {
        // CharacterController.IsGrounded가 true라면 Raycast를 사용하지 않고 판정 종료
        if (_characterController.isGrounded) return true;
        // 발사하는 광선의 초기 위치와 방향
        // 약간 신체에 박혀 있는 위치로부터 발사하지 않으면 제대로 판정할 수 없을 때가 있다.
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        // 탐색 거리
        var maxDistance = 0.11f;
        // 광선 디버그 용도
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        // Raycast의 hit 여부로 판정
        // 지상에만 충돌로 레이어를 지정
        return Physics.Raycast(ray, maxDistance, _fieldLayer);
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