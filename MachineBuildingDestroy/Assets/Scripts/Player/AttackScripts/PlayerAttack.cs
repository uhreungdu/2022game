using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerState _playerState;
    protected string _attackName; // 공격 이름
    public float _damage { protected set; get; } // 공격력
    
    protected float _aftercastAttack; // 공격 후딜레이
    
    public bool aftercast { get; private set; } // 움직일 수 없는 시간

    public List<BoxCollider> _hitBoxColliders; // 히드박스가 공격에 필요한개 하나가 아닐 수 있음
    
    protected float _lastColliderOnTime; // 콜라이더가 켜진 시점
    protected float _lastColliderActiveTime; // 공격 켜져있는 시간
    
    public void AfterCastRecovery()
    {
        if (Time.time >= _playerState._lastAttackTime + _playerState._aftercastAttack && _playerState.aftercast)
        {
            SetAffterCast(0);
        }
    }
    public bool ActiveColliderCheck()
    {
        if (Time.time >= _lastColliderOnTime + _lastColliderActiveTime)
        {
            return false;
        }
        return true;
    }
    
    // 유니티 애니메이션에서 제어할 함수
    public void SetAffterCast(int set)
    {
        if (set >= 1)
        {
            _playerState._lastAttackTime = Time.time;
            _playerState._aftercastAttack = _aftercastAttack;
            _playerState.aftercast = true;
        }
        else if (set < 1)
        {
            _playerState.aftercast = false;
        }
    }
    
    public void SetActiveAttack()
    {
        _lastColliderOnTime = Time.time;
    }
}
