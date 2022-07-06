using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSoundPlay : MonoBehaviour
{
    // Start is called before the first frame update
    private SoundManager _soundManager;
    public AudioClip _AudioClip;
    void Awake()
    {
        _soundManager = SoundManager.GetInstance();
        _soundManager.Play(_AudioClip, SoundManager.Sound.Bgm);
    }
}
