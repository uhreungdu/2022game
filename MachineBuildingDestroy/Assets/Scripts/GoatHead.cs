using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoatHead : MonoBehaviourPun
{
    private GameManager _gameManager;
    private SkinnedMeshRenderer _meshRenderer;
    
    public Animator _Animator;
    public List<AnimationClip> _AnimationClips;
    public Map _Map;
    public int XorY;
    public float[] MinMaxX = new float[2];
    public float[] MinMaxZ = new float[2];
    
    private Vector3 originTransform;
    private float StartY = -60;
    private float appearY = 0;

    public bool appear;
        // Start is called before the first frame update
    void Start()
    {
        if (_Map == null)
        {
            // 부하가 많은 작업 손으로 집어넣지 않았을 경우 방지
            _Map = GameObject.Find("Map").GetComponent<Map>();
        }
        _Animator = GetComponent<Animator>();
        _gameManager = GameManager.GetInstance();
        

        appear = false;
        
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_gameManager == null)
        {
            
        }
        else if (!_gameManager.EManager.gameSet) 
        {
            OnEvent();
            AppearGoatHead();
        }
    }
    private void OnEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_gameManager.EManager.goatheavyrain_Create)
        {
            if (appear == false)
            {
                // 저새상코딩
                MinMaxX[0] = _Map.MaximumX + 150;
                MinMaxX[1] = _Map.MinimumX - 150;
                MinMaxZ[0] = _Map.MaximumZ + 150;
                MinMaxZ[1] = _Map.MinimumZ - 150;
                
                XorY = Random.Range(0, 2);
                if (XorY == 0)
                {
                    int randomInt = Random.Range(0, 2);
                    originTransform = new Vector3(MinMaxX[randomInt], StartY,
                        (_Map.MaximumZ + _Map.MinimumZ) / 2f);
                    if (randomInt == 0)
                        transform.rotation *= Quaternion.Euler(0, 1 * -90, 0);
                    else
                        transform.rotation *= Quaternion.Euler(0, 1 * 90, 0);
                }
                else
                {
                    int randomInt = Random.Range(0, 2);
                    originTransform = new Vector3((_Map.MaximumX + _Map.MinimumX) / 2f, StartY,
                        MinMaxZ[randomInt]);
                    if (randomInt == 0)
                        transform.rotation *= Quaternion.Euler(0, 1 * -180, 0);
                }

                if (!_meshRenderer.enabled)
                    _meshRenderer.enabled = true;
                
                if (PhotonNetwork.IsMasterClient)
                    _Animator.SetBool("State", RandomBool());
                
                appearY = 0;
                appear = true;
                Vector3 position = originTransform;
                transform.position = position;
            }
        }
        else
        {
            if (appear == false)
            {
                if (_meshRenderer.enabled)
                    _meshRenderer.enabled = false;
            }
        }
    }
    
    void AppearGoatHead()
    {
        if (_gameManager.EManager.goatheavyrain_Create)
        {
            if (appearY <= -StartY + 30 && appear)
            {
                Vector3 position = originTransform;
                appearY += Time.deltaTime * (-StartY + 30 / 3.0f);
                position.y += appearY;
                transform.position = position;
            }
        }
        else if (!_gameManager.EManager.goatheavyrain_Create)
        {
            if (appearY >= 0 && appear)
            {
                Vector3 position = originTransform;
                appearY -= Time.deltaTime * (-StartY + 30 / 1.5f);
                position.y += appearY;
                transform.position = position;
            }
            else
            {
                appear = false;
            }
        }
    }

    bool RandomBool()
    {
        int Randomint = Random.Range(0, 2);
        if (Randomint <= 0)
            return false;
        else
            return true;
    }
}
