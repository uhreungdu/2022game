using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;

    public float timeBetAttack = 0.5f; // 공격 간격
    public float  activeAttackTime = 0.3f; // 공격 유지 시간
    private float lastAttackTime; // 공격을 마지막에 한 시점

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
                // 코드 너무 이상하게 짠듯 나중에 바꿈
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
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        Wall attackTarget = other.GetComponent<Wall>();
        if (attackTarget != null && !attackTarget.dead)
        {
            Debug.Log(attackTarget.hp);
            attackTarget.OnDamage(20);
        }
    }
}
