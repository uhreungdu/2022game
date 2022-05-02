using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public GameObject playerName;
    public GameObject level;
    public int costume;
    public GameObject gameResults;
    public GameObject account;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        Vector3 initPos = new Vector3(0, 400.0f, 0);
        Vector3 initSca = new Vector3(0, 0.4166667f, 0.4166667f);
        this.transform.localPosition = initPos;
        this.transform.localScale = initSca;
    }

    private void Awake()
    {
        account = GameObject.Find("Account");
        StartCoroutine(GetPlayerInfo());
    }

    public IEnumerator GetPlayerInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + account.GetComponent<Account>().GetPlayerID() + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/get_player_info.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            playerName.GetComponent<Text>().text = GetStringDataValue(results, "name:");

            var win = GetIntDataValue(results, "win:");
            var lose = GetIntDataValue(results, "lose:");
            gameResults.GetComponent<Text>().text = "총 게임 수: " + (win + lose) + "\n"
                +"승리: " + win + "\n"
                +"패배: " + lose;
            //Level.GetComponent<Text>().text = GetStringDataValue(results, "level:");
             
            costume = GetIntDataValue(results, "costume:");

        }
    }
    string GetStringDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    int GetIntDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return int.Parse(value);
    }
    
}
