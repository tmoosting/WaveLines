using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

// To Do
  
// selected waveline logic
// ui with:
// global, selected line

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
    
    
    // Lines
    public int pointCount = 100;
    public float baseHeight = 1.5f;
    public int lineAmount = 10; // number of waves total
    public float lineHorizontalSpacing = 2; // horizontal space between waves
    public float lineVerticalSpacing = 1; // distance in height for each wave
    public float lineLength = 12; // length of line in total; needs to always be a complete number of waves!
    public float lineDiameter = 2; // thickness of lines
    // to add: line texture; 
    
    // Waves
    public float waveSpeed; // how fast the wave travels 
    public float waveAmplitude; // distance from center to max extent
    public float waveLength; // length of one wave 
    
    
    // Settings
    public bool spawnStartEndMarkers;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
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


    private void SpawnWaves()
    { 
        SpawnWaveLines();
        ConfigureWaveLines();
    }



    private void SpawnWaveLines()
    {
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
            waveLine.Initialize(index,pointCount, vector3,_startEndPoints [vector3] );
            waveLine.LoadWaveSettings(waveSpeed, waveAmplitude, waveLength, lineLength); 
        }
       
    }




    private void DetermineStartEndPoints()
    {
        _startEndPoints = new Dictionary<Vector3, Vector3>();
        Vector3 startPoint = new Vector3(0, baseHeight, 0);
        Vector3 endPoint = new Vector3(0, baseHeight, lineLength);
        _startEndPoints.Add(startPoint, endPoint);
        for (int i = 1; i < lineAmount; i++)
        {
            Vector3 nextLineStartPoint = startPoint+ new Vector3(i * lineHorizontalSpacing, i * lineVerticalSpacing, 0);
            Vector3 nextLineEndPoint = endPoint+ new Vector3(i * lineHorizontalSpacing, i * lineVerticalSpacing, 0); 
            _startEndPoints.Add(nextLineStartPoint, nextLineEndPoint);
        }
        
    } 
    private void SpawnStartMarkers()
    {
        _lockedMarkers = new List<int>();
        _startSpawnMarkers = new List<GameObject>();
        _endSpawnMarkers = new List<GameObject>();
        foreach (var vector3 in _startEndPoints.Keys)
        {
            int index = _startEndPoints.Keys.ToList().IndexOf(vector3);
            GameObject startSpawn = Instantiate(spawnMarkerStartPrefab);
            startSpawn.transform.position = new Vector3(vector3.x, vector3.y + 0.0f, vector3.z);
            SpawnMarker marker = startSpawn.GetComponent<SpawnMarker>();
            marker.Initialize(index); 
            _startSpawnMarkers.Add(startSpawn);
            
            /*GameObject endSpawn = Instantiate(spawnMarkerEndPrefab);
            endSpawn.transform.position = _startEndPoints[vector3];
            _endSpawnMarkers.Add(endSpawn);*/
        }
    }
}
