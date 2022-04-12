using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimator : MonoBehaviour
{
    public Animator playerAnimator;
    [FormerlySerializedAs("playerInput")] public GamePlayerInput gamePlayerInput;

    public thirdpersonmove Thirdpersonmove;

    public float lastAttackTime;
    public float KeepAttackTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        gamePlayerInput = GetComponent<GamePlayerInput>();
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
        playerAnimator.SetBool("Combo", gamePlayerInput.fire);
        
    }

    public void DelayTimer()
    {
        if (Time.time >= lastAttackTime + KeepAttackTime)
        {
            Thirdpersonmove.SetKeepActiveAttack(0);
        }
    }
}
