using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDragonPunchTrigger : MonoBehaviour
{
    private PlayerDragonPunch _playerDragonPunch;
    public PlayerState _playerState;
    void Start()
    {
        _playerState = transform.root.GetComponent<PlayerState>();
        _playerDragonPunch = transform.root.GetComponent<PlayerDragonPunch>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            BulidingObject attackTarget = other.GetComponent<BulidingObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                // attackTarget.NetworkOnDamage(_playerHandAttack._damage);
                attackTarget.OnDamage(_playerDragonPunch._damage);
                Debug.Log(attackTarget.health);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator = other.GetComponent<Animator>();
                if (other.gameObject != null && !otherPlayerState.dead)
                {
                    // && otherPlayerState.team != _playerState.team
                    //playerState.NetworkOnDamage(_playerHandAttack._damage);
                    otherPlayerState.OnDamage(_playerDragonPunch._damage);
                    other.GetComponent<PlayerImpact>().NetworkAddImpact(transform.root.forward, 40);
                    if (!otherAnimator.GetBool("Stiffen"))
                    {
                        otherPlayerState.NetworkOtherAnimatorControl("Stiffen", true);
                    }
                    else if (otherAnimator.GetBool("Stiffen"))
                    {
                        otherPlayerState.NetworkOtherAnimatorControl("RepeatStiffen", true);
                    }
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
