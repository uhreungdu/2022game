using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulidingObject : LivingEntity
{
    public GameObject coinprefab; // ������ ������ ���� ������
    public int point; // 몇점인지
    public Material boxmaterial;
    public Rigidbody rigidbody;
    public MeshRenderer _MeshRenderer;
    public Collider _MeshCollider;

    public int destroyfloor = 0;
    public int destroyTime = 5;

    public float _reSpawnTime = 10.0f;
    protected float _reSpawnTimer = 0.0f;
    protected GameManager _Gamemanager;
    public float _ExplosionForce = 1000.0f;

    protected MeshRenderer[] childMeshRenderers;
    protected Collider[] childColliders;

    public GameObject effect_obj;
    public GameObject prefeb_effect;
    public Map _Map;

    public GameObject DestroyObjects;

    private List<Coroutine> Coroutines = new List<Coroutine>();

    public AudioSource _AudioSource;

    public AudioClip _AudioClip;

    public int MaxCut = 6;
    public bool ItIsLandMark = false;

    // Start is called before the first frame update
    protected void Start()
    {
        var objectName = gameObject.transform.root.name;
        objectName = objectName.Remove(objectName.Length - 7, 7);
        AddBuildingToServerEvent(photonView.ViewID, objectName, transform.position, transform.rotation,
            _reSpawnTime);


        rigidbody = GetComponentInChildren<Rigidbody>();
        _MeshRenderer = GetComponentInChildren<MeshRenderer>();
        _MeshCollider = GetComponentInChildren<Collider>();
        _Gamemanager = GameManager.GetInstance();
        onDeath += DieAction;
        GameObject _DestroyObject = new GameObject("DestroyObjects");
        _DestroyObject.transform.SetParent(transform, false);
        DestroyObjects = _DestroyObject;

        _AudioSource = GetComponent<AudioSource>();
        _AudioClip = Resources.Load<AudioClip>("Sounds/Boom");
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_Gamemanager == null)
        {
            
        }
        else if (!_Gamemanager.EManager.gameSet)
        {
            DeathTimer();
        }
    }

    public void DieAction()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < point; ++i)
            {
                float radian = ((360.0f / (point)) * i) * (float)(Math.PI / 180.0f);
                float radius = 5.0f;
                Vector3 coinPosition = transform.position;
                coinPosition.x = coinPosition.x + (radius * Mathf.Cos(radian));
                coinPosition.z = coinPosition.z + (radius * Mathf.Sin(radian));
                coinPosition.y = 5;
                GameObject coin =
                    PhotonNetwork.InstantiateRoomObject(coinprefab.name, coinPosition, coinprefab.transform.rotation);
                //Instantiate(coinprefab, coinPosition, transform.rotation);
                Vector3 explosionPosition = transform.position;
                coin.GetComponent<Rigidbody>().AddExplosionForce(500, explosionPosition, 10f, 500 / 2);
                coin.GetComponent<Rigidbody>().AddExplosionForce(500, explosionPosition, 10f);
            }
            var objectName = gameObject.transform.root.name;
            objectName = objectName.Remove(objectName.Length - 7, 7);
            if (GameManager.GetInstance().getTime().Ntimer < 180 - 45 && !ItIsLandMark)
            {
                _reSpawnTime = 45 - GameManager.GetInstance().getTime().Ntimer % 45;
                AddBuildingToServerEvent(photonView.ViewID, objectName, transform.position, transform.rotation,
                    _reSpawnTime);
            }
            else
            {
                AddBuildingToServerEvent(photonView.ViewID, objectName, transform.position, transform.rotation,
                    10);
            }
            BuildingDestroyEvent(photonView.ViewID);
        }
        _AudioSource.PlayOneShot(_AudioClip);
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

    public void HideBuildingFragments()
    {
        if (childMeshRenderers == null) return;
        if (childMeshRenderers.Length > 0)
        {
            childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var child in childMeshRenderers)
            {
                if (child != null && child != _MeshRenderer)
                    child.enabled = false;
            }
        }

        if (childColliders.Length > 0)
        {
            childColliders = GetComponentsInChildren<Collider>();
            foreach (var child in childColliders)
            {
                if (child != null && child != _MeshCollider)
                    child.enabled = false;
            }
        }
    }

    public void HideBuilding()
    {
        _MeshRenderer.enabled = false;
        _MeshCollider.enabled = false;
    }

    public void DeathTimer()
    {
        if (dead && Time.time > _reSpawnTimer + _reSpawnTime)
        {
            print("리스폰됨");
        }
    }

    [PunRPC]
    public override void Die()
    {
        base.Die();
        _reSpawnTimer = Time.time;
    }

    [PunRPC]
    public void WallDestroy()
    {
        if (health <= startingHealth / 7f * 6 && destroyfloor <= 0 && MaxCut > destroyfloor)
        {
            destroyfloor++;
            Coroutines.Add(StartCoroutine(Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1)));
        }

        if (health <= startingHealth / 7f * 5 && destroyfloor <= 1 && MaxCut > destroyfloor)
        {
            destroyfloor++;
            Coroutines.Add(StartCoroutine(Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1)));
        }

        if (health <= startingHealth / 7f * 4 && destroyfloor <= 2 && MaxCut > destroyfloor)
        {
            destroyfloor++;
            Coroutines.Add(StartCoroutine(Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1)));
        }

        if (health <= startingHealth / 7f * 3 && destroyfloor <= 3 && MaxCut > destroyfloor)
        {
            destroyfloor++;
            Coroutines.Add(StartCoroutine(Sliceseveraltimes(gameObject, Vector3.up, boxmaterial, 1)));
        }

        if (health <= startingHealth / 7f * 2 && destroyfloor <= 4 && MaxCut > destroyfloor)
        {
            destroyfloor++;
            Coroutines.Add(StartCoroutine(Sliceseveraltimes(gameObject, Vector3.right, boxmaterial, 1)));
        }

        if (health <= startingHealth / 7f * 1 && destroyfloor <= 5 && MaxCut > destroyfloor)
        {
            destroyfloor++;
            Coroutines.Add(StartCoroutine(Sliceseveraltimes(gameObject, Vector3.forward, boxmaterial, 1)));
        }

        if (dead)
        {
            if (Coroutines.Count > 0)
            {
                foreach (var coroutine in Coroutines)
                {
                    if (coroutine != null)
                        StopCoroutine(coroutine);
                }
                Coroutines.Clear();
            }
            childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var child in childMeshRenderers)
            {
                if (child != _MeshRenderer)
                    child.enabled = true;
            }

            childColliders = GetComponentsInChildren<Collider>();
            foreach (var child in childColliders)
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
                if (child.gameObject != gameObject)
                {
                    objectPotision.y = 1;
                    child.AddExplosionForce(_ExplosionForce, objectPotision, 40f, _ExplosionForce / 8.0f);
                    child.AddExplosionForce(_ExplosionForce, objectPotision, 40f);
                }
            }
            
            _MeshRenderer.enabled = false;
            _MeshCollider.enabled = false;
            effect_obj = Instantiate(prefeb_effect);
            effect_obj.transform.SetParent(gameObject.transform);
            effect_obj.transform.position = gameObject.transform.position;

            Destroy(GetComponent<PhotonRigidbodyView>());
            Destroy(rigidbody);
            CinemachineShake.Instance.ShakeCamera(25f, 0.5f);
        }
    }

    [PunRPC]
    public override void OnDamage(float damage)
    {
        SetObjectHealth(health - damage);
        base.OnDamage(0);
        WallDestroy();
    }

    [PunRPC]
    public void RefreshHealth()
    {
        base.OnDamage(0);
    }

    public void NetworkOnDamage(float val)
    {
        photonView.RPC("OnDamage", RpcTarget.AllViaServer, val);
    }

    [PunRPC]
    public void SetObjectHealth(float fHealth)
    {
        health = fHealth;
    }

    private void AddBuildingToServerEvent(int viewID, string type, Vector3 pos, Quaternion rotate, float respawnTime)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        byte evCode = (byte) NetworkManager.EventCode.CreateBuildingFromClient;
        object[] data = new object[]
            {viewID, type, pos.x, pos.y, pos.z, rotate.x, rotate.y, rotate.z, rotate.w, respawnTime};
        RaiseEventOptions RaiseOpt = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient};
        SendOptions sendOpt = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    private void BuildingDestroyEvent(int viewID)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        byte evCode = (byte) NetworkManager.EventCode.DestroyBuildingFromClient;
        object[] data = new object[] {viewID};
        RaiseEventOptions RaiseOpt = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient};
        SendOptions sendOpt = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }
    
    public IEnumerator Sliceseveraltimes(GameObject _target, Vector3 _sliceNormal, Material _interial, int _number)
    {
        int corutineCount = 0;
        for (int i = 0; i < _number; ++i)
        {
            BulidingObject _bulidingObject = _target.GetComponent<BulidingObject>();
            Transform _DestroyObjecttransform = _bulidingObject.DestroyObjects.transform;

            if (_DestroyObjecttransform == null || _DestroyObjecttransform.childCount <= 0)
            {
                CMeshSlicer.SlicerWorld(_target.gameObject, _sliceNormal, _target.GetComponent<MeshRenderer>().bounds.center,
                    _interial);
                corutineCount++;
            }
            else
            {
                if (_DestroyObjecttransform != null)
                {
                    Transform[] allChildren = _DestroyObjecttransform.GetComponentsInChildren<Transform>();
                    foreach (Transform child in allChildren)
                    {
                        if (child != _DestroyObjecttransform && child != null)
                        {
                            MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();
                            if (childMeshRenderer != null)
                            {
                                CMeshSlicer.SlicerWorld(child.gameObject, _sliceNormal, childMeshRenderer.bounds.center, _interial);
                                corutineCount++;
                            }
                        }
                        if (corutineCount % 3 == 0)
                            yield return null;
                    }
                }
            }
        }
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (rigidbody != null)
                rigidbody.isKinematic = true;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (rigidbody != null)
                rigidbody.isKinematic = false;
        }
    }
    
    [PunRPC]
    void NetWorkPlayOneShot(AudioClip audioClip)
    {
        _AudioSource.PlayOneShot(audioClip);
    }
}