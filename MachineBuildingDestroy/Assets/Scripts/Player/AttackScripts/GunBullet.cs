using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GunBullet : MonoBehaviourPun
{
    public PlayerGunAttack _playerGunAttack;
    public Vector3 ShootPosition;
    public Vector3 ForwardVector3;
    public float MaxDistance = 80f;
    public float Speed = 80f;
    public string ShootNickName;
    public int ShootTeam;
    private int _damage = 10;

    void Start()
    {
        MaxDistance = 80f;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (Vector3.Distance(ShootPosition, transform.position) <= MaxDistance)
        {
            transform.position += ForwardVector3 * Speed * Time.deltaTime;
        }
        else
            PhotonNetwork.Destroy(gameObject);
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
                    attackTarget.OnDamage(_damage);
                }
                else
                {
                    attackTarget.NetworkOnDamage(_damage);
                    MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                    myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _damage);
                }
                Debug.Log(attackTarget.health);
                photonView.RPC("RemoveBulletObject", RpcTarget.All);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator = other.GetComponent<Animator>();
                if (other.gameObject != null && !otherPlayerState.dead && otherPlayerState.team != ShootTeam)
                {
                    // && otherPlayerState.team != _playerState.team
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        otherPlayerState.OnDamage(_damage);
                    }
                    else
                    {
                        otherPlayerState.NetworkOnDamage(_damage);
                        otherPlayerState.RecentHit(ShootNickName);
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
                        myInRoomInfo.NetworkCauseDamageCount(myInRoomInfo.mySlotNum, _damage);
                    }
                    photonView.RPC("RemoveBulletObject", RpcTarget.All);
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
                    Target.OnDamage(_damage);
                }
                else
                {
                    Target.NetworkOnDamage(_damage);
                }
                photonView.RPC("RemoveBulletObject", RpcTarget.All);
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

    [PunRPC]
    public void RemoveBulletObject()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
    
}
