using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _backgroundAudioPlayer; // 오디오 소스 컴포넌트
    public AudioClip BackgroundAudioClip; // 사망시 재생할 소리
    void Awake()
    {
        _backgroundAudioPlayer = GetComponent<AudioSource>();
        _backgroundAudioPlayer.PlayOneShot(BackgroundAudioClip);
    }
}
