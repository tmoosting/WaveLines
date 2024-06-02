using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsFile
{
    public int PointCount;
    public float BaseHeight;
    public int LineAmount;
    public float LineHorizontalSpacing;
    public float LineVerticalSpacing;
    public float LineLength;
    public float LineDiameter;
    public float WaveSpeed;
    public float WaveAmplitude;
    public float WaveLength; 
    public List<int> LockedMarkers;
    public List<WaveSettings> WaveSettingsList;
}