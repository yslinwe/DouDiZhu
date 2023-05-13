using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AudioConfigEditor : EditorWindow
{
    private string audioDirectory = "Assets/Resources/Audio/";
    private string outputFilePath = "Assets/StreamingAssets/AudioConfig.json";

    [MenuItem("Tools/Audio Config Editor")]
    static void Init()
    {
        AudioConfigEditor window = (AudioConfigEditor)EditorWindow.GetWindow(typeof(AudioConfigEditor));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Audio Config Settings", EditorStyles.boldLabel);

        audioDirectory = EditorGUILayout.TextField("Audio Directory", audioDirectory);
        outputFilePath = EditorGUILayout.TextField("Output File Path", outputFilePath);

        if (GUILayout.Button("Generate Audio Configs"))
        {
            GenerateAudioConfigs();
        }
    }

    void GenerateAudioConfigs()
    {
        List<AudioFileInfo> audioFiles = GetAudioFilesInDirectory(audioDirectory);
        AudioConfig audioConfig = new AudioConfig(audioFiles);
        string json = JsonUtility.ToJson(audioConfig, true);
        File.WriteAllText(outputFilePath, json);
        AssetDatabase.Refresh();
        Debug.Log("Audio Configs generated successfully!");
    }
    string pathWithoutExtension;
    List<AudioFileInfo> GetAudioFilesInDirectory(string directory)
    {
        List<AudioFileInfo> audioFiles = new List<AudioFileInfo>();

        DirectoryInfo dirInfo = new DirectoryInfo(directory);
        FileInfo[] fileInfo = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

        foreach (FileInfo file in fileInfo)
        {
            if (IsAudioFileType(file.Extension))
            {
                AudioFileInfo audioFileInfo = new AudioFileInfo();
                // string directoryPath = Path.GetDirectoryName(file.FullName);
                // string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string relativePath = Path.GetRelativePath("Assets/Resources/", file.FullName);
                pathWithoutExtension = Path.Combine(Path.GetDirectoryName(relativePath), Path.GetFileNameWithoutExtension(relativePath));
                audioFileInfo.name = pathWithoutExtension;
                audioFiles.Add(audioFileInfo);
            }
        }

        return audioFiles;
    }

    bool IsAudioFileType(string extension)
    {
        switch (extension.ToLower())
        {
            case ".mp3":
            case ".wav":
            case ".ogg":
            case ".aiff":
                return true;
            default:
                return false;
        }
    }
}

[System.Serializable]
public class AudioConfig
{
    public List<AudioFileInfo> audioConfigs;

    public AudioConfig(List<AudioFileInfo> audioConfigs)
    {
        this.audioConfigs = audioConfigs;
    }
}

[System.Serializable]
public class AudioFileInfo
{
    public string name;
}
