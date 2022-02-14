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
    public float activeAttackTime = 0.3f; // ���� ���� �ð�
    private float lastAttackTime; // ������ �������� �� ����

    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        boxCollider = GetComponent<BoxCollider>();
        boxmaterial = GetComponentInChildren<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                Debug.Log("�� ���� = " + boxCollider.transform.forward);
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

        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                Debug.Log("�� ���� = " + boxCollider.transform.forward);
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
        if (other.tag != "Player")
        {
            WallObject attackTarget = other.GetComponent<WallObject>();
            Debug.Log(other.ClosestPointOnBounds(transform.position));
            Vector3 Upvector = Quaternion.AngleAxis(Random.Range(45, 135), boxCollider.transform.up) * boxCollider.transform.forward;
            if (attackTarget != null && !attackTarget.dead)
            {
                attackTarget.OnDamage(100);
                CMeshSlicer.SlicerWorld(other.gameObject, Upvector, other.ClosestPointOnBounds(boxCollider.transform.position), boxmaterial);
                Debug.Log(attackTarget.health);
            }
        }
    }
}
