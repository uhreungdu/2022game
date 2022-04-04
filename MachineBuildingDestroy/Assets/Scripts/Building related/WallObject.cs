using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WallObject : LivingEntity
{
    public GameObject coinprefab;     // ������ ������ ���� ������
    public Material boxmaterial;
    public Rigidbody rigidbody;
    public int destroyfloor = 0;
    public int destroyTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        onDeath += DieAction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DieAction()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 10; ++i)
            {
                float radian = (Random.Range(0, 360)) * Mathf.PI / 180;
                Vector3 coinPosition = transform.position;
                coinPosition.x = coinPosition.x + (1.5f * Mathf.Cos(radian));
                coinPosition.z = coinPosition.z + (1.5f * Mathf.Sin(radian));
                coinPosition.y = coinPosition.y + 3.0f;
                GameObject coin =
                    PhotonNetwork.InstantiateRoomObject(coinprefab.name, coinPosition, transform.rotation);
                //Instantiate(coinprefab, coinPosition, transform.rotation);
                Vector3 coinForward = coin.transform.position - transform.position;
                coinForward.Normalize();
                // coin.GetComponent<Rigidbody>().AddExplosionForce(10, transform.position, 10f);
            }
        }

        GetComponent<MeshCollider>().enabled = false;
        
        Destroy(gameObject, 5);
    }

    public override void Die()
    {
        base.Die();
            // Rigidbody[] allChildren = GetComponentsInChildren<Rigidbody>();
            // rigidbody.constraints = RigidbodyConstraints.None;
            // foreach (Rigidbody child in allChildren)
            // {
            //     child.constraints = RigidbodyConstraints.None;
            //     Vector3 objectPotision = transform.position;
            //     objectPotision.y = 3;
            //     child.AddExplosionForce(250, objectPotision, 50f);
            // }
            // Destroy(GetComponent<PhotonRigidbodyView>());
            // Destroy(rigidbody);
    }
    [PunRPC]
    public void WallDestroy()
    {
        if (health <= startingHealth / 7f * 6 && destroyfloor <= 0)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1);
        }
        else if (health <= startingHealth / 7f * 5  && destroyfloor <= 1)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1);
        }
        else if (health <= startingHealth / 7f * 4  && destroyfloor <= 2)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1);
        }
        else if (health <= startingHealth / 7f * 3 && destroyfloor <= 3)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1);
        }
        else if (health <= startingHealth / 7f * 2  && destroyfloor <= 4)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1);
        }
        else if (health <= startingHealth / 7f * 1  && destroyfloor <= 5)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1);
        }

        if (dead)
        {
            Rigidbody[] allChildren = GetComponentsInChildren<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.None;
            foreach (Rigidbody child in allChildren)
            {
                child.constraints = RigidbodyConstraints.None;
                Vector3 objectPotision = transform.position;
                objectPotision.y = 3;
                // child.AddExplosionForce(250, objectPotision, 50f);
            }
            Destroy(GetComponent<PhotonRigidbodyView>());
            Destroy(rigidbody);
        }
    }
    [PunRPC]
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        photonView.RPC("SetObjectHealth",RpcTarget.Others, health, destroyfloor);
        photonView.RPC("RefreshHealth",RpcTarget.Others);
        photonView.RPC("WallDestroy",RpcTarget.All);
    }

    [PunRPC]
    public void RefreshHealth()
    {
        base.OnDamage(0);
    }

    public void NetworkOnDamage(float val)
    {
        photonView.RPC("OnDamage",RpcTarget.MasterClient,val);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {

        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {

        }
    }

    [PunRPC]
    public void SetObjectHealth(float f_health, int f_destroyfloor)
    {
        health = f_health;
        destroyfloor = f_destroyfloor;
    }

}
