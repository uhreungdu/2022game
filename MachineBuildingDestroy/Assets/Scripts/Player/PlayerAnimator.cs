using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UniRx;

public class PlayerAnimator : MonoBehaviourPun
{
    public Animator _animator;
    public PlayerState _PlayerState;
    public GamePlayerInput gamePlayerInput;
    public Thirdpersonmove _thirdpersonmove;
    private CharacterController _characterController;

    public float lastAttackTime;
    public float keepAttackTime = 0.3f;

    public float lastStiffenTime;
    public float keepstiffenTime = 0.3f;
    
    public float lastFalldownTime;
    public float keepFalldownTime = 1.5f;
    
    public float speed = 6f;
    public float Maxspeed = 18f;
    
    private bool _isGrounded;
    public bool IsGrounded { get => _isGrounded; }
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        gamePlayerInput = GetComponent<GamePlayerInput>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _characterController = GetComponent<CharacterController>();
        _PlayerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        StiffenTimer();
        FalldownTimer();
        AnimationUpdate();
    }

    public void OnComboAttack()
    {
        // 1안
        //playerAnimator.SetTrigger("OnComboAttack");
        // 2안
        _animator.SetBool("Combo", gamePlayerInput.fire);
        
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
        if (_animator.GetBool("Stiffen"))
        {
            if (Time.time >= lastStiffenTime + keepstiffenTime)
            {
                _animator.SetBool("Stiffen", false);
            }
        }
    }
    
    
    public void FalldownTimer()
    {
        if (_animator.GetBool("Falldown"))
        {
            if (Time.time >= lastFalldownTime + keepFalldownTime)
            {
                _animator.SetBool("Falldown", false);
            }
        }
    }
    
    
    public void AnimationUpdate()
    {
        if (photonView.IsMine)
        {
            
            Vector3 Origindirection = new Vector3(gamePlayerInput.rotate, 0f, gamePlayerInput.move);
            if (Origindirection.magnitude >= 1)
            {
                Origindirection.Normalize();
            }

            Origindirection = Origindirection * speed / Maxspeed;

            _animator.SetFloat("Move", Origindirection.magnitude);

            if (gamePlayerInput.jump && _thirdpersonmove.IsGrounded() && !_PlayerState.stiffen &&
                !_thirdpersonmove.landing)
            {
                _animator.SetBool("Jump", true);
            }
            else if (!gamePlayerInput.jump || !_thirdpersonmove.IsGrounded() || !_PlayerState.stiffen ||
                     !_thirdpersonmove.landing)
            {
                _animator.SetBool("Jump", false);
            }
            
        }

        _animator.SetBool("IsGrounded", _thirdpersonmove.IsGrounded());
    }
}
