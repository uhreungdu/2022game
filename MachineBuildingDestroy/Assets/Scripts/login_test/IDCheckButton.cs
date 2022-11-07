using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class IDCheckButton : MonoBehaviour
{
    public GameObject idInput;
    public GameObject pwInput;
    public GameObject resultText;
    public GameObject nameInput;

    private bool _working = false;
    private bool _ok = false;
    private string _id;

    public void ResetValues()
    {
        _working = false;
        _ok = false;
        _id = "";
        idInput.GetComponent<InputField>().text = "";
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
        if (!_working && idInput.GetComponent<InputField>().text.Length != 0)
        {
            GetComponent<Button>().interactable = true;    
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
        if (_ok && idInput.GetComponent<InputField>().text != _id)
        {
            GetComponent<Button>().interactable = false;
            _ok = false;
        }
    }

    public void OnClick()
    {
        // IDCheck 비활성화
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;

        // ID 검증 요청
        _working = true;
        StartCoroutine(IDCheckRequest());
    }

    IEnumerator IDCheckRequest()
    {
        WWWForm form = new WWWForm();
        var id = idInput.GetComponent<InputField>().text;
        form.AddField("id", "\"" + id + "\"");

        UnityWebRequest www =
            UnityWebRequest.Post(
                "http://" + PhotonNetwork.PhotonServerSettings.AppSettings.Server + "/login/id_check.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                _id = id;
                pwInput.GetComponent<InputField>().interactable = true;
                nameInput.GetComponent<InputField>().interactable = true;
                resultText.GetComponent<Text>().text = "ID 사용 가능";
                resultText.GetComponent<Text>().color = new Color(0, 0, 1);
                resultText.SetActive(true);
                _ok = true;
            }
            else
            {
                pwInput.GetComponent<InputField>().interactable = false;
                nameInput.GetComponent<InputField>().interactable = false;
                resultText.GetComponent<Text>().text = "ID 사용 불가";
                resultText.GetComponent<Text>().color = new Color(1, 0, 0);
                resultText.SetActive(true);
                _ok = false;
            }
            _working = false;
        }
    }
}
