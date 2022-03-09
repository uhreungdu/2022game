using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;
    public Material boxmaterial;
    public GameObject coinprefab;     // ����׿�

    public float timeBetAttack = 0.5f; // ���� ����
    public float activeAttackTime = 0.1f; // ���� ���� �ð�
    private float lastAttackTime; // ������ �������� �� ����

    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        boxCollider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                //Debug.Log("�� ���� = " + boxCollider.transform.forward);
                if (Time.time >= lastAttackTime + timeBetAttack + activeAttackTime)
                {
                    lastAttackTime = Time.time;
                }
                boxCollider.enabled = true;
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

    }
    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (other.tag == "Wall")
        {
            WallObject attackTarget = other.GetComponent<WallObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                attackTarget.OnDamage(20);
                Debug.Log(attackTarget.health);
            }
            // CMeshSlicer.SlicerWorld(other.gameObject, Upvector, other.ClosestPointOnBounds(boxCollider.transform.position), boxmaterial);
        }
        if (other.tag == "DestroyWall")
        {
            WallObject attackTarget = other.GetComponentInParent<WallObject>();
            // Debug.Log(other.ClosestPointOnBounds(transform.position));

            Vector3 Upvector = Quaternion.AngleAxis(90, boxCollider.transform.up) * boxCollider.transform.forward;
            if (!attackTarget.dead)
            {
                Material material = other.GetComponent<MeshRenderer>().sharedMaterial;
                if (material == null)
                {
                    material = other.GetComponentInChildren<MeshRenderer>().sharedMaterial;
                }
                CMeshSlicer.SlicerWorld(other.gameObject, Upvector, other.ClosestPointOnBounds(boxCollider.transform.position), material);
            }

            if (attackTarget != null && !attackTarget.dead)
            {
                attackTarget.OnDamage(20);
                Debug.Log(attackTarget.health);
            }
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
}
