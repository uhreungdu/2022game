using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimator : MonoBehaviour
{
    public Animator _animator;
     public GamePlayerInput gamePlayerInput;

    public thirdpersonmove Thirdpersonmove;

    public float lastAttackTime;
    public float keepAttackTime = 0.3f;

    public float lastStiffenTime;
    public float keepstiffenTime = 0.3f;
    
    public float speed = 6f;
    public float Maxspeed = 18f;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        gamePlayerInput = GetComponent<GamePlayerInput>();
        Thirdpersonmove = GetComponent<thirdpersonmove>();
    }

    // Update is called once per frame
    void Update()
    {
        StiffenTimer();
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
            Thirdpersonmove.SetKeepActiveAttack(0);
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
    public void StiffenMove()
    {
        if (_animator.GetBool("Stiffen"))
        {
            if (Time.time >= lastStiffenTime + keepstiffenTime)
            {
                _animator.SetBool("Stiffen", false);
            }
        }
    }
    
    
    public void AnimationUpdate()
    {
        Vector3 Origindirection = new Vector3(gamePlayerInput.rotate, 0f, gamePlayerInput.move);
        if (Origindirection.magnitude >= 1)
        {
            Origindirection.Normalize();
        }

        Origindirection = Origindirection * speed / Maxspeed;

        _animator.SetFloat("Move", Origindirection.magnitude);
    }
}
