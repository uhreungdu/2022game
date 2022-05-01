using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class AccountCheckButton : MonoBehaviour
{
    public GameObject idInput;
    public GameObject resultText;
    public GameObject nameInput;
    public GameObject pwInput;

    private bool _working = false;
    private bool _ok = false;

    public void ResetValues()
    {
        _working = false;
        _ok = false;
        idInput.GetComponent<InputField>().text = "";
        idInput.GetComponent<InputField>().interactable = true;
        nameInput.GetComponent<InputField>().text = "";
        nameInput.GetComponent<InputField>().interactable = true;
        pwInput.GetComponent<InputField>().text = "";
        pwInput.GetComponent<InputField>().interactable = false;
        resultText.GetComponent<Text>().text = "";
    }

    public bool GetOK()
    {
        return _ok;
    }
    
    private void OnDisable()
    {
        ResetValues();
    }
    
    void Update()
    {
        if (_ok)
        {
            GetComponent<Button>().interactable = false;
            idInput.GetComponent<InputField>().interactable = false;
            nameInput.GetComponent<InputField>().interactable = false;
            return;
        }
        if (!_working && idInput.GetComponent<InputField>().text.Length != 0
            && nameInput.GetComponent<InputField>().text.Length != 0)
        {
            GetComponent<Button>().interactable = true;    
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void OnClick()
    {
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;
        nameInput.GetComponent<InputField>().interactable = false;
        
        _working = true;
        StartCoroutine(AccountCheckRequest());
    }

    IEnumerator AccountCheckRequest()
    {
        WWWForm form = new WWWForm();
        var id = idInput.GetComponent<InputField>().text;
        var name = nameInput.GetComponent<InputField>().text;
        form.AddField("id", "\"" + id + "\"");
        form.AddField("name", "\"" + name + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/find_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            if (results == "OK")
            {
                resultText.GetComponent<Text>().text = "계정이 존재 합니다.";
                resultText.GetComponent<Text>().color = new Color(0, 0, 1);
                resultText.SetActive(true);
                GetComponent<Button>().interactable = false;
                pwInput.GetComponent<InputField>().interactable = true;
                _ok = true;
            }
            else
            {
                resultText.GetComponent<Text>().text = "계정을 찾을 수 없습니다.";
                resultText.GetComponent<Text>().color = new Color(1, 0, 0);
                resultText.SetActive(true);
                GetComponent<Button>().interactable = true;
                idInput.GetComponent<InputField>().interactable = true;
                nameInput.GetComponent<InputField>().interactable = true;
                _ok = false;
            }
            _working = false;
        }
    }
}
