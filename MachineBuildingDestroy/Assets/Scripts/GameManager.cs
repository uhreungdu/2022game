using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager instance;
    private int[] gamescore = new int[2];
    public static GameManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<GameManager>();
            if(instance == null)
            {
                GameObject container = new GameObject("GameManager");
                instance = container.AddComponent<GameManager>();
            }
        }
        return instance;
    }
    public float time_display;
    public struct timer_block{
        public int min;
        public float sec;
        public float Ntimer;
        public void settimes(int val, float val2, float val3)
        {
            min = val;
            sec = val2;
            Ntimer = val3;
        }
    }
    public timer_block now_timer;
    public class Event_manager{
        public int min;
        public float sec;
        public float Ntimer;
        public bool itembox_Create;
        public bool goalpost_Create;
        public bool landmakr_Create;
        public void SetTime(int val, float val2, float val3){
            min = val;
            sec = val2;
            Ntimer = val3;
        }
        public void Active_Itembox(){
            if((int)Ntimer / 90 > 0 && (int)Ntimer % 90 < 20)
            {
                itembox_Create = true;
                Debug.Log("아이템 생성");
            }
            else
            {
                itembox_Create = false;
                //Debug.Log(Ntimer);
            }
        }
        public void Active_Goalopost(){
            if((int)Ntimer / 60 > 0 && (int)Ntimer % 60 < 20)
            {
                goalpost_Create = true;
                //Debug.Log("아이템 생성");
            }
            else
            {
                goalpost_Create = false;
                //Debug.Log(Ntimer);
            }
        }
        public void Active_landmakr(){
            if((int)Ntimer / 240 > 0)
            {
                landmakr_Create = true;
                //Debug.Log("아이템 생성");
            }
            else
            {
                landmakr_Create = false;
            }
        }
        
    }
    public Event_manager EManager = new Event_manager();
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
        for(int i = 0;i<2;++i)
        {
            gamescore[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<2;++i)
        {
            if(gamescore[i] <= 5)
            {
                time_display += Time.deltaTime;
            }
            else
            {
                time_display = 0;
            }
        }
        
    }
    public int getScore(int num){
        return gamescore[num];
    }
    public int setScore(int team, int point)
    {
        return gamescore[team] += point;
    }
    public timer_block getTime()
    {
        return now_timer;
    }
}
