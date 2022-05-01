using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class ResetPWButton : MonoBehaviour
{
    public GameObject idInput;
    public GameObject pwInput;
    public GameObject parentWindow;
    public GameObject darkBackground;

    public GameObject idButton;

    private bool _working = false;
    
    void Update()
    {
        var idok = idButton.GetComponent<AccountCheckButton>().GetOK();

        if (idok == false)
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
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;
        pwInput.GetComponent<InputField>().interactable = false;

        _working = true;
        StartCoroutine(RegisterRequest());
    }

    IEnumerator RegisterRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + idInput.GetComponent<InputField>().text + "\"");
        form.AddField("pw", "\"" + pwInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/reset_pw.php", form);
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
