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
    public PlayerAnimator _playeranimator;
    public Thirdpersonmove Thirdpersonmove;

    private float timeBetAttack = 0.3f; // ���� ����
    private float activeAttackTime = 0f; // ���� ���� �ð�
    private float lastAttackTime = 0f; // ������ �������� �� ����

    private float timeBetHeal = 0.5f; // �� ����
    private float activeHealTime = 0f; // �� ���� �ð�
    private float LastHealTime = 0f; // ������ �������� �� ����

    public bool nowEquip;
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
        _playeranimator = GetComponentInChildren<PlayerAnimator>();
        Thirdpersonmove = GetComponentInChildren<Thirdpersonmove>();
        getobj = Resources.Load<GameObject>("Buff_Effect");
        BuffObj = Instantiate(getobj);
        BuffObj.transform.SetParent(gameObject.transform);
        Vector3 tpos = gameObject.transform.position + Vector3.up;
        BuffObj.transform.Translate(tpos);
        BuffObj.SetActive(false);
        buff_Time = 10f;
    }

    // ������ ������ ȣ��˴ϴ�.
    void Update()
    {
        PressFire();
        PressItem();
    }

    void PressFire()
    {
        if (!photonView.IsMine) return;
        // parent_qut = gameObject.transform.parent.transform.rotation;
        parent_qut = gameObject.transform.rotation;
        if (_gamePlayerInput.fire)
        {
            //Debug.Log("�� ���� = " + boxCollider.transform.forward);
            if (nowEquip == true)
            {
                switch (_playerState.Item)
                {
                    case item_box_make.item_type.potion:
                    case item_box_make.item_type.obstacles:
                        Throw_item();
                        break;
                    default:
                        photonView.RPC("Throw_item", RpcTarget.All);
                        break;
                }
            }
            else
            {
                lastAttackTime = Time.time;
                _playeranimator.OnComboAttack();
            }
        }
        else
        {
            _playeranimator.OnComboAttack();
        }
    }

    private void PressItem()
    {
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

        if (nowEquip == true && ItemObj != null && _playerState.Item == item_box_make.item_type.obstacles)
        {
            ItemObj.transform.rotation = new Quaternion(parent_qut.x,
                0, 0, 0);
        }

        BuffCheck();
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
        if (_playerState.Item == item_box_make.item_type.potion && nowEquip == false)
        {
            //getobj = Resources.Load<GameObject>("potion");
            ItemObj = PhotonNetwork.Instantiate("potion", new Vector3(0, 0, 0), Quaternion.identity);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 tpos = GameObject.Find("Bip001 R Finger0").transform.position + Vector3.up + Vector3.forward;
            ItemObj.transform.Translate(tpos);
            item_Coll = ItemObj.GetComponent<Collider>();
            item_Rigid = ItemObj.GetComponent<Rigidbody>();
            ItemObj.GetComponent<PotionState>().SetState("init");
            nowEquip = true;
        }

        if (_playerState.Item == item_box_make.item_type.obstacles && nowEquip == false)
        {
            getobj = Resources.Load<GameObject>("Wall_Obstcle_Frame");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward * 5f) + Vector3.up;
            ItemObj.transform.Translate(tpos);
            Quaternion temp_Q = quaternion.identity;
            ItemObj.transform.rotation = temp_Q;
            nowEquip = true;
        }

        if (_playerState.Item == item_box_make.item_type.Buff && BuffOn == false)
        {
            BuffOn = true;
        }
    }

    [PunRPC]
    public void Throw_item()
    {
        if (ItemObj == null)
            return;
        if (_playerState.Item == item_box_make.item_type.potion)
        {
            ItemObj.transform.parent = null;
            ItemObj.GetComponent<PotionState>().SetState("throw");
            Vector3 throw_Angle;
            throw_Angle = gameObject.transform.forward * 10f;
            throw_Angle.y = 5f;
            item_Rigid.AddForce(throw_Angle, ForceMode.Impulse);
            nowEquip = false;
        }

        if (_playerState.Item == item_box_make.item_type.obstacles)
        {
            Quaternion old_rot = gameObject.transform.rotation;
            Debug.Log(old_rot);
            Destroy(ItemObj.gameObject);
            ItemObj.transform.parent = null;
            //getobj = Resources.Load<GameObject>("Wall_Obstcle_Objs");
            //ItemObj = Instantiate(getobj);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward * 5f);
            //ItemObj.transform.Translate(tpos);
            //ItemObj.transform.rotation = new Quaternion(old_rot.x, old_rot.y, old_rot.z, old_rot.w);
            ItemObj = PhotonNetwork.Instantiate("Wall_Obstcle_Objs", tpos,
                new Quaternion(old_rot.x, old_rot.y, old_rot.z, old_rot.w));
            ItemObj = null;
            nowEquip = false;
        }
    }

    [PunRPC]
    public void Receive_Heal()
    {
        if (Time.time >= LastHealTime + timeBetHeal)
        {
            if (_playerState.health + 20 >= 100)
            {
                _playerState.RestoreHealth(20);
                LastHealTime = Time.time;
            }
            else
            {
                float remain_heal = 100 - _playerState.health;
                _playerState.RestoreHealth(remain_heal);
            }
        }
    }

    public void BuffCheck()
    {
        if (BuffOn)
        {
            _playerState.P_Dm.set_Ite(1.5f);
            buff_Time -= Time.deltaTime;
        }
        else
        {
            _playerState.P_Dm.set_Ite(1.0f);
        }

        if (buff_Time <= 0 && BuffOn == true)
        {
            BuffOn = false;
            buff_Time = 10f;
        }

        BuffObj.SetActive(BuffOn);
        //print(BuffOn);
    }
}