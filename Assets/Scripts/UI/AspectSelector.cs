using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AspectSelector : MonoBehaviour
{
    public CraftingAspects.AspectName aspectName;
    public GameObject choiceButtonTemplate;

    public GameObject header;
    public GameObject choicesPanel;

    public event UnityAction<AspectSelector> OnCurrentSelectionsChanged;
    public List<string> CurrentSelections { get; private set; }

    void Awake()
    {
        // Set panel header
        var headerText = header.GetComponent<TextMeshProUGUI>();
        headerText.text = aspectName.ToString();

        // Instantiate panel options
        ToggleGroup toggleGroup = choicesPanel.GetComponent<ToggleGroup>();
        var options = CraftingAspects.GetOptions(aspectName);
        CurrentSelections = new List<string>(options.Length);

        //foreach (string option in options)
        for (int i = 0; i < options.Length; i++)
        {
            var option = options[i];
            GameObject choiceObject = GameObject.Instantiate(choiceButtonTemplate);
            choiceObject.transform.SetParent(choicesPanel.transform, false);

            Toggle toggle = choiceObject.GetComponent<Toggle>();
            if (toggleGroup != null)
            {
                // Group is used to force single selection
                toggle.group = toggleGroup;
            }

            toggle.onValueChanged.AddListener(isOn => OnToggleChangedHandler(option, isOn));

            var label = choiceObject.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            //TODO
            //Quick dirty inefficient yolo way to change element option color
            if (aspectName == CraftingAspects.AspectName.Elements)
            {
//                Dev.Log("AspectName: " + aspectName);
                if (Enum.TryParse<Enum_Elements>(option, out Enum_Elements elements))
                {
                    //Method 1
                    var color = Core.Ins.UIEffectsManager.GetColorForElement(elements);
                    color.a = 255;
                    label.color = color;
                    //Method 2, which somehow doesn't show text despite it supposed to be highlighter effect
                    //string colorValues = ColorUtility.ToHtmlStringRGB( color );
                    //option = "<mark=#" + colorValues+ ">" + option + "</mark>";
                }
            }

            label.text = option;
        }
    }

    void OnToggleChangedHandler(string name, bool isOn)
    {
        // Debug.Log($"Selection changed: {aspectName}: {name} to {isOn}");

        if (isOn)
        {
            // Seems toggle fires the event if already selected option is clicked twice
            // Temporary fix for this issue
            if (!CurrentSelections.Contains(name))
                CurrentSelections.Add(name);
            else
                return;
        }
        else
        {
            CurrentSelections.Remove(name);
        }

        OnCurrentSelectionsChanged?.Invoke(this);
    }
}