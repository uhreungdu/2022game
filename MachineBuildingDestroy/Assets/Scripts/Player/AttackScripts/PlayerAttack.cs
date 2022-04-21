using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected string _attackName; // 공격 이름
    public float _damage { protected set; get; } // 공격력
    
    protected float _aftercastAttack; // 공격 후딜레이
    protected float _activeAttackTime; // 공격 유지 시간
    protected float _lastAttackTime; // 공격을 마지막에 한 시점
    
    public bool aftercast { get; private set; } // 움직일 수 없는 시간

    public List<BoxCollider> _hitBoxColliders; // 히드박스가 공격에 필요한개 하나가 아닐 수 있음
    
    protected float _lastColliderOnTime; // 콜라이더가 켜진 시점
    protected float _lastColliderActiveTime; // 공격 켜져있는 시간

    public void AfterCastRecovery()
    {
        if (Time.time >= _lastAttackTime + _aftercastAttack && aftercast)
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
            _lastAttackTime = Time.time;
            aftercast = true;
        }
        else if (set < 1)
        {
            aftercast = false;
        }
    }
    
    public void SetActiveAttack()
    {
        _lastColliderOnTime = Time.time;
    }
}
