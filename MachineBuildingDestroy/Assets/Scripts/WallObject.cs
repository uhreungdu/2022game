using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : LivingEntity
{
    public GameObject coinprefab;     // ������ ������ ���� ������
    public Material boxmaterial;
    public int destroyfloor = 0;
    public int destroyTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        onDeath += DieAction;
    }

    // Update is called once per frame
    void Update()
    {
        WallDestroy();
    }

    public void DieAction()
    {
        CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1);
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
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        { 
            child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        GetComponent<MeshCollider>().enabled = false;
        // GetComponent<Rigidbody>().AddForce(Vector3.up * 2000);
        Destroy(GetComponent<Rigidbody>());

        //for (int i = 0; i < transform.childCount; ++i)
        //{
        //    transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //}



        // gameObject.SetActive(false);
        }

    private void WallDestroy()
    {
        if (health <= startingHealth / 3f * 2 && destroyfloor <= 0)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1);
        }
        else if (health <= startingHealth / 3f  && destroyfloor <= 1)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1);
        }
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
