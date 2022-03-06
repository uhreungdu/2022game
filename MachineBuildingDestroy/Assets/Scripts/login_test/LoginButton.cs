using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginButton : MonoBehaviour
{
    public GameObject IDInput;
    public GameObject PWInput;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        // �α��� â ��Ȱ��ȭ
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;
        PWInput.GetComponent<InputField>().interactable = false;
        
        // �α��� ��û
        StartCoroutine(LoginRequest());
    }
    
    IEnumerator LoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\""+IDInput.GetComponent<InputField>().text+"\"") ;
        form.AddField("pw", "\"" + PWInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/login_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // �α��� â Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            Debug.Log("Form upload complete!");
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // �α��� â Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = true;
        }
    }
}
