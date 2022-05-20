using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSound : MonoBehaviour
{
    private SoundManager _soundManager;
    // Start is called before the first frame update
    public void OnClick(AudioClip audioClip)
    {
        SoundManager.GetInstance().Play(audioClip, SoundManager.Sound.Effect);
    }
    
    public void OnClick(string path)
    {
        SoundManager.GetInstance().Play(path, SoundManager.Sound.Effect);
    }
}
