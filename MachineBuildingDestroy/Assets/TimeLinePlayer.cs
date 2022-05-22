using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLinePlayer : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    // Update is called once per frame
    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            _playableDirector.enabled = true;
    }
}
