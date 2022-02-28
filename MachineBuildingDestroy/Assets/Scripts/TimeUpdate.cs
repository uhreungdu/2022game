using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public GameObject TimeDisplay;
    private Text Timetext;
    int min;
    float secs;
    float time_temp;
    void Start()
    {
        gManager = GameManager.GetInstance();
        Timetext = TimeDisplay.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Time_check();
    }
    void Time_check()
    {
        if(gManager.now_timer.min< 5)
        {
            Timetext.text = string.Format("{0:D1} : {1:D2}",gManager.now_timer.min,(int)gManager.now_timer.sec);
        }
        else
        {
            Timetext.text = "Time up!";
            Debug.Log(gManager.now_timer.min);
        }
    }
}
