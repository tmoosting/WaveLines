using UnityEngine;

public class WaveLine : MonoBehaviour
{
    public int lineIndex;
    public float amplitude;
    public float waveLength;
    public float speed;
    public float lineLength;
    public int pointCount;
    private LineRenderer _lineRenderer;
    private Vector3[] _positions;
    private float _offset;

    private bool _waving = false;
 
 
    public void Initialize(int index, int positionCount, Vector3 startPosition, Vector3 endPosition, float diameter,
        float length)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        lineIndex = index;
        pointCount = positionCount;
        lineLength = length;

        // Calculate positions based on start and end points
        _positions = new Vector3[positionCount];
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            _positions[i] = Vector3.Lerp(startPosition, endPosition, t);
        }
        _lineRenderer.positionCount = positionCount;
        _lineRenderer.startWidth = diameter;
        _lineRenderer.endWidth = diameter;
    }
    public void LoadWaveSettings(WaveSettings waveSettings)
    {
        speed = waveSettings.WaveSpeed;
        amplitude = waveSettings.WaveAmplitude;
        waveLength = waveSettings.WaveLength;
        _waving = true;
    }
    
    void Update()
    {
        if (!_waving)
            return;

        _offset += Time.deltaTime * speed;
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            float x = t * lineLength;
            float y = amplitude * Mathf.Sin((2 * Mathf.PI / waveLength) * x + _offset);
            _positions[i].y = y;
        }
        _lineRenderer.SetPositions(_positions);
    }

   
}