using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 손에 직접적으로 들어가는 스크립트 트리거용
public class PlayerHandAttackTrigger : MonoBehaviour
{
    private PlayerHandAttack _playerHandAttack;
    public PlayerState _playerState;
    void Start()
    {
        _playerState = transform.root.GetComponent<PlayerState>();
        _playerHandAttack = transform.root.GetComponent<PlayerHandAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            WallObject attackTarget = other.GetComponent<WallObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                // attackTarget.NetworkOnDamage(_playerHandAttack._damage);
                attackTarget.OnDamage(_playerHandAttack._damage);
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
                    playerState.NetworkOnDamage(_playerHandAttack._damage);
                    playerState.OnDamage(_playerHandAttack._damage);
                    other.GetComponent<PlayerImpact>().AddImpact(transform.root.forward, 10);
                }
            }
        }

        if (other.tag == "Obstcle_Item")
        {
            Obstacle_Obj Target = other.GetComponent<Obstacle_Obj>();
            if (Target != null && !Target.dead)
            {
                Target.OnDamage(_playerState.P_Dm.Damge_formula());
                Debug.Log(Target.health);
            }
        }
    }
}
