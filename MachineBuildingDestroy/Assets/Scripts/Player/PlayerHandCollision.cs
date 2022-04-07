using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerState p_state;
    void Start()
    {
        p_state = transform.root.GetComponent<PlayerState>();

    }

    // Update is called once per frame
    void Update()
    {
        
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
                attackTarget.NetworkOnDamage(p_state.P_Dm.Damge_formula());
                Debug.Log(attackTarget.health);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState playerState = other.gameObject.GetComponent<PlayerState>();
                if (other.gameObject != null && !playerState.dead)
                {
                    playerState.NetworkOnDamage(p_state.P_Dm.Damge_formula());
                }
            }
        }

        if (other.tag == "Obstcle_Item")
        {
            Obstacle_Obj Target = other.GetComponent<Obstacle_Obj>();
            if (Target != null && !Target.dead)
            {
                Target.NetworkOnDamage(p_state.P_Dm.Damge_formula());
                Debug.Log(Target.health);
            }
        }
    }
}
