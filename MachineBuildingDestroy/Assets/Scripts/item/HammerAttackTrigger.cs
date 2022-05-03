using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HammerAttackTrigger : MonoBehaviour
{
    private HammerAttack _hammerAttack;
    public Hammer _Hammer;
    public PlayerState _playerState;
    void Start()
    {
        _playerState = transform.root.GetComponent<PlayerState>();
        _hammerAttack = transform.root.GetComponent<HammerAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            BulidingObject attackTarget = other.GetComponent<BulidingObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                //attackTarget.NetworkOnDamage(_hammerAttack._damage);
                attackTarget.OnDamage(_hammerAttack._damage);
                _Hammer.Durability--;
                Debug.Log(attackTarget.health);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator = other.GetComponent<Animator>();
                if (other.gameObject != null && !otherPlayerState.dead 
                    /*&& otherPlayerState.team != _playerState.team*/)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        otherPlayerState.OnDamage(_hammerAttack._damage);
                    }
                    else
                    {
                        otherPlayerState.NetworkOnDamage(_hammerAttack._damage);
                        if (otherPlayerState.dead)
                        {
                            MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                            myInRoomInfo.Infomations[myInRoomInfo.mySlotNum].TotalKill++;
                        }
                    }

                    other.GetComponent<PlayerImpact>().NetworkAddImpact(transform.root.forward, 40);
                    _Hammer.Durability--;
                    if (!otherAnimator.GetBool("Falldown"))
                    {
                        otherPlayerState.NetworkOtherAnimatorControl("Falldown", true);
                    }
                }
            }
        }

        if (other.tag == "Obstcle_Item")
        {
            Obstacle_Obj Target = other.GetComponent<Obstacle_Obj>();
            if (Target != null && !Target.dead)
            {
                Target.NetworkOnDamage(_playerState.P_Dm.Damge_formula());
                Debug.Log(Target.health);
                _Hammer.Durability--;
            }
        }
    }
}
