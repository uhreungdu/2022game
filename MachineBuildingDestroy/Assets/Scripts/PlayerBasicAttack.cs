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
            Debug.Log("공격");
            boxCollider.enabled = true;
        }
        else
        {
            Debug.Log("공격 안나감");
            boxCollider.enabled = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        Wall attackTarget = other.GetComponent<Wall>();
        if (attackTarget != null)
        {
            Debug.Log(attackTarget.hp);
            attackTarget.OnDamage(20);
        }
    }
}
