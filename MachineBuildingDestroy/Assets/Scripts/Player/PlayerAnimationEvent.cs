using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    System.Action _beginCallback = null;
    System.Action _midCallback = null;
    System.Action _endCallback = null;
    System.Action _audioClipCallback = null;

    public void Play(
        System.Action beginCallback = null,
        System.Action midCallback = null,
        System.Action endCallback = null,
        System.Action audioClipCallback = null
    )
    {
        _beginCallback = beginCallback;
        _midCallback = midCallback;
        _endCallback = endCallback;
        _audioClipCallback = audioClipCallback;
    }

    //Animation Event
    public void OnBeginEvent()
    {
        //if (null != _beginCallback)
        //	_beginCallback();

        _beginCallback?.Invoke();	//위 주석부분처럼 작성해도 무관합니다.
    }

    public void OnMidEvent()
    {
        _midCallback?.Invoke();
    }

    public void OnEndEvent()
    {
        Debug.Log("Animaton End Event");
        _endCallback?.Invoke();
    }

    public void OnAudioClipEvent()
    {
        _audioClipCallback?.Invoke();
    }
}
