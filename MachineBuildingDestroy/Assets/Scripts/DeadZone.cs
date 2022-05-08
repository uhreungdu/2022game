using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.tag == "Player")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                PlayerState otherPlayerState = other.gameObject.GetComponent<PlayerState>();
                Animator otherAnimator  = other.gameObject.GetComponent<Animator>();
                if (other.gameObject != null && !otherPlayerState.dead)
                {
                    // && otherPlayerState.team != _playerState.team
                    if (SceneManager.GetActiveScene().name == "LocalRoom")
                    {
                        otherPlayerState.OnDamage(10000);
                    }
                    else
                    {
                        otherPlayerState.NetworkOnDamage(10000);
                    }
                }
            }
        }
    }
}
