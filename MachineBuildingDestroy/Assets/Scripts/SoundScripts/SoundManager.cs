using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }
    private static SoundManager instance;

    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    public static SoundManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<SoundManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("SoundManager");
                instance = container.AddComponent<SoundManager>();
                instance.Init();
                DontDestroyOnLoad(container);
            }
        }
        return instance;
    }
    public void Init()
    {
        string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] }; 
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = gameObject.transform;
        }
        _audioSources[(int)Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
        _audioSources[(int)Sound.Bgm].volume = 0.1f; // bgm 재생기는 무한 반복 재생
    }
    public void Clear()
    {
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sound.Bgm) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }
    
    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}"; // 📂Sound 폴더 안에 저장될 수 있도록

        AudioClip audioClip = null;

        if (type == Sound.Bgm) // BGM 배경음악 클립 붙이기
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}
