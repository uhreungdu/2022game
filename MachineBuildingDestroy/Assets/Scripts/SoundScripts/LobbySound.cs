using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySound : MonoBehaviour
{
    // Start is called before the first frame update
    private SoundManager _soundManager;
    void Awake()
    {
        _soundManager = SoundManager.GetInstance();
        _soundManager.Play("Sounds/Lobby", SoundManager.Sound.Bgm);
    }
}
