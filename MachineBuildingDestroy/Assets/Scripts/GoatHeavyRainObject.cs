using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GoatHeavyRainObject : MonoBehaviourPun
{
    public GoatHeavyRain.GoatHeavyRainInfo _goatHeavyRainInfo;
    private float speed = 30f;
    public GameObject Dangerzone;
    public float ExplosionRadius = 20f;
    public AudioClip _GoatAudio;
    public AudioSource _AudioSource;

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return; 
        transform.position += _goatHeavyRainInfo.Path * speed * Time.deltaTime;
        _GoatAudio = Resources.Load<AudioClip>("Sounds/염소소리");
        _AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Collider[] colliders = Physics.OverlapSphere(_goatHeavyRainInfo.EndPosition, ExplosionRadius);
                Collider[] Knockbackcolliders =
                    Physics.OverlapSphere(_goatHeavyRainInfo.EndPosition, ExplosionRadius * 1.5f);
                foreach (var collider in Knockbackcolliders)
                {
                    if (collider.tag == "Player")
                    {
                        PlayerState playerState = collider.GetComponent<PlayerState>();
                        Thirdpersonmove thirdpersonmove = collider.GetComponent<Thirdpersonmove>();
                        Vector3 distancevecVector3 = collider.transform.position - _goatHeavyRainInfo.EndPosition;
                        float distance = distancevecVector3.magnitude;
                        if (distance < 10)
                        {
                            collider.GetComponent<PlayerImpact>()
                                .NetworkAddImpact(
                                    (collider.transform.position - _goatHeavyRainInfo.EndPosition).normalized,
                                    800);
                            thirdpersonmove.yvelocity = 1.0f;
                        }
                        else if (distance >= 10)
                        {
                            collider.GetComponent<PlayerImpact>()
                                .NetworkAddImpact(
                                    (collider.transform.position - _goatHeavyRainInfo.EndPosition).normalized,
                                    600);
                            thirdpersonmove.yvelocity = 0.8f;
                        }
                        else if (distance >= 20)
                        {
                            collider.GetComponent<PlayerImpact>()
                                .NetworkAddImpact(
                                    (collider.transform.position - _goatHeavyRainInfo.EndPosition).normalized,
                                    500);
                            thirdpersonmove.yvelocity = 0.65f;
                        }

                        Animator otherAnimator = collider.GetComponent<Animator>();
                        if (!otherAnimator.GetBool("Stiffen"))
                        {
                            playerState.NetworkOtherAnimatorControl("Stiffen", true);
                        }
                        else if (otherAnimator.GetBool("Stiffen"))
                        {
                            playerState.NetworkOtherAnimatorControl("RepeatStiffen", true);
                        }
                    }
                }

                foreach (var collider in colliders)
                {
                    if (collider.tag == "Wall")
                    {
                        BulidingObject bulidingObject = collider.GetComponent<BulidingObject>();
                        if (bulidingObject != null && !bulidingObject.dead)
                            bulidingObject.NetworkOnDamage(80);
                    }
                    else if (collider.tag == "Player")
                    {
                        PlayerState playerState = collider.GetComponent<PlayerState>();
                        if (playerState != null && !playerState.dead)
                            playerState.NetworkOnDamage(80);
                    }
                }

            }
            CinemachineShake.Instance.ShakeCamera(30f, 0.5f);
            SoundManager.GetInstance().Play(_GoatAudio, SoundManager.Sound.Effect, 1.0f, 1.0f);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject("Effect/MeteosExplosion",
                    _goatHeavyRainInfo.EndPosition, Quaternion.identity);
                PhotonNetwork.Destroy(Dangerzone);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}