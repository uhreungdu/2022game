using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditerManager : MonoBehaviour
{
    // 세이브 모드이면 화면이동을 할 수 없게 멈춤
    public bool SaveMode = false;
    // 이름 텍스트가 들어갈 자리
    public Text PathText;
    // 몇번 프리펩을 사용할 것인지
    public int Prefnum = 0;
    
    private static MapEditerManager instance;
    
    public static MapEditerManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<MapEditerManager>();
            if(instance == null)
            {
                GameObject container = new GameObject("MapEditerManager");
                instance = container.AddComponent<MapEditerManager>();
            }
        }
        return instance;
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
		
        }
        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SaveModeSetActive(bool mode)
    {
        SaveMode = mode;
    }
}
