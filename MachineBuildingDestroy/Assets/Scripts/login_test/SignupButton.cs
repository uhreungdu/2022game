using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SignupButton : MonoBehaviour
{
    public GameObject IDInput;
    public GameObject PWInput;
    public GameObject ParentWindow;
    public GameObject ErrText;

    private bool working = false;
    
    // Start is called before the first frame update
    void Start()
    {
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
        // 가입 비활성화
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;
        PWInput.GetComponent<InputField>().interactable = false;
        
        // 가입 요청
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
            // 가입 활성화
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = true;
            working = false;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // 가입 활성화
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                ParentWindow.SetActive(false);
                ErrText.SetActive(false);
            }
            else
            {
                ErrText.GetComponent<Text>().text = results;
                ErrText.SetActive(true);
            }
            working = false;
        }
    }
}
