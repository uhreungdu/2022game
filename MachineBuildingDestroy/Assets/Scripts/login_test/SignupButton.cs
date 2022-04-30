using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class SignupButton : MonoBehaviour
{
    public GameObject idInput;
    public GameObject pwInput;
    public GameObject nameInput;
    public GameObject parentWindow;
    public GameObject darkBackground;

    public GameObject idButton;
    public GameObject nameButton;

    private bool _working = false;
    
    void Update()
    {
        var idok = idButton.GetComponent<IDCheckButton>().GetOK();
        var nameok = nameButton.GetComponent<NameCheckButton>().GetOK();

        if (idok == false || nameok == false)
        {
            GetComponent<Button>().interactable = false;
            return;
        }
        if (!_working && pwInput.GetComponent<InputField>().text.Length != 0)
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
        // 가입 비활성화
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;
        pwInput.GetComponent<InputField>().interactable = false;
        nameInput.GetComponent<InputField>().interactable = false;

        // 가입 요청
        _working = true;
        StartCoroutine(RegisterRequest());
    }

    IEnumerator RegisterRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + idInput.GetComponent<InputField>().text + "\"");
        form.AddField("pw", "\"" + pwInput.GetComponent<InputField>().text + "\"");
        form.AddField("name", "\"" + nameInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/make_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            parentWindow.SetActive(false);
            darkBackground.SetActive(false);
        }
    }
}
