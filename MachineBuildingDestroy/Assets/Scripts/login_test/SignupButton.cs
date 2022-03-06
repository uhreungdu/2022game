using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SignupButton : MonoBehaviour
{
    private GameObject IDInput;
    private GameObject PWInput;
    public GameObject ParentWindow;

    private bool working = false;
    
    // Start is called before the first frame update
    void Start()
    {
        IDInput = GameObject.Find("R_IDInputField");
        PWInput = GameObject.Find("R_PWInputField");
    }

    // Update is called once per frame
    void Update()
    {
        if (!working && PWInput.GetComponent<InputField>().text.Length != 0)
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
        // ���� ��Ȱ��ȭ
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;
        PWInput.GetComponent<InputField>().interactable = false;
        
        // ���� ��û
        working = true;
        StartCoroutine(RegisterRequest());
    }

    IEnumerator RegisterRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + IDInput.GetComponent<InputField>().text + "\"");
        form.AddField("pw", "\"" + PWInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/make_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // ���� Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = true;
            working = false;
        }
        else
        {
            Debug.Log("Form upload complete!");
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // ���� Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = true;
            ParentWindow.SetActive(false);
            working = false;
        }
    }
}