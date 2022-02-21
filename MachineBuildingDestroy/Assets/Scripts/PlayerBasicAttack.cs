using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInput playerInput;
    public BoxCollider boxCollider;
    public Material boxmaterial;
    public GameObject coinprefab;     // 디버그용

    public float timeBetAttack = 0.5f; // 공격 간격
    public float activeAttackTime = 0.3f; // 공격 유지 시간
    private float lastAttackTime; // 공격을 마지막에 한 시점

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
                Debug.Log("앞 백터 = " + boxCollider.transform.forward);
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

        if (playerInput.fire)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                Debug.Log("앞 백터 = " + boxCollider.transform.forward);
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
