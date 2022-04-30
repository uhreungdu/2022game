using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatScroll : MonoBehaviour
{
    private bool _done = false;
    private void FixedUpdate()
    {
        if (_done) return;
        GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>().value = 0;
        _done = true;
    }
}
