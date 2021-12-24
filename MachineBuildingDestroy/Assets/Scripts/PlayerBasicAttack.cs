using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;

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
            Debug.Log("����");
            boxCollider.enabled = true;
        }
        else
        {
            Debug.Log("���� �ȳ���");
            boxCollider.enabled = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        Wall attackTarget = other.GetComponent<Wall>();
        if (attackTarget != null)
        {
            Debug.Log(attackTarget.hp);
            attackTarget.OnDamage(20);
        }
    }
}
