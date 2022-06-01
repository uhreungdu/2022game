using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
// using static item_box_make.item_type;

public class PlayerKeySystem : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GamePlayerInput _gamePlayerInput;
    public List<BoxCollider> HitBoxColliders;
    public Material boxmaterial;
    public GameObject coinprefab;
    public PlayerState _playerState;
    public PlayerAnimator _playerAnimator;
    public Thirdpersonmove Thirdpersonmove;
    private PlayerEquipitem _playerEquipitem;

    private float timeBetAttack = 0.3f; // ���� ����
    private float activeAttackTime = 0f; // ���� ���� �ð�
    private float lastAttackTime = 0f; // ������ �������� �� ����

    private float timeBetHeal = 0.5f; // �� ����
    private float activeHealTime = 0f; // �� ���� �ð�
    private float LastHealTime = 0f; // ������ �������� �� ����

    public bool BuffOn;
    public float buff_Time;
    public GameObject getobj;
    public GameObject ItemObj;
    public GameObject BuffObj;
    public Rigidbody item_Rigid;
    public Collider item_Coll;
    public Quaternion parent_qut;

    void Start()
    {
        _gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        _playerState = GetComponentInParent<PlayerState>();
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
        Thirdpersonmove = GetComponentInChildren<Thirdpersonmove>();
        _playerEquipitem = GetComponent<PlayerEquipitem>();
        
    }

    // ������ ������ ȣ��˴ϴ�.
    void Update()
    {
        PressFire();
        PressItem();
        BuffCheck();
    }

    void PressFire()
    {
        if (!photonView.IsMine) return;
        // parent_qut = gameObject.transform.parent.transform.rotation;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _playerAnimator.DragonPunch();
        }
        else
        {
            _playerAnimator._Animator.SetBool("DragonPunch", false);
        }
        if (_gamePlayerInput.fire)
        {
            bool DashAttack = _playerAnimator._Animator.GetBool("DashAttack");
            bool HammerAttack = _playerAnimator._Animator.GetBool("HammerAttack");
            bool Combo = _playerAnimator._Animator.GetBool("Combo");
            bool Throw = _playerAnimator._Animator.GetBool("Throw");
            //Debug.Log("�� ���� = " + boxCollider.transform.forward);
            if (_playerState.nowEquip == true && !_playerState.aftercast)
            {
                switch (_playerState.Item)
                {
                    case item_box_make.item_type.potion:
                    case item_box_make.item_type.obstacles:
                        Throw_item();
                        _playerAnimator.Throw();
                        break;
                    case item_box_make.item_type.Hammer:
                        _playerAnimator.HammerAttack();
                        break;
                    case item_box_make.item_type.EnergyWave:
                        _playerAnimator.EnergyWaveAttack();
                        break;
                    default:
                        photonView.RPC("Throw_item", RpcTarget.AllViaServer);
                        break;
                }
            }
            else if (_gamePlayerInput.dash && !_gamePlayerInput.fireKeyDown && !_playerState.aftercast)
            {
                _playerAnimator.OnDashAttack();
            }
            else
            {
                _playerAnimator._Animator.SetBool("DashAttack", false);
                _playerAnimator._Animator.SetBool("HammerAttack", false);
                _playerAnimator._Animator.SetBool("Throw", false);
                if ( !_gamePlayerInput.fireKeyDown 
                     || (_gamePlayerInput.fireKeyDown && _playerAnimator._Animator.GetBool("Combo")))
                {
                    lastAttackTime = Time.time;
                    _playerAnimator.OnAttack();
                }
                else
                {
                    _playerAnimator._Animator.SetBool("Combo", false);
                }
            }
        }
        else
        {
            _playerAnimator._Animator.SetBool("DashAttack", false);
            _playerAnimator._Animator.SetBool("HammerAttack", false);
            _playerAnimator._Animator.SetBool("Combo", false);
            _playerAnimator._Animator.SetBool("Throw", false);
        }
    }

    private void PressItem()
    {
        if (!photonView.IsMine) return;
        if (_gamePlayerInput.item)
        {
            switch (_playerState.Item)
            {
                case item_box_make.item_type.potion:
                case item_box_make.item_type.obstacles:
                    Equip_item();
                    break;
                default:
                    photonView.RPC("Equip_item", RpcTarget.All);
                    break;
            }
        }

        if (_playerState.nowEquip == true && _playerEquipitem._ItemObject != null && _playerState.Item == item_box_make.item_type.obstacles)
        {
            parent_qut = gameObject.transform.rotation;
            ItemObj.transform.rotation = new Quaternion(parent_qut.x,
                0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (other.tag == "Wall")
        {
            // WallObject attackTarget = other.GetComponent<WallObject>();
            // if (attackTarget != null && !attackTarget.dead)
            // {
            //     Material material = other.GetComponent<MeshRenderer>().sharedMaterial;
            //     if (material == null)
            //     {
            //         material = other.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            //     }
            //     // CMeshSlicer.SlicerWorld(other.gameObject, Upvector, other.ClosestPointOnBounds(boxCollider.transform.position), boxmaterial);
            //     
            //     attackTarget.OnDamage(20);
            //     attackTarget.WallDestroy();
            //     
            //     Debug.Log(attackTarget.health);
            // }
        }

        if (other.tag == "DestroyWall")
        {
            // Debug.Log(other.ClosestPointOnBounds(transform.position));
            // WallObject attackTargetParent = other.GetComponentInParent<WallObject>();
            //
            // if (!attackTargetParent.dead)
            // {
            //     Vector3 Upvector = Quaternion.AngleAxis(90, boxCollider.transform.up) * Vector3.forward;
            //
            //     Material material = other.GetComponent<MeshRenderer>().sharedMaterial;
            //     if (material == null)
            //     {
            //         material = other.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            //     }
            //     CMeshSlicer.Sliceseveraltimes(other.gameObject, Vector3.up, material, 1);
            //     // CMeshSlicer.SlicerWorld(other.gameObject, Upvector, other.ClosestPointOnBounds(boxCollider.transform.position), material);
            // }
            //
            // WallObject attackTarget = other.GetComponent<WallObject>();
            //
            // if (attackTarget != null && !attackTarget.dead && attackTargetParent == attackTarget)
            // {
            //     attackTarget.OnDamage(20);
            //     Debug.Log(attackTarget.health);
            // }
        }

        if (other.tag == "Player")
        {
            PlayerState playerState = other.gameObject.GetComponent<PlayerState>();
            if (other.gameObject != null && !playerState.dead && playerState.team != _playerState.team)
            {
                playerState.OnDamage(20);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Heal_field")
        {
            photonView.RPC("Receive_Heal", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Equip_item()
    {
        _playerEquipitem.Equip_item();
    }

    [PunRPC]
    public void Throw_item()
    {
        _playerEquipitem.Throw_item();
    }

    [PunRPC]
    public void Receive_Heal()
    {
        _playerEquipitem.Receive_Heal();
    }

    public void BuffCheck()
    {
        _playerEquipitem.BuffCheck();
    }
}