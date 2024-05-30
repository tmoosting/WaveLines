using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SpecialDropdown : DropdownField
    {
        public SpecialDropdown()
        {
            style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            var body = this.Q<VisualElement>(null, "unity-base-field__input");
            if (body != null)
            {
                body.style.backgroundColor = Color.clear;
            }
        }
       
    }
}
