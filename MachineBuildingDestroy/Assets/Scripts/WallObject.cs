using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    public GameObject coinprefab;     // ������ ź���� ���� ������
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
