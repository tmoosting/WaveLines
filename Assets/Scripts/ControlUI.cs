using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlUI : MonoBehaviour
{
    
    // Setup  (applies to all, regardless of lock state)
    // pointcount: slider 
    // baseheight: slider
    // lineAmount: slider
    // lineHorizontalSpacing: slider
    // lineVerticalSpacing: slider
    // linelength: slider
    // linediameter: slider
    
    
    // Global Lines (applies to unlocked only)
    // button: unlock all
    

    // speed   slider
    // amplitude  slider
    // wavelength  slider

   
 
    // Specific Line - can be locked to exclude from global sliders
    // toggle: lock line
 
    
    // linelength: slider
    // linediameter: slider
    // speed   slider
    // amplitude  slider
    // wavelength  slider
    
   

    
    
    

    public Color textFieldColor;

    public bool showWindow = true;
    private UIDocument uiDocument;
    private VisualElement window;
    private VisualElement content;

    private VisualElement tabRow;

    private int _tabIndex = 0;

    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        window = uiDocument.rootVisualElement.Q("WindowBase");

        BuildWindow(); 
    }

    public void ShowWindow(bool show)
    {
        showWindow = show;
        if (showWindow)
            uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        else
            uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

    }
    private void BuildWindow()
    {
        content = new VisualElement();
        content.style.width = Length.Percent(98);
       content.style.height = 500;
    //   content.style.height = Length.Percent(98);
        content.style.marginTop = 2f;
        content.style.marginLeft = 3f;
        content.style.marginRight = 5f;

        BuildTabRow();
        RefreshTabRow();
        
        window.Add(content);

        RefreshWindow(2);
    }

    private void BuildTabRow()
    {
        // Create the tab row container
        tabRow = new VisualElement
        {
            name = "TabRow",
            style =
            {
                flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween),
                paddingTop = 5,
                paddingBottom = 5,
                backgroundColor = Color.clear
            }
        };
        
        window.Add(tabRow); 
    }
    private void RefreshTabRow()
    { 
        tabRow.Clear();
        // Create tabs
        string[] tabNames = { "Setup","Data" , "Global", "Line ", };
        for (int i = 0; i < tabNames.Length; i++)
        {
            int tabIndex = i;
            string tabName = tabNames[i];
            if (  i == 3 )
                tabName = "Line " + WaveController.Instance.selectedLineIndex;
            
            var tabButton = new Button(() => RefreshWindow(tabIndex))
            {
                text = tabName,
                style =
                {
                    flexGrow = 1,
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    fontSize = 16,
                    paddingTop = 10,
                    paddingBottom = 10,
                    backgroundColor = Color.clear,
                    marginLeft =   5  ,  
                    borderBottomColor = new StyleColor(Color.black),
                    borderBottomWidth = 1,
                    borderTopWidth = 0,
                    borderLeftWidth = 0,
                    borderRightWidth = 0
                }
            };

            tabRow.Add(tabButton);
        }
    }

    public void RefreshWindow(int tabIndex )
    {
        RefreshTabRow();

         content.Clear();
         content.Add(BuildWindowContent(tabIndex));
    }

    private VisualElement BuildWindowContent(int tabIndex)
    {  
        if (tabIndex == 0)
            return BuildSetupContent();
        if (tabIndex == 1)
            return BuildDataContent();
        if (tabIndex == 2)
            return BuildGlobalContent();
        if (tabIndex == 3)
            return BuildLineContent();
        return null;
    }

    private VisualElement BuildSetupContent()
    {
        VisualElement returnElement = new VisualElement();
        returnElement.style.width = Length.Percent(100);
        returnElement.style.height = Length.Percent(100);
       // returnElement.style.backgroundColor = Color.yellow;
        
        return returnElement;
    }

    private VisualElement BuildGlobalContent()
    {
        VisualElement returnElement = new VisualElement();
        returnElement.style.width = Length.Percent(100);
        returnElement.style.height = Length.Percent(100);
      //  returnElement.style.backgroundColor = Color.white;

        
        return returnElement;
    }

    private VisualElement BuildLineContent()
    {
        VisualElement returnElement = new VisualElement();
        returnElement.style.width = Length.Percent(100);
        returnElement.style.height = Length.Percent(100);

        returnElement.style.backgroundColor = Color.yellow;

        return returnElement;
    }

    private VisualElement BuildDataContent()
    {
        VisualElement returnElement = new VisualElement();
        returnElement.style.width = Length.Percent(100);
        returnElement.style.height = Length.Percent(100);
    //    returnElement.style.backgroundColor = Color.green;

        
        return returnElement;
    }


    private void ClickUnlockAllButton()
    { 
    
    }
   
}