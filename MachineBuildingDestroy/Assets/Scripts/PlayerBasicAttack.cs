using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;

    public float timeBetAttack = 0.5f; // ���� ����
    public float  activeAttackTime = 0.3f; // ���� ���� �ð�
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
        Wall attackTarget = other.GetComponent<Wall>();
        if (attackTarget != null && !attackTarget.dead)
        {
            Debug.Log(attackTarget.hp);
            attackTarget.OnDamage(20);
        }
    }
}
