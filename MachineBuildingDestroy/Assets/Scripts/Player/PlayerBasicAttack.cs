using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

public class PlayerBasicAttack : MonoBehaviourPun
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public List<BoxCollider> HitBoxColliders;
    public Material boxmaterial;
    public GameObject coinprefab;
    public PlayerState playerState;
    public PlayerAnimator playeranimator;
    public thirdpersonmove Thirdpersonmove;
    
    private float timeBetAttack = 0.3f; // 공격 간격
    private float activeAttackTime = 0f; // 공격 유지 시간
    private float lastAttackTime = 0f; // 공격을 마지막에 한 시점
    
    public bool nowEquip;
    public GameObject getobj;
    public GameObject ItemObj;
    public Rigidbody item_Rigid;
    public Collider item_Coll;
    public Quaternion parent_qut;
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        playeranimator = GetComponentInChildren <PlayerAnimator>();
        Thirdpersonmove = GetComponentInChildren <thirdpersonmove>();
        HitBoxColliders.Add(GameObject.Find("Bip001 L Hand").GetComponent<BoxCollider>());
        HitBoxColliders.Add(GameObject.Find("Bip001 R Hand").GetComponent<BoxCollider>());
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
        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                //Debug.Log("앞 백터 = " + boxCollider.transform.forward);
                if (nowEquip == true)
                {
                   //Throw_item();
                    photonView.RPC("Throw_item",RpcTarget.All);
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            photonView.RPC("Equip_item",RpcTarget.All);
            //Equip_item();
        }

        if (nowEquip == true && ItemObj != null && playerState.Item == item_box_make.item_type.obstacles)
        {
            ItemObj.transform.rotation = new Quaternion(parent_qut.x,
                0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            WallObject attackTarget = other.GetComponent<WallObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                Material material = other.GetComponent<MeshRenderer>().sharedMaterial;
                if (material == null)
                {
                    material = other.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                }
                // CMeshSlicer.SlicerWorld(other.gameObject, Upvector, other.ClosestPointOnBounds(boxCollider.transform.position), boxmaterial);
                
                attackTarget.OnDamage(20);
                attackTarget.WallDestroy();
                
                Debug.Log(attackTarget.health);
            }
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
            getobj = Resources.Load<GameObject>("potion");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 tpos = GameObject.Find("Bip001 R Finger0").transform.position + Vector3.up+ Vector3.forward;
            ItemObj.transform.Translate(tpos);
            item_Coll = ItemObj.GetComponent<Collider>();
            item_Rigid = ItemObj.GetComponent<Rigidbody>();
            item_Rigid.isKinematic = true;
            item_Rigid.useGravity = false;
            item_Coll.enabled = false;
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
    }

    [PunRPC]
    public void Throw_item()
    {
        if (ItemObj == null)
            return;
        if (playerState.Item == item_box_make.item_type.potion)
        {
            item_Coll.enabled = false;
            ItemObj.transform.parent = null;
            item_Rigid.isKinematic = false;
            item_Coll.enabled = true;
            item_Rigid.useGravity = true;
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
            getobj = Resources.Load<GameObject>("Wall_Obstcle_Objs");
            ItemObj = Instantiate(getobj);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward*5f);
            ItemObj.transform.Translate(tpos);
            ItemObj.transform.rotation = new Quaternion(old_rot.x, old_rot.y, old_rot.z, old_rot.w);
            ItemObj = null;
            nowEquip = false;
        }

    }
}