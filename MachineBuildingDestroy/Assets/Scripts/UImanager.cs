using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    private static UImanager instance;
    public GameManager gmanager;
    public List<GameObject> Score_UI = new List<GameObject>();
    public GameObject Canvas;
    public static UImanager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<UImanager>();
            if(instance == null)
            {
                GameObject container = new GameObject("UIManager");
                instance = container.AddComponent<UImanager>();
            }
        }
        return instance;
    }
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if( instance != this)
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
        DontDestroyOnLoad(instance);
        gmanager = GameManager.GetInstance();
        Score_UI.Add(GameObject.Find("Team_Blue_Score"));
        Score_UI.Add(GameObject.Find("Team_Red_Score"));
        Canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameScore();
    }
    void UpdateGameScore()
    {
        for(int i = 0; i<2; ++i)
        {
            Score_UI[i].GetComponent<ScoreText>().UpdateScore(gmanager.getScore(i));
        }
    }
}
