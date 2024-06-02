using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser; // Ensure you have imported the namespace for the file browser

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private void Awake()
    {
        Instance = this;
        // It's generally a good practice to initialize the file browser at the start
        FileBrowser.SetFilters(true, new FileBrowser.Filter("JSON Files", ".json"));
        FileBrowser.SetDefaultFilter(".json");
    }

    // Save settings to a JSON file using a runtime file browser
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

        StartCoroutine(ShowSaveFileDialog(json));
    }

    IEnumerator ShowSaveFileDialog(string json)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, null, "Save Configuration", "Save");
        if (FileBrowser.Success)
        {
            File.WriteAllText(FileBrowser.Result[0], json);
        }
    }

    // Load settings from a JSON file using a runtime file browser
    public void LoadSettingsFromFile()
    {
        StartCoroutine(ShowLoadFileDialog());
    }

    IEnumerator ShowLoadFileDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Configuration", "Load");
        if (FileBrowser.Success)
        {
            string json = File.ReadAllText(FileBrowser.Result[0]);
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
