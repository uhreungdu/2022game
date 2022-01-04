using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public GameObject Score_Text;
    int now_score;
    void Start()
    {
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int num)
    {
        Score_Text.GetComponent<Text>().text = "" + num;
    }
}
