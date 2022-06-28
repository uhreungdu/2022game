using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGumAttackTrigger : MonoBehaviourPun
{
    private PlayerGunAttack _playerJumpAttack;
    public PlayerState _playerState;
    void Start()
    {
        _playerState = transform.root.GetComponent<PlayerState>();
        _playerJumpAttack = transform.root.GetComponent<PlayerGunAttack>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            BulidingObject attackTarget = other.GetComponentInParent<BulidingObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                if (SceneManager.GetActiveScene().name == "LocalRoom")
                {
                    attackTarget.OnDamage(_playerJumpAttack._damage);
                }
                else
                {
                    attackTarget.NetworkOnDamage(_playerJumpAttack._damage);
                    MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                    myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _playerJumpAttack._damage);
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
                        myInRoomInfo.NetworkKillCount(myInRoomInfo.mySlotNum);
                        myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _playerJumpAttack._damage);
                    }
                }
            }
        }

        if (other.tag == "Obstcle_Item")
        {
            Obstacle_Obj Target = other.GetComponent<Obstacle_Obj>();
            if (Target != null && !Target.dead)
            {
                if (SceneManager.GetActiveScene().name == "LocalRoom")
                {
                    Target.OnDamage(_playerJumpAttack._damage);
                }
                else
                {
                    Target.NetworkOnDamage(_playerJumpAttack._damage);
                }
            }
        }
    }
    
    
    [PunRPC]
    public void CauseDamageCount(int damage)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.CauseDamageCount(myInRoomInfo.mySlotNum, damage);
    }
    
    [PunRPC]
    public void KillCount()
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.KillCount(myInRoomInfo.mySlotNum);
    }
    
    [PunRPC]
    public void DeathCount()
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.DeathCount(myInRoomInfo.mySlotNum);
    }
    
    [PunRPC]
    public void GetPointCount(int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.GetPointCount(myInRoomInfo.mySlotNum, Point);
    }
}
