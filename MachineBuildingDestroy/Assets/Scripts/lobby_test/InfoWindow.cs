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
        if (!this.gameObject.activeSelf) return;
        if (this.transform.localPosition.y > 0)
        {
            this.transform.Translate(0, -1600.0f * Time.deltaTime, 0);
        }
        else if(this.transform.localScale.x<=0.4166667f)
        {
            this.transform.localScale += new Vector3(0.4166667f * Time.deltaTime * 4.0f, 0, 0);
        }
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
