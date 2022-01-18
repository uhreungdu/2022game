using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;

    public float timeBetAttack = 0.5f; // ���� ����
    public float activeAttackTime = 0.3f; // ���� ���� �ð�
    private float lastAttackTime; // ������ �������� �� ����

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
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
        WallObject attackTarget = other.GetComponent<WallObject>();
        Debug.Log(other.ClosestPointOnBounds(transform.position));
        CMeshSlicer.SlicerWorld(other.gameObject, transform.forward, other.ClosestPointOnBounds(transform.position), other.gameObject.GetComponent<Material>());
        // GetComponent<CMeshSlicer>().SlicerWorld(attackTarget, other.ClosestPoint, other.ClosestPoint, attackTarget.GetComponent<Material>());
        if (attackTarget != null && !attackTarget.dead)
        {
            attackTarget.OnDamage(100);
            Debug.Log(attackTarget.health);
        }
    }
}
