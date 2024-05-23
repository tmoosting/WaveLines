using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

// To Do
   
// update ui based on current wave settings

// linecolors: one for unlocked; one for selected; one for locked

// global vs individual lines
// Connect UI to variables below
// preset file save system - external import/export for webgl


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
    private List<int> _lockedMarkers;
    private List<WaveSettings> _waveSettingsList;
    
    
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
    
    
    // Settings
    public bool spawnStartEndMarkers;
    public float speedMinValue = 0.01f;
    public float speedMaxValue = 5f;
    public float amplitudeMinValue = 0.01f;
    public float amplitudeMaxValue = 5f;
    public float wavelengthMinValue = 0.01f;
    public float wavelengthMaxValue = 5f;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
        foreach (var waveSettings in _waveSettingsList)
        {
            int index = _waveSettingsList.IndexOf(waveSettings);
            if (_lockedMarkers.Contains(index) == false) 
                _waveSettingsList[index].waveSpeed = waveSpeed;  
        }
        MakeWaveChange();
    }
    public void SetGlobalWaveAmplitude(float evtNewValue)
    {
        waveAmplitude = evtNewValue;
        foreach (var waveSettings in _waveSettingsList)
        {
            int index = _waveSettingsList.IndexOf(waveSettings);
            if (_lockedMarkers.Contains(index) == false) 
                _waveSettingsList[index].waveAmplitude = waveAmplitude;  
        }
        MakeWaveChange(); 
    }
    public void SetGlobalWaveWavelength(float evtNewValue)
    {
        waveLength = evtNewValue;
        foreach (var waveSettings in _waveSettingsList)
        {
            int index = _waveSettingsList.IndexOf(waveSettings);
            if (_lockedMarkers.Contains(index) == false) 
                _waveSettingsList[index].waveLength = waveLength;  
        }
        MakeWaveChange(); 
    }
    public void SetWaveSpeed(float evtNewValue)
    {
        _waveSettingsList[selectedLineIndex].waveSpeed = evtNewValue;
        MakeWaveChange();
    }
    public void SetWaveAmplitude(float evtNewValue)
    {
        _waveSettingsList[selectedLineIndex].waveAmplitude = evtNewValue;
        MakeWaveChange();
    }
    public void SetWaveWavelength(float evtNewValue)
    {
        _waveSettingsList[selectedLineIndex].waveLength = evtNewValue;
        MakeWaveChange();
    }
    
    public void MakeLineChange()
    {
        DestroyStartMarkers();
        DestroyWaveLines();
        SpawnEverything();
    }

    public void MakeWaveChange()
    {
        foreach (var waveLine in _waveLines)
        {
            int index = _waveLines.IndexOf(waveLine);
            waveLine.LoadWaveSettings(_waveSettingsList[index] );
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
        _waveSettingsList = new List<WaveSettings>();
    }

    private void SpawnWaves()
    { 
        SpawnWaveLines();
        ConfigureWaveLines();
    } 
    private void SpawnWaveLines()
    {
        _waveSettingsList = new List<WaveSettings>();
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
            waveLine.Initialize(index,pointCount, new Vector3(0,vector3.y,0),_startEndPoints [vector3], lineLength );
            WaveSettings waveSettings = new WaveSettings(index, waveSpeed,waveAmplitude,waveLength );
            waveLine.LoadWaveSettings(waveSettings); 
            _waveSettingsList.Add(waveSettings);
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
        _lockedMarkers = new List<int>();
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
            else if (_lockedMarkers.Contains(index))
                marker.GetComponent<SpawnMarker>().SetMaterial(lockedMarkerMaterial);
            else
                marker.GetComponent<SpawnMarker>().SetMaterial(defaultMarkerMaterial); 
        }
    }



    public WaveSettings GetWaveSettingsForIndex(int index = 99)
    {
        if (index == 99)
            return _waveSettingsList[selectedLineIndex];
        return _waveSettingsList[index];
    }

    public bool IsActiveLineLocked()
    {
        return _lockedMarkers.Contains(selectedLineIndex);
    }

    public void SetSelectedLineLockValue(bool nowLocked)
    {
        if (nowLocked == false)
        {
            if (_lockedMarkers.Contains(selectedLineIndex))
                _lockedMarkers.Remove(selectedLineIndex);
        }
        else
        {
            if (_lockedMarkers.Contains(selectedLineIndex) == false)
                _lockedMarkers.Add(selectedLineIndex);

        }
       
    }
}
