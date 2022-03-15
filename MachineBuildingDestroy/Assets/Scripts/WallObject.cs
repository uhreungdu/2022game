using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : LivingEntity
{
    public GameObject coinprefab;     // ������ ������ ���� ������
    public Material boxmaterial;
    public int destroyTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        onDeath += DieAction;
    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
    }

    public void DieAction()
    {
        for (int i = 0; i < 10; ++i)
        {
            float radian = (Random.Range(0, 360)) * Mathf.PI / 180;
            Vector3 coinPosition = transform.position;
            coinPosition.x = coinPosition.x + (1.5f * Mathf.Cos(radian));
            coinPosition.z = coinPosition.z + (1.5f * Mathf.Sin(radian));
            GameObject coin = Instantiate(coinprefab, coinPosition, transform.rotation);
            Vector3 coinForward = coin.transform.position - transform.position;
            coinForward.Normalize();
        }
        // for (int i = 0; i < 2; ++i)
        // {
        //     Transform[] allChildren = GetComponentsInChildren<Transform>();
        //     foreach (Transform child in allChildren)
        //     {
        //         child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //     }
        // }
        GetComponent<MeshCollider>().enabled = false;
        // GetComponent<Rigidbody>().AddForce(Vector3.up * 2000);
        Destroy(GetComponent<Rigidbody>());
        CMeshSlicer.Sliceseveraltimes(gameObject, boxmaterial, 1);

        //for (int i = 0; i < transform.childCount; ++i)
        //{
        //    transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //}



        // gameObject.SetActive(false);
        }

    private void OnCollisionEnter(Collision collision)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        // if (collision.gameObject.tag == gameObject.tag)
        // {
        //     // Debug.Log("������ �浹 ����");
        //     //gameObject.transform.localScale += new Vector3(0.3f, 0, 0.3f);
        //     //Destroy(collision.collider.gameObject);
        //     Destroy(gameObject);
        // }
    }
}
