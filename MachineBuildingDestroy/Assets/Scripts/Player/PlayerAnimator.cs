using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator playerAnimator;
    public PlayerInput playerInput;

    public thirdpersonmove Thirdpersonmove;

    public float lastAttackTime;
    public float KeepAttackTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        Thirdpersonmove = GetComponent<thirdpersonmove>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnComboAttack()
    {
        // 1안
        //playerAnimator.SetTrigger("OnComboAttack");
        // 2안
        playerAnimator.SetBool("Combo", playerInput.fire);
        
    }

    public void DelayTimer()
    {
        if (Time.time >= lastAttackTime + KeepAttackTime)
        {
            Thirdpersonmove.SetKeepActiveAttack(0);
        }
    }
}
