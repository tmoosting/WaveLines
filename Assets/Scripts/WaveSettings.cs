using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// data object, one for each line
public class WaveSettings
{
    public int index;
    public float waveSpeed; // how fast the wave travels 
    public float waveAmplitude; // distance from center to max extent
    public float waveLength; // length of one wave 

    public WaveSettings(int givenIndex, float speed, float amplitude, float length)
    {
        index = givenIndex;
        waveSpeed = speed;
        waveAmplitude = amplitude;
        waveLength = length;

    }
}
