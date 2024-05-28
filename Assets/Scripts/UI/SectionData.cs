using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SectionData : VisualElement
    {
        private SpecialButton _exportButton;
        private SpecialButton _importButton;


        public SectionData()
        {
            
            _exportButton = new SpecialButton();
            _exportButton.clicked -= ClickExportButton;
            _exportButton.clicked += ClickExportButton;
            _exportButton.text = "Export";
            _exportButton.style.marginTop = 10; 
            _exportButton.style.width = 120f; 
            Add(_exportButton);
            
            _importButton = new SpecialButton();
            _importButton.clicked -= ClickImportButton;
            _importButton.clicked += ClickImportButton;
            _importButton.text = "Import";
            _importButton.style.marginTop = 10; 
            _importButton.style.width = 120f; 
            Add(_importButton);
        }

        private void ClickExportButton()
        {
        WaveController.Instance.SaveSettings();
        }
        private void ClickImportButton()
        {
            WaveController.Instance.LoadSettings();

        }
    }
}
