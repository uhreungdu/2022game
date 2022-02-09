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
    float time_display;
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
				// 인스턴스가 이미 존재하며, 이 인스턴스가 나 자신이 아니라면
				// 즉 이미 다른 누군가가 인스턴스 자리에 있다면
				// 첫번쨰 이후에 만들어진, 즉 자신을 스스로 제거한다.
				Destroy(gameObject);
			}
		
		}
        //씬 옮길때 해당 오브젝트 삭제하지 않도록 하는 것
        DontDestroyOnLoad(instance);
        for(int i = 0;i<2;++i)
        {
            gamescore[i] = 0;
            //Debug.Log("값들어감?");
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
    public float getTime()
    {
        return time_display;
    }
}
