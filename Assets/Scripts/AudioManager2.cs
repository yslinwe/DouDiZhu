using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    // 音频管理类实例
    private static AudioManager2 instance;

    // 音频组件
    private AudioSource audioSource;

    // 是否正在播放音频
    private bool isPlaying = false;

    // 音量
    private float volume = 1f;

    // 单例模式获取音频管理类实例
    public static AudioManager2 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("AudioManager2").AddComponent<AudioManager2>();
            }
            return instance;
        }
    }

    // 初始化音频组件
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // 播放音频
    public void PlayAudio(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            isPlaying = true;
        }
    }

    // 暂停音频
    public void PauseAudio()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
        }
    }

    // 停止音频
    public void StopAudio()
    {
        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    // 设置音量
    public void SetVolume(float volume)
    {
        this.volume = volume;
        audioSource.volume = volume;
    }
}
