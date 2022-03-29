using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator playerAnimator;
    public PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
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
}
