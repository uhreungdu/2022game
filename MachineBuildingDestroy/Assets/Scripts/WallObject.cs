using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    public GameObject coinprefab;     // 생성할 탄약의 원본 프리펩
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
        for (int i = 0; i < 10; ++i)
        {
            float radian = ((360f / 10f) * i) * Mathf.PI / 180;
            Vector3 coinPosition = transform.position;
            coinPosition.x = coinPosition.x + (1.5f * Mathf.Cos(radian));
            coinPosition.z = coinPosition.z + (1.5f * Mathf.Sin(radian));
            GameObject coin = Instantiate(coinprefab, coinPosition, transform.rotation);
            Vector3 coinForward = coin.transform.position - transform.position;
            coinForward.Normalize();
            coin.GetComponent<Rigidbody>().AddForce(coinForward*500);
        }
        gameObject.SetActive(false);
    }
}
