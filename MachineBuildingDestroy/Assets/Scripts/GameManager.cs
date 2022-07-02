using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    private static GameManager instance;
    private int[] gamescore = new int[2];
    private int[] teamcount = new int[2];
    public bool gameStart { get; private set; }
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
        public bool gameSet;
        public float gameSetTime;
        public void SetTime(float val3){
            Ntimer = val3;
        }
        
        public void Active_Itembox(){
            if((int)Ntimer / 20 > 0 && (int)Ntimer % 20 < 10)
            {
                itembox_Create = true;
                // Debug.Log("아이템 생성");
            }
            else
            {
                itembox_Create = false;
                //Debug.Log("아이템 존재 X");
            }
        }
        public void Active_Goalopost() {
            if((int)Ntimer > 0 /* && (int)Ntimer % 60 < 20*/)
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
            if((int)Ntimer / 120 > 0)
            {
                landmakr_Create = true;
                //Debug.Log("아이템 생성");
            }
            else
            {
                landmakr_Create = false;
            }
        }

        public void Active_gameSet()
        {
            if((int)Ntimer / 180 > 0)
            {
                gameSetTime = Ntimer;
                gameSet = true;
                //Debug.Log("아이템 생성");
            }
            else
            {
                gameSet = false;
            }
        }
        
    }
    public Event_manager EManager = new Event_manager();
    public class temp_status
    {
        public float health { get; private set; }
        public item_box_make.item_type Item_num { get; private set; }
        public float Coin_Count{get; private set;}
        public float MaxHealth{get; private set;}
        public bool UIGiltch { get; private set; }

        public void setting(float hp, item_box_make.item_type Item, float coin, float m_Health,bool UIGil)
        {
            health = hp;
            Item_num = Item;
            Coin_Count = coin;
            MaxHealth = m_Health;
            UIGiltch = UIGil;
        }
    }

    public temp_status player_stat = new temp_status();
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
        gameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            now_timer.Ntimer += Time.deltaTime;
            if (EManager.gameSet && now_timer.Ntimer >= EManager.gameSetTime + 7)
            {
                PhotonNetwork.LoadLevel("GameResultScene");
                GameInfo gameInfo = GameInfo.GetInstance();
                for (int i = 0; i < 2; i++)
                {
                    gameInfo.Infomations.gamescore[i] = gamescore[i];
                }
                Destroy(UImanager.GetInstance().gameObject);
                Destroy(gameObject);
            }
        }
        
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
    public int setScore(int team, int point)
    {
        return gamescore[team] = point;
    }
    public int addScore(int team, int point)
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

    public void SetGameStart(bool start)
    {
        gameStart = start;
    }
   
    public timer_block getTime()
    {
        return now_timer;
    }
    public void Time_check()
    {
        if(now_timer.min < 5 && !EManager.gameSet)
        {
            // 내가 Master Client(동기화의 주체)이면 시간을 더해줍니다.
            now_timer.min = (int) now_timer.Ntimer / 60;
            now_timer.sec = ((int)now_timer.Ntimer - now_timer.min * 60) % 60;
            EManager.SetTime(now_timer.Ntimer);
            EManager.Active_Itembox();
            EManager.Active_Goalopost();
            EManager.Active_landmakr();
            EManager.Active_gameSet();
        }
        
        //Debug.Log(now_timer.min);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(EManager.Ntimer);
            stream.SendNext(gamescore[0]);
            stream.SendNext(gamescore[1]);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            now_timer.Ntimer = (float) stream.ReceiveNext();
            gamescore[0] = (int) stream.ReceiveNext();
            gamescore[1] = (int) stream.ReceiveNext();
        }
    }
}
