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
        public float sec;

        public void settimes(int val, float val2)
        {
            min = val;
            sec = val2;
        }
    }
    public timer_block now_timer;

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
}
