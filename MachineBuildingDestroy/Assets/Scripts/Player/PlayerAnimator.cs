using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UniRx;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviourPun
{
    public Animator _Animator;
    public PlayerState _PlayerState;
    public GamePlayerInput _gamePlayerInput;
    public Thirdpersonmove _thirdpersonmove;
    public PlayerDashAttack _PlayerDashAttack;
    public PlayerHandAttack _PlayerHandAttack;
    public PlayerJumpAttack _PlayerJumpAttack;
    public PlayerDragonPunch _PlayerDragonPunch;
    public PlayerEnergywaveAttack _PlayerEnergywaveAttack;
    public PlayerGunAttack _PlayerGunAttack;
    public PlayerAnimationEvent _PlayerAnimationEvent;
    public AudioSource _AudioSource;
    public HammerAttack _HammerAttack;

    public float lastAttackTime;
    public float keepAttackTime = 0.3f;

    public float lastStiffenTime;
    public float keepstiffenTime = 0.3f;

    public float lastFalldownTime;
    public float keepFalldownTime = 0.5f;

    public float speed = 18f;
    public float Maxspeed = 24f;

    public bool CanAirAttack { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponentInChildren<Animator>();
        _gamePlayerInput = GetComponent<GamePlayerInput>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _PlayerState = GetComponent<PlayerState>();
        _PlayerDashAttack = GetComponent<PlayerDashAttack>();
        _PlayerHandAttack = GetComponent<PlayerHandAttack>();
        _PlayerJumpAttack = GetComponent<PlayerJumpAttack>();
        _PlayerDragonPunch = GetComponent<PlayerDragonPunch>();
        _PlayerEnergywaveAttack = GetComponent<PlayerEnergywaveAttack>();
        _PlayerGunAttack = GetComponent<PlayerGunAttack>();
        _PlayerAnimationEvent = GetComponent<PlayerAnimationEvent>();
        _AudioSource = GetComponent<AudioSource>();
        _HammerAttack = GetComponent<HammerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        StiffenTimer();
        FalldownTimer();
        AnimationUpdate();
        if (_thirdpersonmove.IsGrounded())
            CanAirAttack = true;
    }

    public void OnAttack()
    {
        if (_Animator.GetBool("IsGrounded"))
        {
            _PlayerAnimationEvent.Play(
                null,
                null,
                null,
                () =>
                {
                    NetWorkPlayOneShot(_PlayerHandAttack._AttackAudioClips[0]);
                });
            _Animator.SetBool("Combo", true);
        }
        else
        {
            if (!_PlayerState.aftercast && CanAirAttack == true)
            {
                _PlayerAnimationEvent.Play(
                    null,
                    null,
                    null,
                    () =>
                    {
                        NetWorkPlayOneShot(_PlayerJumpAttack._AttackAudioClips[0]);
                    });
                _PlayerJumpAttack.SetAffterCast(1);
                _Animator.SetBool("Combo", true);
                CanAirAttack = false;
            }
        }
    }

    public void HammerAttack()
    {
        _PlayerAnimationEvent.Play(
            null,
            null,
            null,
            () =>
            {
                NetWorkPlayOneShot(_HammerAttack._AttackAudioClips[0]);
            });
        _HammerAttack.SetAffterCast(1);
        _Animator.SetBool("HammerAttack", _gamePlayerInput.fire);
    }
    
    public void EnergyWaveAttack()
    {
        _PlayerAnimationEvent.Play(
            null,
            () =>
            {
                NetWorkPlayOneShot(_PlayerEnergywaveAttack._AttackAudioClips[1]);
            },
            null,
            () =>
            {
                NetWorkPlayOneShot(_PlayerEnergywaveAttack._AttackAudioClips[0]);
            });
        _PlayerEnergywaveAttack.SetAffterCast(1);
        _Animator.SetTrigger("EnergyWave");
    }

    public void GunAttack()
    {
        _PlayerAnimationEvent.Play(
            null,
            null,
            null,
            () =>
            {
                NetWorkPlayOneShot(_PlayerGunAttack._AttackAudioClips[Random.Range(0, _PlayerGunAttack._AttackAudioClips.Count)]);
            });
        if (_PlayerGunAttack.isDelay == false)
        {
            StartCoroutine(_PlayerGunAttack.ShootBullet());
            _Animator.SetBool("Shoot", true);
            NetWorkPlayOneShot(_PlayerGunAttack._AttackAudioClips[Random.Range(0, _PlayerGunAttack._AttackAudioClips.Count)]);
            _PlayerGunAttack.isDelay = true;
        }
    }

    public void DelayTimer()
    {
        if (Time.time >= lastAttackTime + keepAttackTime)
        {
            _thirdpersonmove.SetKeepActiveAttack(0);
        }
    }

    public void StiffenTimer()
    {
        if (_Animator.GetBool("Stiffen"))
        {
            if (Time.time >= lastStiffenTime + keepstiffenTime)
            {
                _Animator.SetBool("Stiffen", false);
            }
        }
    }

    public void FalldownTimer()
    {
        if (_Animator.GetBool("Falldown"))
        {
            if (Time.time >= lastFalldownTime + keepFalldownTime)
            {
                _Animator.SetBool("Falldown", false);
            }
        }
    }


    public void AnimationUpdate()
    {
        if (photonView.IsMine)
        {
            Vector3 Origindirection = new Vector3(_gamePlayerInput.rotate, 0f, _gamePlayerInput.move);
            if (Origindirection.magnitude >= 1)
            {
                Origindirection.Normalize();
            }

            Origindirection = Origindirection * speed / Maxspeed;

            _Animator.SetFloat("Move", Origindirection.magnitude);

            if (_gamePlayerInput.jump && _thirdpersonmove.IsGrounded() && !_PlayerState.stiffen &&
                !_thirdpersonmove.landing)
            {
                _Animator.SetBool("Jump", true);
            }
            else if (!_gamePlayerInput.jump || !_thirdpersonmove.IsGrounded() || !_PlayerState.stiffen ||
                     !_thirdpersonmove.landing)
            {
                _Animator.SetBool("Jump", false);
            }
        }
        _Animator.SetBool("IsGrounded", _thirdpersonmove.IsGrounded());
    }

    public void OnDashAttack()
    {
        _PlayerDashAttack.SetAffterCast(1);
        _Animator.SetBool("DashAttack", _gamePlayerInput.fire);
    }
    public void Throw()
    {
        _Animator.SetBool("Throw", true);
    }
    
    public void DragonPunch()
    {
        if (_PlayerDragonPunch.CoolTimer()) 
        {
            _PlayerAnimationEvent.Play(
                null,
                null,
                null,
                () =>
                {
                    NetWorkPlayOneShot(_PlayerDragonPunch._AttackAudioClips[0]);
                });
            _PlayerDragonPunch.SetAffterCast(1);
            _Animator.SetBool("DragonPunch", true);
        }
    }

    [PunRPC]
    void NetWorkPlayOneShot(AudioClip audioClip)
    {
        _AudioSource.PlayOneShot(audioClip);
    }
}