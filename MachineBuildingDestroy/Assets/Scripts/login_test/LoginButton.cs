using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class LoginButton : MonoBehaviour
{
    [FormerlySerializedAs("IDInput")] public GameObject idInput;
    [FormerlySerializedAs("PWInput")] public GameObject pwInput;
    [FormerlySerializedAs("MakeCharWindow")] public GameObject makeCharWindow;
    [FormerlySerializedAs("ErrText")] public GameObject errText;
    
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
        idInput.GetComponent<InputField>().interactable = false;
        pwInput.GetComponent<InputField>().interactable = false;
        
        // �α��� ��û
        StartCoroutine(LoginRequest());
    }
    
    IEnumerator LoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\""+idInput.GetComponent<InputField>().text+"\"") ;
        form.AddField("pw", "\"" + pwInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/login_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // �α��� â Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // �α��� â Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                // ĳ���� ����, �κ�� �̵�
                errText.SetActive(false);
            }
            else if (results == "Need Character")
            {
                // ĳ���� �̺���, ���� �ʿ�
                makeCharWindow.SetActive(true);
            }
            else
            {
                // �α��� ����
                errText.SetActive(true);
                errText.GetComponent<Text>().text = results;
            }
        }
    }
}
