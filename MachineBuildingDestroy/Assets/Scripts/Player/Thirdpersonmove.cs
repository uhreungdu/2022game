using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class Thirdpersonmove : MonoBehaviourPun
{
    public CharacterController _characterController;
    public GamePlayerInput gamePlayerInput; // �÷��̾������� �����ϴ� ��ũ��Ʈ
    public PlayerState _playerState;
    public Transform cam;
    public LayerMask _fieldLayer;
    private PlayerAllAttackAfterCast _playerAllAttackAfterCast;

    public float speed = 38f;
    public float Maxspeed = 50f;
    public float Minspeed = 38f;
    public float yvelocity = 0;
    public float Cgravity = 4f;
    private float tempgravity = -3f;
    private float jumppower = 1.5f;
    public float gourndgravity = -0.05f;

    public float turnsmoothTime = 0.1f;
    public float turnsmoothvelocity;

    public float pushPower = 10.0F;

    public GameObject getobj;
    public GameObject ItemObj;

    public GameObject Rfoot;
    public GameObject Lfoot;
    public bool activeattack { get; private set; }
    public bool collidingbuilding = false;
    public bool keepactiveattack { get; private set; }
    public bool landing { get; private set; }

    public bool DashDelay;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("CamPos").GetComponent<Transform>();
        _playerAllAttackAfterCast = GetComponent<PlayerAllAttackAfterCast>();
        gamePlayerInput = GetComponent<GamePlayerInput>();
        _playerState = GetComponent<PlayerState>();
        Debug.Log(Application.platform);
        _playerState.isAimming = false;
        _playerState.nowEquip = false;
        DashDelay = false;
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
                !_playerState.aftercast && 
                !_playerState.IsCrowdControl() && !_playerState.dead && !landing
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

            // if (_playerAllAttackAfterCast.PlayerActivejumpColliderCheck())
            // {
            //     //photonView.RPC("Setyvelocity", RpcTarget.AllViaServer);
            //     //yvelocity = 0.0f;
            // }
            // else
                yvelocity += tempgravity * Time.deltaTime;
            //Debug.Log(jumpmove);
            if (IsGrounded())
            {
                yvelocity = 0;
            }
        }
    }

    [PunRPC]
    public void Setyvelocity()
    {
        yvelocity = 0.0f;
    }

    public void Jump()
    {
        if (IsGrounded() && !_playerState.stiffen && !landing)
        {
            yvelocity = jumppower;
        }
    }

    public void Dash()
    {
        if (gamePlayerInput.dash && !DashDelay && IsGrounded() && !_playerState.IsCrowdControl() && !_playerState.dead && !landing)
        {
            speed = 200;
            StartCoroutine(DashSpeed());
            DashDelay = true;
        }
        // if (!gamePlayerInput.dash || !IsGrounded() || _playerState.IsCrowdControl() || _playerState.dead || landing)
        // {
        //     if (speed > Minspeed)
        //     {
        //         speed -= 9f * Time.deltaTime;
        //     }
        //     else
        //     {
        //         speed = Minspeed;
        //     }
        // }
    }

    public IEnumerator DashSpeed()
    {
        yield return new WaitForSeconds(0.05f);
        speed = Minspeed;
        yield return new WaitForSeconds(4f);
        DashDelay = false;
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
    
    public void Setlanding(int set)
    {
        if (set >= 1)
            landing = true;
        else if (set < 1)
            landing = false;
    }

    public void Equip_item()
    {
        if (!_playerState.isAimming)
            return;
        if (_playerState.Item == item_box_make.item_type.potion)
        {
            getobj = Resources.Load<GameObject>("potion");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 move_item = gameObject.transform.position + new Vector3(3, 5, 0);
            Debug.Log(move_item);
            ItemObj.transform.Translate(move_item);
            _playerState.nowEquip = true;
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
                if (_playerState.Item == item_box_make.item_type.no_item)
                    _playerState.SetItem(itemBoxMake.now_type);
            _playerState.isAimming = true;
        }
        else if (other.gameObject.tag == "Coin")
        {
            print("coin");
            _playerState.AddPoint(1);
            if (photonView.IsMine)
            {
                photonView.RPC("GetPointCount", RpcTarget.AllViaServer, _playerState.point);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else
            {
                other.gameObject.GetComponent<Renderer>().enabled = false;
                other.gameObject.GetComponent<Collider>().enabled = false;
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
        var ray1 = new Ray(Rfoot.transform.position + Vector3.up * 0.1f, Vector3.down);
        var ray2 = new Ray(Lfoot.transform.position + Vector3.up * 0.1f, Vector3.down);
        var raycenter = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        // 탐색 거리
        var maxDistance = 0.11f;
        // 광선 디버그 용도
        Debug.DrawRay(Rfoot.transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        Debug.DrawRay(Lfoot.transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        // Raycast의 hit 여부로 판정
        // 지상에만 충돌로 레이어를 지정
        if (Physics.Raycast(ray1, maxDistance, _fieldLayer))
            return true;
        if (Physics.Raycast(ray2, maxDistance, _fieldLayer))
            return true;
        if (Physics.Raycast(raycenter, maxDistance, _fieldLayer))
            return true;
        return false;
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "DestroyWall")
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            
            if (body == null || body.isKinematic)
                return;

            // if (hit.moveDirection.y < -0.3F)
            //     return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;
        }
    }
    //
    //
    // [PunRPC]
    // public void GetPointCount(int Point)
    // {
    //     MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
    //     myInRoomInfo.GetPointCount(myInRoomInfo.mySlotNum, Point);
    // }
}