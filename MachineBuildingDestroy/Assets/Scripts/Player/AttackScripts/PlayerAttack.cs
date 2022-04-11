using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected string _attackName; // 공격 이름
    protected float _damage; // 공격력
    
    protected float _timeBetAttack; // 공격 간격
    protected float _activeAttackTime; // 공격 유지 시간
    protected float _lastAttackTime; // 공격을 마지막에 한 시점
    
    public bool keepCollideractive { get; private set; } // 히트박스가 켜지고 꺼지는것을 제어하는 함수

    public List<BoxCollider> _hitBoxColliders; // 히드박스가 공격에 필요한개 하나가 아닐 수 있음
    
    // 유니티 애니메이션에서 제어할 함수
    public void SetKeepActiveAttack(int set)
    {
        if (set >= 1)
            keepCollideractive = true;
        else if (set < 1)
            keepCollideractive = false;
    }
}
