using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HammerAttackTrigger : MonoBehaviourPun
{
    private HammerAttack _hammerAttack;
    public Hammer _Hammer;
    public PlayerState _playerState;
    void Start()
    {
        _Hammer = GetComponent<Hammer>();
        _playerState = transform.root.GetComponent<PlayerState>();
        _hammerAttack = transform.root.GetComponent<HammerAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (other.tag == "Wall")
        {
            _playerState._AudioSource.PlayOneShot(_Hammer.HitClip);
            if (!PhotonNetwork.IsMasterClient) return;
            BulidingObject attackTarget = other.GetComponent<BulidingObject>();
            if (attackTarget != null && !attackTarget.dead)
            {
                if (SceneManager.GetActiveScene().name == "LocalRoom")
                {
                    attackTarget.OnDamage(_hammerAttack._damage);
                    ReduceDurability(1);
                }
                else
                {
                    attackTarget.NetworkOnDamage(_hammerAttack._damage);
                    photonView.RPC("ReduceDurability", RpcTarget.AllViaServer, 1);
                    MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                    myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _hammerAttack._damage);
                }

                Debug.Log(attackTarget.health);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                _playerState._AudioSource.PlayOneShot(_Hammer.HitClip);
                if (!PhotonNetwork.IsMasterClient) return;
                PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator = other.GetComponent<Animator>();
                if (other.gameObject != null && !otherPlayerState.dead 
                    /*&& otherPlayerState.team != _playerState.team*/)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        otherPlayerState.OnDamage(_hammerAttack._damage);
                        ReduceDurability(1);
                    }
                    else
                    {
                        otherPlayerState.NetworkOnDamage(_hammerAttack._damage);
                        photonView.RPC("ReduceDurability", RpcTarget.AllViaServer, 1);
                        if (otherPlayerState.dead)
                        {
                            MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                            myInRoomInfo.NetworkKillCount(myInRoomInfo.mySlotNum);
                            myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _hammerAttack._damage);
                        }
                    }
                    other.GetComponent<PlayerImpact>().NetworkAddImpact(transform.root.forward, 40);
                    if (!otherAnimator.GetBool("Falldown"))
                    {
                        otherPlayerState.NetworkOtherAnimatorControl("Falldown", true);
                    }
                }
            }
        }

        if (other.tag == "Obstcle_Item")
        {
            _playerState._AudioSource.PlayOneShot(_Hammer.HitClip);
            if (!PhotonNetwork.IsMasterClient) return;
            Obstacle_Obj Target = other.GetComponent<Obstacle_Obj>();
            if (Target != null && !Target.dead)
            {
                Target.NetworkOnDamage(_playerState.P_Dm.Damge_formula());
                Debug.Log(Target.health);
                NetWorkPlayOneShot(_Hammer.HitClip);
                photonView.RPC("ReduceDurability", RpcTarget.AllViaServer, 1);
            }
        }
    }

    [PunRPC]
    public void ReduceDurability(int value)
    {
        _Hammer.Durability -= value;
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
    public void SetPointCount(int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.SetPointCount(myInRoomInfo.mySlotNum, Point);
    }
    
    [PunRPC]
    void NetWorkPlayOneShot(AudioClip audioClip)
    {
        _playerState._AudioSource.PlayOneShot(audioClip);
    }
}
