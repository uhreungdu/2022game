using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float hp = 100f;
    public bool dead { get; protected set; } // ��� ����
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

        // ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (hp <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // ��� ���¸� ������ ����
        dead = true;
    }
}
