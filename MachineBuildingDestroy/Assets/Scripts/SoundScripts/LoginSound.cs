using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSound : MonoBehaviour
{
    // Start is called before the first frame update
    private SoundManager _soundManager;
    void Start()
    {
        _soundManager = SoundManager.GetInstance();
        _soundManager.Play("Sounds/LoginAudio" , SoundManager.Sound.Bgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
