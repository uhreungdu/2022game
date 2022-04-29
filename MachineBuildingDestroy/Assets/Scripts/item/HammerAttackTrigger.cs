using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HammerAttackTrigger : MonoBehaviour
{
    private HammerAttack _hammerAttack;
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
                Debug.Log(attackTarget.health);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState playerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator = other.GetComponent<Animator>();
                if (other.gameObject != null && !playerState.dead)
                {
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        playerState.OnDamage(_hammerAttack._damage);
                    }
                    else
                    {
                        playerState.NetworkOnDamage(_hammerAttack._damage);
                    }

                    other.GetComponent<PlayerImpact>().AddImpact(transform.root.forward, 10);
                }
                if (!otherAnimator.GetBool("Falldown"))
                {
                    otherAnimator.SetBool("Falldown", true);
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
            }
        }
    }
}
