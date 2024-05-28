 
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SpecialSlider : Slider
    {

        public SpecialSlider()
        {
            style.marginTop = 1f;
            var trackerBar = this.Q<VisualElement>("unity-tracker");
            if (trackerBar != null)
            {
                trackerBar.style.backgroundColor = Color.clear;
            }
            var dragger = this.Q<VisualElement>("unity-dragger");
            if (dragger != null)
            {
                dragger.style.backgroundColor = Color.cyan;
            }
        }
   
    }
}
