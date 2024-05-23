using UnityEngine.UIElements;

namespace UI
{
    public class SectionGlobal : VisualElement
    {
       private SpecialSlider _lineGlobalSpeedSlider;
        private SpecialSlider _lineGlobalAmplitudeSlider;
        private SpecialSlider _lineGlobalWavelengthSlider;
    
        public SectionGlobal  ()
        { 
            style.width = Length.Percent(100);
            style.height = Length.Percent(100); 

            _lineGlobalSpeedSlider = new SpecialSlider
            {
                label = "Speed: " + WaveController.Instance.waveSpeed.ToString("F2"),
                value = WaveController.Instance.waveSpeed,
                lowValue = WaveController.Instance.speedMinValue,
                highValue =  WaveController.Instance.speedMaxValue,
            };
            _lineGlobalSpeedSlider.RegisterValueChangedCallback(ChangeGlobalSpeedSliderValue);
            Add(_lineGlobalSpeedSlider);
        
            _lineGlobalAmplitudeSlider = new SpecialSlider
            {
                label = "Amplitude: " + WaveController.Instance.waveAmplitude.ToString("F2"),
                value = WaveController.Instance.waveAmplitude,
                lowValue = WaveController.Instance.amplitudeMinValue,
                highValue =  WaveController.Instance.amplitudeMaxValue,
            };
            _lineGlobalAmplitudeSlider.RegisterValueChangedCallback(ChangeGlobalAmplitudeSliderValue);
            Add(_lineGlobalAmplitudeSlider);
        
            _lineGlobalWavelengthSlider = new SpecialSlider
            {
                label = "Wavelength: " + WaveController.Instance.waveLength.ToString("F2"),
                value = WaveController.Instance.waveLength,
                lowValue = WaveController.Instance.wavelengthMinValue,
                highValue =  WaveController.Instance.wavelengthMaxValue,
            };
            _lineGlobalWavelengthSlider.RegisterValueChangedCallback(ChangeGlobalWavelengthSliderValue);
            Add(_lineGlobalWavelengthSlider);
         
        }
    
    
        private void ChangeGlobalSpeedSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetGlobalWaveSpeed(evt.newValue);
        }
   
        private void ChangeGlobalAmplitudeSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetGlobalWaveAmplitude(evt.newValue);

        }
        private void ChangeGlobalWavelengthSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetGlobalWaveWavelength(evt.newValue);

        }
    }
}
