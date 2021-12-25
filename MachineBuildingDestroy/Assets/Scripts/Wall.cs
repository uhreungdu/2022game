using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float hp = 100f;
    public bool dead { get; protected set; } // 사망 상태
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDamage(float damage)
    {
        hp -= damage;

        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (hp <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // 사망 상태를 참으로 변경
        dead = true;
    }
}
