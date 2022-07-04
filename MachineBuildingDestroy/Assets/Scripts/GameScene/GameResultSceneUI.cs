using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultSceneUI : MonoBehaviour
{
    public GameObject GameTotalPointInfomation;
    public GameObject PlayerInfomation;
    // Start is called before the first frame update
    void Start()
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        GameInfo gameInfo = GameInfo.GetInstance();
        
        GameTotalPointInfomation.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(gameInfo.Infomations.gamescore[0]);
        GameTotalPointInfomation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToString(gameInfo.Infomations.gamescore[1]);
        if (gameInfo.Infomations.gamescore[0] > gameInfo.Infomations.gamescore[1])
        {
            GameTotalPointInfomation.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                "Victory!";
            GameTotalPointInfomation.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
                "Defeat";
        }
        else if (gameInfo.Infomations.gamescore[0] < gameInfo.Infomations.gamescore[1])
        {
            GameTotalPointInfomation.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                "Defeat";
            GameTotalPointInfomation.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
                "Victory!";
        }
        else
        {
            GameTotalPointInfomation.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                "Draw";
            GameTotalPointInfomation.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
                "Draw";
        }

        for (int i = 0; i < 6; ++i)
        {
            // 0 닉네임 1 킬 2 데스 3 코인
            if (myInRoomInfo.Infomations[i].Name != "")
            {
                PlayerInfomation.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    myInRoomInfo.Infomations[i].Name;
                PlayerInfomation.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    Convert.ToString(myInRoomInfo.Infomations[i].TotalKill);
                PlayerInfomation.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text =
                    Convert.ToString(myInRoomInfo.Infomations[i].TotalDeath);
                PlayerInfomation.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text =
                    Convert.ToString(myInRoomInfo.Infomations[i].TotalGetPoint);
            }
            else
            {
                PlayerInfomation.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                PlayerInfomation.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                PlayerInfomation.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                PlayerInfomation.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public static void SceneChange(String Scenename)
    {
        SceneManager.LoadScene(Scenename);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
