using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAllAttackAfterCast : MonoBehaviour
{
    private PlayerHandAttack _playerHandAttack;
    private PlayerJumpAttack _playerJumpAttack;
    private HammerAttack _hammerAttack;

    void Start()
    {
        _playerHandAttack = GetComponent<PlayerHandAttack>();
        _playerJumpAttack = GetComponent<PlayerJumpAttack>();
        _hammerAttack = GetComponent<HammerAttack>();
    }
    public bool AllAfterAfterCast()
    {
        if (_playerHandAttack.aftercast)
            return true;
        if (_playerJumpAttack.aftercast)
            return true;
        if (_hammerAttack.aftercast)
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

    public bool PlayerActivejumpColliderCheck()
    {
        if (_playerJumpAttack.ActiveColliderCheck())
            return true;
        return false;
    }
}