using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;
    public Material boxmaterial;
    public GameObject coinprefab; // ����׿�
    public PlayerState playerState;
    public float timeBetAttack = 0.5f; // ���� ����
    public float activeAttackTime = 0.1f; // ���� ���� �ð�
    private float lastAttackTime; // ������ �������� �� ����
    public bool nowEquip;
    public GameObject getobj;
    public GameObject ItemObj;
    public Rigidbody item_Rigid;
    public Collider item_Coll;
    public Quaternion parent_qut;
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        playerState = GetComponentInParent<PlayerState>();
        
    }

    // Update is called once per frame
    void Update()
    {
        parent_qut = gameObject.transform.parent.transform.rotation;
        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                //Debug.Log("�� ���� = " + boxCollider.transform.forward);
                if (Time.time >= lastAttackTime + timeBetAttack + activeAttackTime)
                {
                    lastAttackTime = Time.time;
                }

                if (nowEquip == true)
                {
                    Throw_item();
                }
                else
                {
                    boxCollider.enabled = true;
                }
                
            }
            else
            {
                // �ڵ� �ʹ� �̻��ϰ� §�� ���߿� �ٲ�
                boxCollider.enabled = false;
            }
        }
        else
        {
            boxCollider.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Equip_item();
        }

        if (nowEquip == true && ItemObj != null && playerState.Item == item_box_make.item_type.obstacles)
        {
            ItemObj.transform.rotation = new Quaternion(parent_qut.x,
                0, 0, 0);
            ItemObj.transform.Rotate(new Vector3(90,0,0));
        }

        Debug.Log(gameObject.transform.parent.transform.parent.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (other.tag == "Wall")
        {
            WallObject attackTarget = other.GetComponent<WallObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                Vector3 Upvector = Quaternion.AngleAxis(90, boxCollider.transform.up) * Vector3.forward;

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
    public void Equip_item()
    {
        if (playerState.Item == item_box_make.item_type.potion && nowEquip == false)
        {
            getobj = Resources.Load<GameObject>("potion");
            ItemObj = Instantiate(getobj);
            ItemObj.transform.SetParent(gameObject.transform);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.up*(-2f));
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
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.up*(-5f));
            ItemObj.transform.Translate(tpos);
            Quaternion temp_Q = quaternion.identity;
            ItemObj.transform.rotation = temp_Q;
            Debug.Log(ItemObj.transform.rotation);
            nowEquip = true;
            
        }
    }

    public void Throw_item()
    {
        if (ItemObj == null)
            return;
        if (playerState.Item == item_box_make.item_type.potion)
        {
            item_Coll.enabled = false;
            gameObject.transform.DetachChildren();
            item_Rigid.isKinematic = false;
            item_Coll.enabled = true;
            item_Rigid.useGravity = true;
            Vector3 throw_Angle;
            throw_Angle = gameObject.transform.up * -10f;
            throw_Angle.y = 5f;
            item_Rigid.AddForce(throw_Angle, ForceMode.Impulse);
            nowEquip = false;
        }
        if (playerState.Item == item_box_make.item_type.obstacles)
        {
            Quaternion old_rot = gameObject.transform.parent.transform.parent.transform.rotation;
            Debug.Log(old_rot);
            Destroy(ItemObj.gameObject);
            getobj = Resources.Load<GameObject>("Wall_Obstcle_Objs");
            ItemObj = Instantiate(getobj);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.up*(-5f));
            ItemObj.transform.Translate(tpos);
            ItemObj.transform.rotation = new Quaternion(old_rot.x, old_rot.y, old_rot.z, old_rot.w);
            ItemObj = null;
            nowEquip = false;
        }

    }
}