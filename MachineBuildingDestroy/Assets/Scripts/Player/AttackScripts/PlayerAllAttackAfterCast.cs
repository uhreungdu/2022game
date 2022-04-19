using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAllAttackAfterCast : MonoBehaviour
{
    private PlayerHandAttack _playerHandAttack;
    private PlayerJumpAttack _playerJumpAttack;

    void Start()
    {
        _playerHandAttack = GetComponent<PlayerHandAttack>();
        _playerJumpAttack = GetComponent<PlayerJumpAttack>();
    }
    public bool AllAfterAfterCast()
    {
        if (_playerHandAttack.aftercast)
            return true;
        if (_playerJumpAttack.aftercast)
            return true;
        return false;
    }
    
    public bool PlayerHandAttackAfterCast()
    {
        if (_playerHandAttack.aftercast)
            return true;
        return false;
    }
    public bool PlayerJumpAttackAfterCast()
    {
        if (_playerJumpAttack.aftercast)
            return true;
        return false;
    }
}