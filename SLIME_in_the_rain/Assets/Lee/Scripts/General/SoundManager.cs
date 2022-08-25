using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{ 
    #region 싱글톤
    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> BGMs = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> SFXs = new Dictionary<string, AudioClip>();

    SceneDesign sceneDesign;
    public enum Sound
    {
        BGM,
        SFX,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = this.transform.GetChild(i).GetComponent<AudioSource>();
        }
        GetOrAddAudioClip("Title", Sound.BGM);
    }

    //오디오 플레이 함수
    public void Play(AudioClip audioClip, Sound type)
    {
        if (audioClip == null)
            return;

        if (type == Sound.BGM) // BGM 배경음악 재생
        {
            AudioSource audioSource = audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // SFX 효과음 재생
        {
            AudioSource audioSource = audioSources[(int)Sound.SFX];
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void Play(string path, Sound type)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type);
    }

    AudioClip GetOrAddAudioClip(string path, Sound type)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{type}/{path}"; // Sound 폴더 안에 저장될 수 있도록, $붙이면 컴파일러에서 {}가 변수인걸 알아서 구분함

        AudioClip audioClip = null;

        if (type == Sound.BGM) // BGM 배경음악 클립 붙이기
        {
            if (BGMs.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                BGMs.Add(path, audioClip);
            }
        }
        else // Effect 효과음 클립 붙이기
        {
            if (SFXs.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                SFXs.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}
