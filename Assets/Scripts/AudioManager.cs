using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioClip> audioClips;
    private AudioSource bgmSource = null;
    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // 加载背景音乐AudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.8f;

        DontDestroyOnLoad(gameObject);

        LoadAudioClips();
    }

    private void LoadAudioClips()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "AudioConfig.json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            AudioConfig audioConfig = JsonUtility.FromJson<AudioConfig>(dataAsJson);

            foreach (AudioConfigData audioConfigData in audioConfig.audioConfigs)
            {
                // AudioClip audioClip = audioClips.Find(x => x.name == audioConfigData.name);
                
                AudioClip audioClip = Resources.Load<AudioClip>(audioConfigData.name);

                if (audioClip == null)
                {
                    Debug.LogWarning("Audio clip " + audioConfigData.name + " not found!");
                }
                else
                {         
                    audioClips.Add(audioClip);
                    audioClipDict.Add(Path.GetFileName(audioConfigData.name), audioClip);
                }
            }
        }
        else
        {
            Debug.LogError("Audio config file not found at " + filePath);
        }
    }
    public void PlayBGM(string name)
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = audioClipDict[name];
        bgmSource.Play();
    }
    public void StopBGM(string name)
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
    }
    public void PlaySound(string soundName)
    {
        AudioClip audioClip;
        if (audioClipDict.TryGetValue(soundName, out audioClip))
        {
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        }
        else
        {
            Debug.LogWarning("Audio clip " + soundName + " not found!");
        }
    }
}

[System.Serializable]
public class AudioConfig
{
    public List<AudioConfigData> audioConfigs;
}

[System.Serializable]
public class AudioConfigData
{
    public string name;
}
