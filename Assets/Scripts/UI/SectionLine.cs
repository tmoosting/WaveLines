using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SectionLine : VisualElement
    {
       
        private SpecialSlider _lineSpeedSlider;
        private SpecialSlider _lineAmplitudeSlider;
        private SpecialSlider _lineWavelengthSlider;

        private Toggle _lockToggle;
    
        public    SectionLine()
        { 
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);

            BuildWaveSettingSliders(); 
            
            _lockToggle = new Toggle();
            _lockToggle.style.width = 200;
            _lockToggle.label = "Lock Wave Settings";
            _lockToggle.value = WaveController.Instance.IsActiveLineLocked();
            _lockToggle.RegisterValueChangedCallback(ToggleLineLock);
            Add(_lockToggle);

        }

        private void ToggleLineLock(ChangeEvent<bool> evt)
        {
            WaveController.Instance.SetShowMarkers(evt.newValue);
        }

        private void BuildWaveSettingSliders()
        {
            _lineSpeedSlider = new SpecialSlider
            {
                label = "Speed: " + WaveController.Instance.GetWaveSettingsForIndex().WaveSpeed.ToString("F2"),
                value = WaveController.Instance.GetWaveSettingsForIndex().WaveSpeed,
                lowValue = WaveController.Instance.speedMinValue,
                highValue =  WaveController.Instance.speedMaxValue,
            };
            _lineSpeedSlider.RegisterValueChangedCallback(ChangeSpeedSliderValue);
            Add(_lineSpeedSlider);
        
            _lineAmplitudeSlider = new SpecialSlider
            {
                label = "Amplitude: " + WaveController.Instance.GetWaveSettingsForIndex().WaveAmplitude.ToString("F2"),
                value = WaveController.Instance.GetWaveSettingsForIndex().WaveAmplitude,
                lowValue = WaveController.Instance.amplitudeMinValue,
                highValue =  WaveController.Instance.amplitudeMaxValue,
            };
            _lineAmplitudeSlider.RegisterValueChangedCallback(ChangeAmplitudeSliderValue);
            Add(_lineAmplitudeSlider);
        
            _lineWavelengthSlider = new SpecialSlider
            {
                label = "Wavelength: " + WaveController.Instance.GetWaveSettingsForIndex().WaveLength.ToString("F2"),
                value = WaveController.Instance.GetWaveSettingsForIndex().WaveLength,
                lowValue = WaveController.Instance.wavelengthMinValue,
                highValue =  WaveController.Instance.wavelengthMaxValue,
            };
            _lineWavelengthSlider.RegisterValueChangedCallback(ChangeWavelengthSliderValue);
            Add(_lineWavelengthSlider);
        }
     
     
        private void ChangeSpeedSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetWaveSpeed(evt.newValue);
            _lineSpeedSlider.label = "Speed: " + WaveController.Instance.GetWaveSettingsForIndex().WaveSpeed.ToString("F2");
        }
        private void ChangeAmplitudeSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetWaveAmplitude(evt.newValue);
            _lineAmplitudeSlider.label = "Amplitude: " + WaveController.Instance.GetWaveSettingsForIndex().WaveAmplitude.ToString("F2");
        }
        private void ChangeWavelengthSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetWaveWavelength(evt.newValue);
            _lineWavelengthSlider.label = "Wavelength: " + WaveController.Instance.GetWaveSettingsForIndex().WaveLength.ToString("F2");
        }
    }
}
