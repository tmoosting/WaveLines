using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    
    
    
    public class SectionSetup : VisualElement
    {
        private SpecialSlider _pointsSlider; 
        private SpecialSlider _heightSlider; 
        private SpecialSlider _lineAmountSlider; 
        private SpecialSlider _horizontalSpaceSlider; 
        private SpecialSlider _verticalSpaceSlider; 
        private SpecialSlider _lengthSlider; 
        private SpecialSlider _diameterSlider;
        
        private SpecialButton _rebuildButton;



        public SectionSetup()
        {
            style.width = Length.Percent(100);
            style.height = Length.Percent(100);

    

            _heightSlider = new SpecialSlider
            {
                label = "Height " + WaveController.Instance.baseHeight.ToString("F2"),
                value = WaveController.Instance.baseHeight,
                lowValue = WaveController.Instance.heightMinValue,
                highValue = WaveController.Instance.heightMaxValue,
            }; 
            _heightSlider.RegisterValueChangedCallback(ChangeHeightSliderValue);
            Add(_heightSlider);
            
            _lineAmountSlider = new SpecialSlider
            {
                label = "Lines " + WaveController.Instance.lineAmount.ToString("F2"),
                value = WaveController.Instance.lineAmount,
                lowValue = WaveController.Instance.lineCountMinValue,
                highValue = WaveController.Instance.lineCountMaxValue,
            };
            _lineAmountSlider.RegisterValueChangedCallback(ChangeLineAmountSliderValue);
            Add(_lineAmountSlider);
            
            _horizontalSpaceSlider = new SpecialSlider
            {
                label = "Horiz. Space " + WaveController.Instance.lineHorizontalSpacing.ToString("F2"),
                value = WaveController.Instance.lineHorizontalSpacing,
                lowValue = WaveController.Instance.horizontalSpaceMinValue,
                highValue = WaveController.Instance.horizontalSpaceMaxValue,
            };
            _horizontalSpaceSlider.RegisterValueChangedCallback(ChangeHorizontalSpaceSliderValue);
            Add(_horizontalSpaceSlider);
            
            _verticalSpaceSlider = new SpecialSlider
            {
                label = "Vert. Space " + WaveController.Instance.lineVerticalSpacing.ToString("F2"),
                value = WaveController.Instance.lineVerticalSpacing,
                lowValue = WaveController.Instance.verticalSpaceMinValue,
                highValue = WaveController.Instance.verticalSpaceMaxValue,
            };
            _verticalSpaceSlider.RegisterValueChangedCallback(ChangeVerticalSpaceSliderValue);
            Add(_verticalSpaceSlider);
            
            _lengthSlider = new SpecialSlider
            {
                label = "Length " + WaveController.Instance.lineLength.ToString("F2"),
                value = WaveController.Instance.lineLength,
                lowValue = WaveController.Instance.lengthMinValue,
                highValue = WaveController.Instance.lengthMaxValue,
            };
            _lengthSlider.RegisterValueChangedCallback(ChangeLengthSliderValue);
            Add(_lengthSlider);
            
            _diameterSlider = new SpecialSlider
            {
                label = "Diameter " + WaveController.Instance.lineDiameter.ToString("F2"),
                value = WaveController.Instance.lineDiameter,
                lowValue = WaveController.Instance.diameterMinValue,
                highValue = WaveController.Instance.diameterMaxValue,
            };
            _diameterSlider.RegisterValueChangedCallback(ChangeDiameterSliderValue);
            Add(_diameterSlider);

            _pointsSlider = new SpecialSlider
            {
                label = "Points On Line " + WaveController.Instance.pointCount.ToString("F2"),
                value = WaveController.Instance.pointCount,
                lowValue = WaveController.Instance.pointCountMinValue,
                highValue = WaveController.Instance.pointCountMaxValue,
            };
            _pointsSlider.RegisterValueChangedCallback(ChangePointSliderValue);
            Add(_pointsSlider);

            _rebuildButton = new SpecialButton();
            _rebuildButton.clicked -= ClickRebuildButton;
            _rebuildButton.clicked += ClickRebuildButton;
            _rebuildButton.text = "Respawn";
            _rebuildButton.style.marginTop = 10; 
            _rebuildButton.style.width = 120f; 
            Add(_rebuildButton);
        }

        private void ClickRebuildButton()
        {
            WaveController.Instance.Rebuild();
        }

        private void ChangeHeightSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetHeight(evt.newValue);
            _heightSlider.label = "Height " + WaveController.Instance.baseHeight.ToString("F2");
        }
           
        private void ChangeLineAmountSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetLinesAmount((int)evt.newValue);
            _lineAmountSlider.label = "Lines " + WaveController.Instance.lineAmount.ToString("F2");
        } 
           
        private void ChangeHorizontalSpaceSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetHorizontalSpaceAmount(evt.newValue);
            _horizontalSpaceSlider.label = "Horiz. Space " + WaveController.Instance.lineHorizontalSpacing.ToString("F2");
        }
           
        private void ChangeVerticalSpaceSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetVerticalSpaceAmount(evt.newValue);
            _verticalSpaceSlider.label = "Vert. Space " + WaveController.Instance.lineVerticalSpacing.ToString("F2");
        }
           
        private void ChangeLengthSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetLineLength(evt.newValue);
            _lengthSlider.label = "Length " + WaveController.Instance.lineLength.ToString("F2");
        }
           
        private void ChangeDiameterSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetLineDiameter(evt.newValue);
            _diameterSlider.label = "Diameter " + WaveController.Instance.lineDiameter.ToString("F2");
        }
        private void ChangePointSliderValue(ChangeEvent<float> evt)
        {
            WaveController.Instance.SetPointAmount((int)evt.newValue);
            _pointsSlider.label = "Points On Line " + WaveController.Instance.pointCount.ToString("F2");
        }
        
     
    }
}
