using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Save settings to a JSON file
    public void SaveSettingsToFile()
    {
        SettingsFile settings = new SettingsFile
        {
            PointCount = WaveController.Instance.pointCount,
            BaseHeight = WaveController.Instance.baseHeight,
            LineAmount = WaveController.Instance.lineAmount,
            LineHorizontalSpacing = WaveController.Instance.lineHorizontalSpacing,
            LineVerticalSpacing = WaveController.Instance.lineVerticalSpacing,
            LineLength = WaveController.Instance.lineLength,
            LineDiameter = WaveController.Instance.lineDiameter,
            WaveSpeed = WaveController.Instance.waveSpeed,
            WaveAmplitude = WaveController.Instance.waveAmplitude,
            WaveLength = WaveController.Instance.waveLength,
            LockedMarkers = new List<int>(WaveController.Instance.lockedMarkers),
            WaveSettingsList = new List<WaveSettings>(WaveController.Instance.waveSettingsList)
        };

        string json = JsonUtility.ToJson(settings, true);

        string path = EditorUtility.SaveFilePanel("Save Configuration", "", "WaveConfig.json", "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
        }
    }

    // Load settings from a JSON file
    public void LoadSettingsFromFile()
    {
        string path = EditorUtility.OpenFilePanel("Load Configuration", "", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            ApplySettings(json);
        }
    }

    // Apply loaded settings
    private void ApplySettings(string json)
    {
        SettingsFile settings = JsonUtility.FromJson<SettingsFile>(json);
        WaveController.Instance.ApplyLoadedSettings(settings);
    }
}


