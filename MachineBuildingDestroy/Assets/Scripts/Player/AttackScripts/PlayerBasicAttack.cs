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

public class PlayerBasicAttack : MonoBehaviourPun
{
    // Start is called before the first frame update
    [FormerlySerializedAs("playerInput")] public GamePlayerInput gamePlayerInput;
    public List<BoxCollider> HitBoxColliders;
    public Material boxmaterial;
    public GameObject coinprefab;
    public PlayerState playerState;
    public PlayerAnimator playeranimator;
    public thirdpersonmove Thirdpersonmove;
    
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
        gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        playeranimator = GetComponentInChildren <PlayerAnimator>();
        Thirdpersonmove = GetComponentInChildren <thirdpersonmove>();
        HitBoxColliders.Add(GameObject.Find("Bip001 L Hand").GetComponent<BoxCollider>());
        HitBoxColliders.Add(GameObject.Find("Bip001 R Hand").GetComponent<BoxCollider>());
        getobj = Resources.Load<GameObject>("Buff_Effect");
        BuffObj = Instantiate(getobj);
        BuffObj.transform.SetParent(gameObject.transform);
        Vector3 tpos = gameObject.transform.position + Vector3.up;
        BuffObj.transform.Translate(tpos);
        BuffObj.SetActive(false);
        buff_Time = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        PressFire();
    }

    void PressFire()
    {
        if (!photonView.IsMine) return;
        // parent_qut = gameObject.transform.parent.transform.rotation;
        parent_qut = gameObject.transform.rotation;
        if (gamePlayerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                //Debug.Log("�� ���� = " + boxCollider.transform.forward);
                if (nowEquip == true)
                {
                    switch (playerState.Item)
                    {
                        case item_box_make.item_type.potion:
                        case item_box_make.item_type.obstacles:
                            Throw_item();
                            break;
                        default:
                            photonView.RPC("Throw_item",RpcTarget.All);
                            break;
                    }
                }
                else
                {
                    lastAttackTime = Time.time;
                    playeranimator.OnComboAttack();
                    Thirdpersonmove.SetKeepActiveAttack(1);
                }
            }
        }
        else
        {
            playeranimator.OnComboAttack();
        }
        
        if (Time.time >= lastAttackTime + 0.6f)
        {
            Thirdpersonmove.SetKeepActiveAttack(0);
        }

        if (Keyboard.current.eKey.isPressed)
        {
            switch (playerState.Item)
            {
                case item_box_make.item_type.potion:
                case item_box_make.item_type.obstacles:
                    Equip_item();
                    break;
                default:
                    photonView.RPC("Equip_item",RpcTarget.All);
                    break;
            }
        }

        if (nowEquip == true && ItemObj != null && playerState.Item == item_box_make.item_type.obstacles)
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
            if (other.gameObject != null && !playerState.dead)
            {
                playerState.OnDamage(20);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Heal_field")
        {
            //photonView.RPC("Receive_Heal",RpcTarget.All);
            Receive_Heal();
        }
    }
    public void SetLHandCollision(int set)
    {
        if (set > 0)
            HitBoxColliders[0].enabled = true;
        else if (set <= 0)
            HitBoxColliders[0].enabled = false;
    }
    
    public void SetRHandCollision(int set)
    {
        if (set > 0)
            HitBoxColliders[1].enabled = true;
        else if (set <= 0)
            HitBoxColliders[1].enabled = false;
    }
    
    [PunRPC]
    public void Equip_item()
    {
        if (playerState.Item == item_box_make.item_type.potion && nowEquip == false)
        {
            //getobj = Resources.Load<GameObject>("potion");
            ItemObj = PhotonNetwork.Instantiate("potion",new Vector3(0,0,0),Quaternion.identity);
            ItemObj.transform.SetParent(gameObject.transform);
            // ���� ���� ���� ���� ��ġ �޶���
            Vector3 tpos = GameObject.Find("Bip001 R Finger0").transform.position + Vector3.up+ Vector3.forward;
            ItemObj.transform.Translate(tpos);
            item_Coll = ItemObj.GetComponent<Collider>();
            item_Rigid = ItemObj.GetComponent<Rigidbody>();
            ItemObj.GetComponent<PotionState>().SetState("init");
            nowEquip = true;
        }

        if (playerState.Item == item_box_make.item_type.obstacles && nowEquip == false)
        {
            getobj = Resources.Load<GameObject>("Wall_Obstcle_Frame");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward*5f)+ Vector3.up;
            ItemObj.transform.Translate(tpos);
            Quaternion temp_Q = quaternion.identity;
            ItemObj.transform.rotation = temp_Q;
            nowEquip = true;
            
        }

        if (playerState.Item == item_box_make.item_type.Buff && BuffOn == false)
        {
            BuffOn = true;
        }
        
    }

    [PunRPC]
    public void Throw_item()
    {
        if (ItemObj == null)
            return;
        if (playerState.Item == item_box_make.item_type.potion)
        {
            ItemObj.transform.parent = null;
            ItemObj.GetComponent<PotionState>().SetState("throw");
            Vector3 throw_Angle;
            throw_Angle = gameObject.transform.forward * 10f;
            throw_Angle.y = 5f;
            item_Rigid.AddForce(throw_Angle, ForceMode.Impulse);
            nowEquip = false;
        }
        if (playerState.Item == item_box_make.item_type.obstacles)
        {
            Quaternion old_rot = gameObject.transform.rotation;
            Debug.Log(old_rot);
            Destroy(ItemObj.gameObject);
            ItemObj.transform.parent = null;
            //getobj = Resources.Load<GameObject>("Wall_Obstcle_Objs");
            //ItemObj = Instantiate(getobj);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward*5f);
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
            if (playerState.health + 20 >= 100)
            {
                playerState.RestoreHealth(20);
                LastHealTime = Time.time;
            }
            else
            {
                float remain_heal = 100 - playerState.health;
                playerState.RestoreHealth(remain_heal);
            }
        }
    }

    public void BuffCheck()
    {
        if (BuffOn)
        {
            playerState.P_Dm.set_Ite(1.5f);
            buff_Time -= Time.deltaTime;
        }
        else
        {
            playerState.P_Dm.set_Ite(1f);
            
        }

        if (buff_Time <= 0 && BuffOn == true)
        {
            BuffOn = false;
            buff_Time = 10f;
        }
        BuffObj.SetActive(BuffOn);
        print(BuffOn);
    }
}