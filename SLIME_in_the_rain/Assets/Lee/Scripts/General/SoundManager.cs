using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{ 
    #region �̱���
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

    public enum Sound
    {
        BGM,
        SFX,
        MaxCount,  // �ƹ��͵� �ƴ�. �׳� Sound enum�� ���� ���� ���� �߰�. (0, 1, '2' �̷��� 2��) 
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

    //����� �÷��� �Լ�
    /// <summary>
    /// ����� ��� ��(AudioClip)
    /// </summary>
    /// <param name="audioClip">�Ҹ� ���� �ֱ� ����</param>
    /// <param name="type">SoundManager�� Ÿ�� ����</param>
    public void Play(AudioClip audioClip, Sound type)
    {
        if (audioClip == null)
            return;

        if (type == Sound.BGM) // BGM ������� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // SFX ȿ���� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.SFX];
            audioSource.PlayOneShot(audioClip);
        }
    }
    //�Ҹ� ���� �ð�
    public void Play(AudioClip audioClip, Sound type, float time)
    {
        if (audioClip == null)
            return;

        if (type == Sound.BGM) // BGM ������� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.time = time;
            audioSource.Play();
        }
        else // SFX ȿ���� ���
        {
            AudioSource audioSource = audioSources[(int)Sound.SFX];
            audioSource.time = time;
            audioSource.PlayOneShot(audioClip);
        }
    }
    /// <summary>
    /// �Ҹ� ���� ���丮 ��� string �Է�(Resoures/Sounds/.. BGM or SFX)
    /// </summary>
    /// <param name="path">���丮�� string���� �Է�</param>
    /// <param name="type">SoundManager�� Ÿ�� ����</param>
    public void Play(string path, Sound type)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type);
    }
    /// <summary>
    /// n�� ���� ����ϱ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <param name="time"></param>
    /// 
    public void Play(string path, Sound type, float time)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, time);
    }

    AudioClip GetOrAddAudioClip(string path, Sound type)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{type}/{path}"; // Sound ���� �ȿ� ����� �� �ֵ���, $���̸� �����Ϸ����� {}�� �����ΰ� �˾Ƽ� ������

        AudioClip audioClip = null;

        if (type == Sound.BGM) // BGM ������� Ŭ�� ���̱�
        {
            if (BGMs.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                BGMs.Add(path, audioClip);
            }
        }
        else // Effect ȿ���� Ŭ�� ���̱�
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
