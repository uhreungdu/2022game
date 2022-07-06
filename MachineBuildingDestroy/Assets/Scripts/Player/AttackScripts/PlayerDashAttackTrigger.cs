using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDashAttackTrigger : MonoBehaviourPun
{
    private PlayerDashAttack _playerDashAttack;
    public PlayerState _playerState;
    void Start()
    {
        _playerState = transform.root.GetComponent<PlayerState>();
        _playerDashAttack = transform.root.GetComponent<PlayerDashAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                BulidingObject attackTarget = other.GetComponent<BulidingObject>();
                if (attackTarget != null && !attackTarget.dead)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        attackTarget.OnDamage(_playerDashAttack._damage);
                    }
                    else
                    {
                        attackTarget.NetworkOnDamage(_playerDashAttack._damage);
                        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                        myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _playerDashAttack._damage);
                    }

                    Debug.Log(attackTarget.health);
                }
            }
            _playerState._AudioSource.PlayOneShot(_playerDashAttack._AttackAudioClips[1]);
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                    Animator otherAnimator = other.gameObject.GetComponent<Animator>();
                    if (other.gameObject != null && !otherPlayerState.dead)
                    {
                        // && otherPlayerState.team != _playerState.team
                        if (SceneManager.GetActiveScene().name == "LocalRoom")
                        {
                            otherPlayerState.OnDamage(_playerDashAttack._damage);
                        }
                        else
                        {
                            otherPlayerState.NetworkOnDamage(_playerDashAttack._damage);
                            otherPlayerState.RecentHit(_playerState.NickName);
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
                            myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _playerDashAttack._damage);
                        }
                    }
                }
                _playerState._AudioSource.PlayOneShot(_playerDashAttack._AttackAudioClips[1]);
            }
        }

        if (other.tag == "Obstcle_Item")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Obstacle_Obj Target = other.GetComponent<Obstacle_Obj>();
                if (Target != null && !Target.dead)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        Target.OnDamage(_playerDashAttack._damage);
                    }
                    else
                    {
                        Target.NetworkOnDamage(_playerDashAttack._damage);
                        _playerState._AudioSource.PlayOneShot(_playerDashAttack._AttackAudioClips[1]);
                    }
                }
            }
        }
    }
    
    [PunRPC]
    public void CauseDamageCount(int damage)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        photonView.RPC("CauseDamageCount", RpcTarget.AllViaServer, _playerDashAttack._damage);
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
