using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : PlayerAttack
{
    public PlayerImpact _playerImpact;
    public BoxCollider _hammerBoxCollider;
    public GameObject RHandGameObject;
    private Thirdpersonmove _thirdpersonmove;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _attackName = "해머기본공격";
        _lastColliderActiveTime = 0.4f; // 공격 유지 시간
        SetHammerAffterCast(0);
        _damage = 25;
        
        _hitBoxColliders.Add(_hammerBoxCollider);
    }

    private void Update()
    {
        AfterCastRecovery();
        ActiveAttack();
    }

    public void ActiveAttack()
    {
        if (_hitBoxColliders[0].enabled && ActiveColliderCheck())
        {
            return;
        }
        else if (_hitBoxColliders[0].enabled)
        {
            SetHammerCollision(0);
        }
    }
    public void SetHammerCollision(int set)
    {
        if (set > 0)
        {
            _hitBoxColliders[0].enabled = true;
            SetActiveAttack();
            _lastColliderOnTime = Time.time;
        }
        else if (set <= 0)
        {
            _hitBoxColliders[0].enabled = false;
            // print("해머 콜라이더 지속시간 " + (_lastColliderActiveTime + _lastColliderOnTime - Time.time) + "초");
        }
    }
    public void SetHammerAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }
}
