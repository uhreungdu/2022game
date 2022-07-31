using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    private static UImanager instance;
    public GameManager gmanager;
    public GameObject Score;
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
            Canvas = GameObject.Find("Canvas");
            Score = Canvas.transform.Find("Score").gameObject;
            Score_UI.Add(Score.transform.Find("TeamRedScore").gameObject);
            Score_UI.Add(Score.transform.Find("TeamBlueScore").gameObject);
            Score_UI.Add(Score.transform.Find("ScoreCenter").gameObject);
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
        //DontDestroyOnLoad(instance);
        gmanager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameScore();
    }
    void UpdateGameScore()
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        int[] teamScore = new int[2];
        float[] teamScoreRatio = new float[2];
        
        for(int i = 0; i < 2; ++i)
        {
            teamScore[i] += gmanager.getScore(i);
        }
        
        for(int i = 0; i < 2; ++i)
        {
            if ((teamScore[0] + teamScore[1]) == 0)
            {
                teamScoreRatio[i] = 0.5f;
            }
            else
                teamScoreRatio[i] = teamScore[i] / (float)(teamScore[0] + teamScore[1]);
            Score_UI[i].GetComponent<Image>().fillAmount = teamScoreRatio[i];
        }
        Vector3 CenterPosition = new Vector3((teamScoreRatio[0] * 1500) - 750, Score_UI[2].transform.localPosition.y, Score_UI[2].transform.localPosition.z);
        Score_UI[2].transform.localPosition = CenterPosition;
    }
}
