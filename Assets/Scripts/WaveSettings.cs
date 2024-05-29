using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// data object, one for each line
[System.Serializable]
public class WaveSettings
{
    public int Index;
    public float WaveSpeed; // how fast the wave travels 
    public float WaveAmplitude; // distance from center to max extent
    public float WaveLength; // length of one wave 
    public Color LineColor;

    public WaveSettings(int givenIndex, float speed, float amplitude, float length)
    {
        Index = givenIndex;
        WaveSpeed = speed;
        WaveAmplitude = amplitude;
        WaveLength = length;

    }
}
