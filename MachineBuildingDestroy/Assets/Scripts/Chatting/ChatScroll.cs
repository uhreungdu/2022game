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
        var scrollbar = GameObject.Find("Scrollbar Vertical");
        if (scrollbar != null) scrollbar.GetComponent<Scrollbar>().value = 0;
        _done = true;
    }
}
