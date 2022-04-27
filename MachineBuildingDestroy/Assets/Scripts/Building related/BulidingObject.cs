using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulidingObject : LivingEntity, IPunObservable
{
    public GameObject coinprefab;     // ������ ������ ���� ������
    public int point;                // 몇점인지
    public Material boxmaterial;
    public Rigidbody rigidbody;
    public MeshRenderer _MeshRenderer;
    public MeshCollider _MeshCollider;
    
    public int destroyfloor = 0;
    public int destroyTime = 5;
    
    public float _reSpawnTime = 10.0f;
    private float _reSpawnTimer = 0.0f;

    public float _ExplosionForce = 1000.0f;

    private MeshRenderer[] childMeshRenderers;
    private MeshCollider[] childMeshCollider;

    public GameObject effect_obj;
    public GameObject prefeb_effect;

    // Start is called before the first frame update
    protected void Start()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
        _MeshRenderer = GetComponentInChildren<MeshRenderer>();
        _MeshCollider = GetComponentInChildren<MeshCollider>();
        onDeath += DieAction;
    }

    // Update is called once per frame
    protected void Update()
    {
        DeathTimer();
    }

    public void DieAction()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < point; ++i)
            {
                float radian = (Random.Range(0, 360)) * Mathf.PI / 180;
                float radius = Random.value * 3.0f;
                Vector3 coinPosition = transform.position;
                coinPosition.x = coinPosition.x + (radius * Mathf.Cos(radian));
                coinPosition.z = coinPosition.z + (radius * Mathf.Sin(radian));
                coinPosition.y = coinPosition.y;
                GameObject coin =
                    PhotonNetwork.InstantiateRoomObject(coinprefab.name, coinPosition, coinprefab.transform.rotation);
                //Instantiate(coinprefab, coinPosition, transform.rotation);
                Vector3 explosionPosition = transform.position;
                coin.GetComponent<Rigidbody>().AddExplosionForce(_ExplosionForce, explosionPosition, 10f, _ExplosionForce / 2);
                coin.GetComponent<Rigidbody>().AddExplosionForce(_ExplosionForce, explosionPosition, 10f);
            }
            Invoke("Net_HideBuilding", destroyTime);
            Invoke("RespawnBuilding", _reSpawnTime);
        }
        // Destroy(gameObject, destroyTime);
    }

    void RespawnBuilding()
    {
        string objectName;
        if (gameObject.transform.parent != null)
        {
            objectName = gameObject.transform.parent.name;
        }
        else
            objectName = gameObject.name;
        objectName = objectName.Remove(objectName.Length - 7, 7);
        Vector3 position = transform.position;
        position.y += 30;
        PhotonNetwork.InstantiateRoomObject(objectName, position, transform.rotation);
        print("리스폰진짜됨");
        PhotonNetwork.Destroy(gameObject);
    }

    void Net_HideBuilding()
    {
        photonView.RPC("HideBuilding",RpcTarget.All);
    }

    [PunRPC]
    void HideBuilding()
    {
        if (childMeshRenderers.Length > 0)
        {
            childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var child in childMeshRenderers)
            {
                if (child != null && child != _MeshRenderer)
                    child.enabled = false;
            }
        }

        if (childMeshCollider.Length > 0)
        {
            childMeshCollider = GetComponentsInChildren<MeshCollider>();
            foreach (var child in childMeshCollider)
            {
                if (child != null && child != _MeshCollider)
                    child.enabled = false;
            }
        }
    }

    public void DeathTimer()
    {
        if (dead && Time.time > _reSpawnTimer + _reSpawnTime)
        {
            print("리스폰됨");
        }
    }
    
    public override void Die()
    {
        base.Die();
        _reSpawnTimer = Time.time;
    }
    
    [PunRPC]
    public void WallDestroy()
    {
        if (health <= startingHealth / 7f * 6 && destroyfloor <= 0)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1);
        }
        if (health <= startingHealth / 7f * 5  && destroyfloor <= 1)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1);
        }
        if (health <= startingHealth / 7f * 4  && destroyfloor <= 2)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1);
        }
        if (health <= startingHealth / 7f * 3 && destroyfloor <= 3)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1);
        }
        if (health <= startingHealth / 7f * 2  && destroyfloor <= 4)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1);
        }
        if (health <= startingHealth / 7f * 1  && destroyfloor <= 5)
        {
            destroyfloor++;
            CMeshSlicer.Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1);
        }

        if (dead)
        {
            childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var child in childMeshRenderers)
            {
                if (child != _MeshRenderer)
                    child.enabled = true;
            }
            
            childMeshCollider = GetComponentsInChildren<MeshCollider>();
            foreach (var child in childMeshCollider)
            {
                if (child != _MeshRenderer)
                    child.enabled = true;
            }

            Rigidbody[] childRigidbodys = GetComponentsInChildren<Rigidbody>();
            //rigidbody.constraints = RigidbodyConstraints.None;
            foreach (Rigidbody child in childRigidbodys)
            {
                child.constraints = RigidbodyConstraints.None;
                Vector3 objectPotision = transform.position;
                objectPotision.y = 1;
                child.AddExplosionForce(_ExplosionForce, objectPotision, 40f, _ExplosionForce / 2.0f);
                child.AddExplosionForce(_ExplosionForce, objectPotision, 40f);
            }

            _MeshRenderer.enabled = false;
            _MeshCollider.enabled = false;
            effect_obj = Instantiate(prefeb_effect);
            effect_obj.transform.SetParent(gameObject.transform);
            effect_obj.transform.Translate(gameObject.transform.position - Vector3.back);
            Destroy(GetComponent<PhotonRigidbodyView>());
            Destroy(rigidbody);
        }
    }
    [PunRPC]
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        //photonView.RPC("SetObjectHealth",RpcTarget.Others, health, destroyfloor);
        //photonView.RPC("RefreshHealth",RpcTarget.Others);
        //photonView.RPC("WallDestroy",RpcTarget.All);
        WallDestroy();
    }

    [PunRPC]
    public void RefreshHealth()
    {
        base.OnDamage(0);
    }

    public void NetworkOnDamage(float val)
    {
        photonView.RPC("OnDamage",RpcTarget.All,val);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(_reSpawnTime);
            stream.SendNext(_reSpawnTimer);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            _reSpawnTime = (float)stream.ReceiveNext();
            _reSpawnTimer = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SetObjectHealth(float f_health, int f_destroyfloor)
    {
        health = f_health;
        destroyfloor = f_destroyfloor;
    }

}
