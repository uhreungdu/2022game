using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : LivingEntity
{
    public GameObject coinprefab;     // 생성할 코인의 원본 프리펩

    // Start is called before the first frame update
    void Start()
    {
        onDeath += DieAction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DieAction()
    {
        // 사망 상태를 참으로 변경
        dead = true;
        for (int i = 0; i < 10; ++i)
        {
            float radian = (Random.Range(0, 360)) * Mathf.PI / 180;
            Vector3 coinPosition = transform.position;
            coinPosition.x = coinPosition.x + (1.5f * Mathf.Cos(radian));
            coinPosition.z = coinPosition.z + (1.5f * Mathf.Sin(radian));
            /*GameObject coin = Instantiate(coinprefab, coinPosition, transform.rotation);
            Vector3 coinForward = coin.transform.position - transform.position;
            coinForward.Normalize();
            coin.GetComponent<Rigidbody>().AddForce(coinForward * 350);*/
        }
        gameObject.SetActive(false);
    }
}
