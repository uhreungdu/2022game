using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IDCheckButton : MonoBehaviour
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
        GetComponent<Button>().interactable = IDInput.GetComponent<InputField>().text.Length != 0;
    }

    public void OnClick()
    {
        // IDCheck ��Ȱ��ȭ
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;

        // ID ���� ��û
        StartCoroutine(IDCheckRequest());
    }

    IEnumerator IDCheckRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + IDInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/id_check.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // IDCheck Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = false;
        }
        else
        {
            Debug.Log("Form upload complete!");
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // IDCheck Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = results == "OK";
        }
    }
}
