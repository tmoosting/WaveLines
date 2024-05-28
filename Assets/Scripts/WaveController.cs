using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

// To Do 
  
// file export, import in data ui
   


public class WaveController : MonoBehaviour
{
    public static WaveController Instance;

    [Header("Assignments")] 
    public ControlUI controlUI;
    public GameObject spawnMarkerStartPrefab;
    public GameObject spawnMarkerEndPrefab;
    public GameObject waveLinePrefab;
    public Material defaultMarkerMaterial;
    public Material selectedMarkerMaterial;
    public Material lockedMarkerMaterial;
    
    

    
    // Tracking
    public int selectedLineIndex = 0;
    
    // Memory
    private Dictionary<Vector3, Vector3> _startEndPoints;
    private List<GameObject> _startSpawnMarkers;
    private List<GameObject> _endSpawnMarkers;
    private List<WaveLine> _waveLines;
    [HideInInspector]
    public List<int> lockedMarkers;
    [HideInInspector]
    public List<WaveSettings> waveSettingsList;
    
    
    // Lines
    public int pointCount = 100;
    public float baseHeight = 1.5f;
    public int lineAmount = 10; // number of waves total
    public float lineHorizontalSpacing = 2; // horizontal space between waves
    public float lineVerticalSpacing = 1; // distance in height for each wave
    public float lineLength = 12; // length of line in total; needs to always be a complete number of waves!
    public float lineDiameter = 2; // thickness of lines
    // to add: line texture; 
    
    
    // Waves Global Settings
    public float waveSpeed; // how fast the wave travels 
    public float waveAmplitude; // distance from center to max extent
    public float waveLength; // length of one wave 
    
    
    public bool spawnStartEndMarkers;

    
    // Min Max Values - Waves
    public float speedMinValue = 0.01f;
    public float speedMaxValue = 5f;
    public float amplitudeMinValue = 0.01f;
    public float amplitudeMaxValue = 5f;
    public float wavelengthMinValue = 0.01f;
    public float wavelengthMaxValue = 5f;
    
    // Min Max Values - Setup
    public float pointCountMinValue = 50f;
    public float pointCountMaxValue = 500f;
    public float heightMinValue = 1f;
    public float heightMaxValue = 10f;
    public float lineCountMinValue = 1f;
    public float lineCountMaxValue = 20f;
    public float horizontalSpaceMinValue = 0.5f;
    public float horizontalSpaceMaxValue = 5f;
    public float verticalSpaceMinValue = 0f;
    public float verticalSpaceMaxValue = 5f;
    public float lengthMinValue = 1f;
    public float lengthMaxValue = 50f;
    public float diameterMinValue = 0.01f;
    public float diameterMaxValue = 5f;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lockedMarkers = new List<int>();

