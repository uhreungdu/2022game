using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IDCheckButton : MonoBehaviour
{
    public GameObject IDInput;
    public GameObject PWInput;
    public GameObject ErrText;
    
    private bool working = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!working && IDInput.GetComponent<InputField>().text.Length != 0)
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
        // IDCheck 비활성화
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;

        // ID 검증 요청
        working = true;
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
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            PWInput.GetComponent<InputField>().interactable = false;
        }
        else
        {
            Debug.Log("Form upload complete!");
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                PWInput.GetComponent<InputField>().interactable = true;
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
