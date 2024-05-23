using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SectionLine : VisualElement
    {
       
        private SpecialSlider _lineSpeedSlider;
        private SpecialSlider _lineAmplitudeSlider;
        private SpecialSlider _lineWavelengthSlider;
    
        public    SectionLine()
        { 
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);
        
            _lineSpeedSlider = new SpecialSlider
            {
                label = "Speed: " + WaveController.Instance.GetWaveSettingsForIndex().waveSpeed.ToString("F2"),
                value = WaveController.Instance.GetWaveSettingsForIndex().waveSpeed,
                lowValue = WaveController.Instance.speedMinValue,
                highValue =  WaveController.Instance.speedMaxValue,
            };
            _lineSpeedSlider.RegisterValueChangedCallback(ChangeSpeedSliderValue);
            Add(_lineSpeedSlider);
        
            _lineAmplitudeSlider = new SpecialSlider
            {
                label = "Amplitude: " + WaveController.Instance.GetWaveSettingsForIndex().waveAmplitude.ToString("F2"),
                value = WaveController.Instance.GetWaveSettingsForIndex().waveAmplitude,
                lowValue = WaveController.Instance.amplitudeMinValue,
                highValue =  WaveController.Instance.amplitudeMaxValue,
            };
            _lineAmplitudeSlider.RegisterValueChangedCallback(ChangeAmplitudeSliderValue);
            Add(_lineAmplitudeSlider);
        
            _lineWavelengthSlider = new SpecialSlider
            {
                label = "Wavelength: " + WaveController.Instance.GetWaveSettingsForIndex().waveLength.ToString("F2"),
                value = WaveController.Instance.GetWaveSettingsForIndex().waveLength,
                lowValue = WaveController.Instance.wavelengthMinValue,
                highValue =  WaveController.Instance.wavelengthMaxValue,
            };
            _lineWavelengthSlider.RegisterValueChangedCallback(ChangeWavelengthSliderValue);
            Add(_lineWavelengthSlider);
         
        }
    
        private void ChangeSpeedSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetWaveSpeed(evt.newValue);
            _lineSpeedSlider.label = "Speed: " + WaveController.Instance.GetWaveSettingsForIndex().waveSpeed.ToString("F2");
        }
        private void ChangeAmplitudeSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetWaveAmplitude(evt.newValue);
            _lineAmplitudeSlider.label = "Amplitude: " + WaveController.Instance.GetWaveSettingsForIndex().waveAmplitude.ToString("F2");
        }
        private void ChangeWavelengthSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetWaveWavelength(evt.newValue);
            _lineWavelengthSlider.label = "Wavelength: " + WaveController.Instance.GetWaveSettingsForIndex().waveLength.ToString("F2");
        }
    }
}