        SpawnEverything();
   
    }

    private void SpawnEverything()
    {
        DetermineStartEndPoints();

        if (spawnStartEndMarkers)
        {
            SpawnStartMarkers(); 
            UpdateSpawnMarkers();
        }
        
        SpawnWaves(); 
    }
    public void Rebuild()
    {
        DestroyStartMarkers();
        DestroyWaveLines();
        SpawnEverything();
    }

    public void SaveSettings()
    {
        SettingsManager.Instance.SaveSettingsToFile();
    }
    public void LoadSettings()
    {
        SettingsManager.Instance.LoadSettingsFromFile(); 
        
    }
    public void ApplyLoadedSettings(SettingsFile settings)
    {
        // Update global settings
        pointCount = settings.PointCount;
        baseHeight = settings.BaseHeight;
        lineAmount = settings.LineAmount;
        lineHorizontalSpacing = settings.LineHorizontalSpacing;
        lineVerticalSpacing = settings.LineVerticalSpacing;
        lineLength = settings.LineLength;
        lineDiameter = settings.LineDiameter;
        waveSpeed = settings.WaveSpeed;
        waveAmplitude = settings.WaveAmplitude;
        waveLength = settings.WaveLength;

  

        // Apply wave settings to each line
        waveSettingsList = new List<WaveSettings>(settings.WaveSettingsList); 
        // Since the structure of the lines might have changed, need to rebuild the visual representation
    
        lockedMarkers = new List<int>(settings.LockedMarkers);

        Rebuild();
 
        
        // Optionally, refresh UI elements if needed
        controlUI?.RefreshWindow(0); // Assuming there is a method to update UI based on new settings
    }

 
    
    
    public void ClickLineWithIndex(int index)
    { 
        selectedLineIndex = index;
        // open tab for line
        controlUI.RefreshWindow(3  );
        UpdateSpawnMarkers();
    }
    
    
    
    
    
    
    public void SetGlobalWaveSpeed(float evtNewValue)
    {
        waveSpeed = evtNewValue;
        foreach (var waveSettings in waveSettingsList)
        {
            int index = waveSettingsList.IndexOf(waveSettings);
            if (lockedMarkers.Contains(index) == false) 
                waveSettingsList[index].WaveSpeed = waveSpeed;  
        }
        MakeWaveChange();
    }
    public void SetGlobalWaveAmplitude(float evtNewValue)
    {
        waveAmplitude = evtNewValue;
        foreach (var waveSettings in waveSettingsList)
        {
            int index = waveSettingsList.IndexOf(waveSettings);
            if (lockedMarkers.Contains(index) == false) 
                waveSettingsList[index].WaveAmplitude = waveAmplitude;  
        }
        MakeWaveChange(); 
    }
    public void SetGlobalWaveWavelength(float evtNewValue)
    {
        waveLength = evtNewValue;
        foreach (var waveSettings in waveSettingsList)
        {
            int index = waveSettingsList.IndexOf(waveSettings);
            if (lockedMarkers.Contains(index) == false) 
                waveSettingsList[index].WaveLength = waveLength;  
        }
        MakeWaveChange(); 
    }
    public void SetWaveSpeed(float evtNewValue)
    {
        waveSettingsList[selectedLineIndex].WaveSpeed = evtNewValue;
        MakeWaveChange();
    }
    public void SetWaveAmplitude(float evtNewValue)
    {
        waveSettingsList[selectedLineIndex].WaveAmplitude = evtNewValue;
        MakeWaveChange();
    }
    public void SetWaveWavelength(float evtNewValue)
    {
        waveSettingsList[selectedLineIndex].WaveLength = evtNewValue;
        MakeWaveChange();
    }
    
  
    public void MakeWaveChange()
    {
        foreach (var waveLine in _waveLines)
        {
            int index = _waveLines.IndexOf(waveLine);
            waveLine.LoadWaveSettings(waveSettingsList[index] );
        }
    }

  

    private void DestroyStartMarkers()
    {
        foreach (var marker in _startSpawnMarkers)
            if (marker != null)
                Destroy(marker.gameObject);
        _startSpawnMarkers = new List<GameObject>();
    }
    private void DestroyWaveLines()
    {
        foreach (var line in _waveLines)
            if (line != null)
                Destroy(line.gameObject);
        _waveLines = new List<WaveLine>();
        waveSettingsList = new List<WaveSettings>();
    }

    private void SpawnWaves()
    { 
        SpawnWaveLines();
        ConfigureWaveLines();
    } 
    private void SpawnWaveLines()
    {
        waveSettingsList = new List<WaveSettings>();
        _waveLines = new List<WaveLine>();
        foreach (var vector3 in _startEndPoints.Keys)
            SpawnWaveLine(vector3,_startEndPoints[vector3] );
    }  
    private void SpawnWaveLine(Vector3 startPoint, Vector3 endPoint)
    {
        GameObject lineObj = Instantiate(waveLinePrefab);
        lineObj.transform.position = startPoint;
         WaveLine waveLine = lineObj.GetComponent<WaveLine>();
     //   waveLine.GetComponent<LineRenderer>().SetPosition(0, startPoint);
     
     
        _waveLines.Add(waveLine);
    }
    
    private void ConfigureWaveLines()
    {
        foreach (var vector3 in _startEndPoints.Keys)
        {
            int index = _startEndPoints.Keys.ToList().IndexOf(vector3);

            WaveLine waveLine = _waveLines[index];
            waveLine.Initialize(index,pointCount, new Vector3(0,vector3.y,0),_startEndPoints [vector3], lineDiameter,lineLength   );
            WaveSettings waveSettings = new WaveSettings(index, waveSpeed,waveAmplitude,waveLength );
            waveLine.LoadWaveSettings(waveSettings); 
            waveSettingsList.Add(waveSettings);
        }
       
    }




    private void DetermineStartEndPoints()
    {
        _startEndPoints = new Dictionary<Vector3, Vector3>();
        Vector3 startPoint = new Vector3(0, baseHeight, 0);
        Vector3 endPoint = new Vector3(0, baseHeight, lineLength);
        _startEndPoints.Add(startPoint, endPoint);

        // Ensure correct calculation of subsequent start and end points based on spacing
        for (int i = 1; i < lineAmount; i++)
        {
            // Calculate the new start point by applying the correct spacing
            Vector3 nextLineStartPoint = new Vector3(startPoint.x + i * lineHorizontalSpacing, startPoint.y + i * lineVerticalSpacing, startPoint.z);
            Vector3 nextLineEndPoint = new Vector3(nextLineStartPoint.x, nextLineStartPoint.y, nextLineStartPoint.z + lineLength);
            _startEndPoints.Add(nextLineStartPoint, nextLineEndPoint);
        }
    }


    private void SpawnStartMarkers()
    {
        _startSpawnMarkers = new List<GameObject>();
        _endSpawnMarkers = new List<GameObject>();

        foreach (var startPosition in _startEndPoints.Keys)
        {
            int index = _startEndPoints.Keys.ToList().IndexOf(startPosition);
            GameObject startSpawn = Instantiate(spawnMarkerStartPrefab);
            startSpawn.transform.position = startPosition; // Ensure marker uses the correct start position
            SpawnMarker marker = startSpawn.GetComponent<SpawnMarker>();
            marker.Initialize(index); 
            _startSpawnMarkers.Add(startSpawn);
        }
    }

    private void UpdateSpawnMarkers()
    {
        foreach (var marker in _startSpawnMarkers)
        {
            int index = _startSpawnMarkers.IndexOf(marker);
            if (selectedLineIndex == index)
                marker.GetComponent<SpawnMarker>().SetMaterial(selectedMarkerMaterial);
            else if (lockedMarkers.Contains(index))
                marker.GetComponent<SpawnMarker>().SetMaterial(lockedMarkerMaterial);
            else
                marker.GetComponent<SpawnMarker>().SetMaterial(defaultMarkerMaterial); 
        }
    }



    public WaveSettings GetWaveSettingsForIndex(int index = 99)
    {
        if (index == 99)
            return waveSettingsList[selectedLineIndex];
        return waveSettingsList[index];
    }

    public bool IsActiveLineLocked()
    {
        return lockedMarkers.Contains(selectedLineIndex);
    }

    public void SetSelectedLineLockValue(bool nowLocked)
    {
        if (nowLocked == false)
        {
            if (lockedMarkers.Contains(selectedLineIndex))
                lockedMarkers.Remove(selectedLineIndex);
        }
        else
        {
            if (lockedMarkers.Contains(selectedLineIndex) == false)
                lockedMarkers.Add(selectedLineIndex); 
        }
       
    }

    public void SetPointAmount(int evtNewValue)
    {
        pointCount = evtNewValue;
    }

    public void SetHeight(float evtNewValue)
    {
        baseHeight = evtNewValue;
    }

    public void SetLinesAmount(int evtNewValue)
    {
        lineAmount = evtNewValue;
    }

    public void SetHorizontalSpaceAmount(float evtNewValue)
    {
        lineHorizontalSpacing = evtNewValue;
    }

    public void SetVerticalSpaceAmount(float evtNewValue)
    {
        lineVerticalSpacing = evtNewValue;
    }

    public void SetLineLength(float evtNewValue)
    {
        lineLength = evtNewValue;
    }

    public void SetLineDiameter(float evtNewValue)
    {
        lineDiameter = evtNewValue;
    }


}
