using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttack : PlayerAttack
{
    // Start is called before the first frame update
    public PlayerState _playerState;
    public PlayerImpact _playerImpact;
    public BoxCollider _rHandBoxCollider;
    public GameObject LHandGameObject;
    public GameObject RHandGameObject;
    void Start()
    {
        _playerState = transform.GetComponent<PlayerState>();
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _attackName = "기본공격";
        _aftercastAttack = 0f; // 후딜레이
        _activeAttackTime = 0.4f; // 공격 유지 시간
        _lastAttackTime = 0f; // 공격을 마지막에 한 시점
        SetAffterCast(0);
        _damage = 15;
        
        _hitBoxColliders.Add(_rHandBoxCollider);
    }

    void Update()
    {
        HandTransform();
        AfterCaseRecovery();
    }
    
    public void SetRHandCollision(int set)
    {
        if (set > 0)
            _hitBoxColliders[0].enabled = true;
        else if (set <= 0)
            _hitBoxColliders[0].enabled = false;
    }

    private void HandTransform()
    {
        Vector3 WorldRHandPosition = RHandGameObject.transform.position;
        _rHandBoxCollider.transform.position = WorldRHandPosition;
    }
    
    public void AttackMovement()
    {
        Transform rootTransform = transform.root;
        _playerImpact.AddImpact(rootTransform.forward, 10);
    }
    
    public void SetAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }
}
