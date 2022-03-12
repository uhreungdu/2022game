using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager instance;
    private int[] gamescore = new int[2];
    private int[] teamcount = new int[2];
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
        public int sec;
        public float Ntimer;
    }
    public timer_block now_timer;
    public class Event_manager{
        public float Ntimer;
        public bool itembox_Create;
        public bool goalpost_Create;
        public bool landmakr_Create;
        public void SetTime(float val3){
            Ntimer = val3;
        }
        public void Active_Itembox(){
            if((int)Ntimer / 90 > 0 && (int)Ntimer % 90 < 10)
            {
                itembox_Create = true;
                //Debug.Log("아이템 생성");
            }
            else
            {
                itembox_Create = false;
                //Debug.Log("아이템 존재 X");
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
        //now_timer.settimes(0,0,0f);
        now_timer.min = 0;
        now_timer.sec = 0;
        now_timer.Ntimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<2;++i)
        {
            if(gamescore[i] <= 5)
            {
                Time_check();
            }
        }
        
    }
    public int getScore(int num){
        return gamescore[num];
    }
    public int plusScore(int team, int point)
    {
        return gamescore[team] += point;
    }
    public int getTeamcount(int num)
    {
        return teamcount[num];
    }
    public int addTeamcount(int team)
    {
        return teamcount[team] += 1;
    }
    public int setScore(int team, int point)
    {
        return gamescore[team] = point;
    }
    public timer_block getTime()
    {
        return now_timer;
    }
    public void Time_check()
    {
        if(now_timer.min < 5)
        {
            now_timer.Ntimer += Time.deltaTime * 0.5f;
            now_timer.min = (int) now_timer.Ntimer / 60;
            now_timer.sec = ((int)now_timer.Ntimer - now_timer.min * 60) % 60;
            EManager.SetTime(now_timer.Ntimer);
            EManager.Active_Itembox();
            EManager.Active_Goalopost();
            EManager.Active_landmakr();
        }
        
        Debug.Log(now_timer.min);
    }
}
