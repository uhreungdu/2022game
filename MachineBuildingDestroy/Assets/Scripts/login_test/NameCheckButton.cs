using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class NameCheckButton : MonoBehaviour
{
    public GameObject nameInput;
    public GameObject resultText;

    private bool _working = false;
    private bool _ok = false;
    private string _nickname;

    public void ResetValues()
    {
        _working = false;
        _ok = false;
        _nickname = "";
        nameInput.GetComponent<InputField>().text = "";
        nameInput.GetComponent<InputField>().interactable = false;
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
        if (!_working && nameInput.GetComponent<InputField>().text.Length != 0)
        {
            GetComponent<Button>().interactable = true;    
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }

        if (_ok && nameInput.GetComponent<InputField>().text != _nickname)
        {
            GetComponent<Button>().interactable = false;
            _ok = false;
        }
    }

    public void OnClick()
    {
        // NameCheck 비활성화
        GetComponent<Button>().interactable = false;
        nameInput.GetComponent<InputField>().interactable = false;

        // Name 검증 요청
        _working = true;
        StartCoroutine(NameCheckRequest());
    }

    IEnumerator NameCheckRequest()
    {
        WWWForm form = new WWWForm();
        var nickname = nameInput.GetComponent<InputField>().text;
        form.AddField("name", "\"" + nickname + "\"");

        UnityWebRequest www =
            UnityWebRequest.Post(
                "http://" + PhotonNetwork.PhotonServerSettings.AppSettings.Server + "/login/name_check.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // NameCheck 활성화
            GetComponent<Button>().interactable = true;
            nameInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            // NameCheck 활성화
            GetComponent<Button>().interactable = true;
            nameInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                _nickname = nickname;
                resultText.GetComponent<Text>().text = "닉네임 사용 가능";
                resultText.GetComponent<Text>().color = new Color(0, 0, 1);
                resultText.SetActive(true);
                _ok = true;
            }
            else
            {
                resultText.GetComponent<Text>().text = "닉네임 사용 불가";
                resultText.GetComponent<Text>().color = new Color(1, 0, 0);
                resultText.SetActive(true);
                _ok = false;
            }
            _working = false;
        }
    }
}
