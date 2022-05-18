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
    private CharacterController _characterController;
    public PlayerEquipitem _PlayerEquipitem;
    public PlayerDashAttack _PlayerDashAttack;
    public PlayerHandAttack _PlayerHandAttack;
    public PlayerJumpAttack _PlayerJumpAttack;
    public PlayerDragonPunch _PlayerDragonPunch;
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

    private bool _isGrounded;

    public bool IsGrounded
    {
        get => _isGrounded;
    }

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponentInChildren<Animator>();
        _gamePlayerInput = GetComponent<GamePlayerInput>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _characterController = GetComponent<CharacterController>();
        _PlayerState = GetComponent<PlayerState>();
        _PlayerEquipitem = GetComponent<PlayerEquipitem>();
        _PlayerDashAttack = GetComponent<PlayerDashAttack>();
        _PlayerHandAttack = GetComponent<PlayerHandAttack>();
        _PlayerJumpAttack = GetComponent<PlayerJumpAttack>();
        _PlayerDragonPunch = GetComponent<PlayerDragonPunch>();
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
                    _AudioSource.PlayOneShot(_PlayerHandAttack._AttackAudioClips[Random.Range(0, _PlayerHandAttack._AttackAudioClips.Count)]);
                });
            _Animator.SetBool("Combo", true);
        }
        else
        {
            if (!_PlayerState.aftercast)
            {
                _PlayerAnimationEvent.Play(
                    null,
                    null,
                    null,
                    () =>
                    {
                        _AudioSource.PlayOneShot(_PlayerJumpAttack._AttackAudioClips[Random.Range(0, _PlayerJumpAttack._AttackAudioClips.Count)]);
                    });
                _PlayerJumpAttack.SetAffterCast(1);
                _Animator.SetBool("Combo", true);
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
                _AudioSource.PlayOneShot(_HammerAttack._AttackAudioClips[Random.Range(0, _HammerAttack._AttackAudioClips.Count)]);
                
            });
        _HammerAttack.SetAffterCast(1);
        _Animator.SetBool("HammerAttack", _gamePlayerInput.fire);
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
                    _AudioSource.PlayOneShot(_PlayerDragonPunch._AttackAudioClips[Random.Range(0, _PlayerDragonPunch._AttackAudioClips.Count)]);
                });
            _PlayerDragonPunch.SetAffterCast(1);
            _Animator.SetBool("DragonPunch", true);
        }
    }
}