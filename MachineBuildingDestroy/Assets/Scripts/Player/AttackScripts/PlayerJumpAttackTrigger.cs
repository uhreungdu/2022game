using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJumpAttackTrigger : MonoBehaviour
{
    private PlayerJumpAttack _playerJumpAttack;
    public PlayerState _playerState;
    void Start()
    {
        _playerState = transform.root.GetComponent<PlayerState>();
        _playerJumpAttack = transform.root.GetComponent<PlayerJumpAttack>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            BulidingObject attackTarget = other.GetComponent<BulidingObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                if (SceneManager.GetActiveScene().name == "LocalRoom")
                {
                    attackTarget.OnDamage(_playerJumpAttack._damage);
                }
                else
                {
                    attackTarget.NetworkOnDamage(_playerJumpAttack._damage); 
                }

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
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        otherPlayerState.OnDamage(_playerJumpAttack._damage);
                    }
                    else
                    {
                        otherPlayerState.NetworkOnDamage(_playerJumpAttack._damage);
                    }
                    other.GetComponent<PlayerImpact>().NetworkAddImpact(transform.root.forward, 40);
                    
                    if (!otherAnimator.GetBool("Stiffen"))
                    {
                        otherPlayerState.NetworkOtherAnimatorControl("Stiffen", true);
                    }
                    else if (otherAnimator.GetBool("Stiffen"))
                    {
                        otherPlayerState.NetworkOtherAnimatorControl("RepeatStiffen", true);
                    }
                    if (otherPlayerState.dead)
                    {
                        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                        myInRoomInfo.Infomations[myInRoomInfo.mySlotNum].TotalKill++;
                        myInRoomInfo.Infomations[myInRoomInfo.mySlotNum].TotalCauseDamage += _playerJumpAttack._damage;
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
