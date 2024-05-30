using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SpecialButton : Button
    {
        public SpecialButton()
        {
            style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);

            style.backgroundColor = Color.clear;
            style.height = 36f;
        }
    }
}
