using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CharMakeButton : MonoBehaviour
{
    public GameObject IDInput;
    public GameObject NameInput;
    public GameObject ErrText;
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
        // 로그인 창 비활성화
        GetComponent<Button>().interactable = false;
        NameInput.GetComponent<InputField>().interactable = false;

        // 로그인 요청
        StartCoroutine(MakeCharRequest());
    }
    
    IEnumerator MakeCharRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + IDInput.GetComponent<InputField>().text + "\"");
        form.AddField("name", "\""+NameInput.GetComponent<InputField>().text+"\"") ;

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/make_character.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            NameInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            NameInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                ErrText.SetActive(false);
            }
            else
            {
                ErrText.SetActive(true);
                ErrText.GetComponent<Text>().text = results;
            }
        }
    }
}
