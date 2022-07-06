using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// 손에 직접적으로 들어가는 스크립트 트리거용
public class PlayerHandAttackTrigger : MonoBehaviourPun
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
        if (!PhotonNetwork.IsMasterClient) return;
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            BulidingObject attackTarget;
            attackTarget = other.GetComponent<BulidingObject>();
            if (attackTarget != null)
            {
                if (!attackTarget.dead)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        attackTarget.OnDamage(_playerHandAttack._damage);
                    }
                    else
                    {
                        attackTarget.NetworkOnDamage(_playerHandAttack._damage); 
                        NetWorkPlayOneShot(_playerHandAttack._AttackAudioClips[1]);
                        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                        myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _playerHandAttack._damage);
                    }
                    Debug.Log(attackTarget.health);
                }
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator = other.GetComponent<Animator>();
                if (other.gameObject != null && !otherPlayerState.dead /*&& otherPlayerState.team != _playerState.team*/)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        otherPlayerState.OnDamage(_playerHandAttack._damage);
                    }
                    else
                    {
                        otherPlayerState.NetworkOnDamage(_playerHandAttack._damage);
                        otherPlayerState.RecentHit(_playerState.NickName);
                    }
                    
                    NetWorkPlayOneShot(_playerHandAttack._AttackAudioClips[1]);
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
                        myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _playerHandAttack._damage);
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
                    Target.OnDamage(_playerHandAttack._damage);
                }
                else
                {
                    NetWorkPlayOneShot(_playerHandAttack._AttackAudioClips[1]);
                    Target.NetworkOnDamage(_playerHandAttack._damage);
                }
            }
        }
    }
    
    [PunRPC]
    void NetWorkPlayOneShot(AudioClip audioClip)
    {
        _playerState._AudioSource.PlayOneShot(audioClip);
    }
}
