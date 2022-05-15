using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapEditerCoordText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public GameObject pointer;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = $"X : {pointer.transform.position.x.ToString("N0")}\n" +
                     $"Z : {pointer.transform.position.z.ToString("N0")}";
    }
}
