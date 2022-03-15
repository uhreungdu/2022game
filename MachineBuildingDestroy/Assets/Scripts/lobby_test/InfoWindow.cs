using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public GameObject Name;
    public GameObject Level;
    public GameObject Customize;
    public GameObject account;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        account = GameObject.Find("Account");
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
            Debug.Log(results);
            Name.GetComponent<Text>().text = GetStringDataValue(results, "character_name:");
            Level.GetComponent<Text>().text = GetStringDataValue(results, "character_level:");
            Customize.GetComponent<Text>().text = GetStringDataValue(results, "customize_1:") + " | "+ 
                                                  GetStringDataValue(results, "customize_2:");
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
