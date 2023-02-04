using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AspectSelector : MonoBehaviour
{
    public string aspectName;
    public GameObject choiceButtonTemplate;

    public GameObject header;
    public GameObject choicesPanel;

    public event UnityAction<AspectSelector> OnCurrentSelectionsChanged;
    public List<string> CurrentSelections { get; private set; }

    void Awake()
    {
        // Set panel header
        var headerText = header.GetComponent<TextMeshProUGUI>();
        headerText.text = aspectName;
        
        // Instantiate panel options
        ToggleGroup toggleGroup = choicesPanel.GetComponent<ToggleGroup>();
        var options = CraftingAspects.GetOptions(aspectName);
        CurrentSelections = new List<string>(options.Length);

        foreach (string option in options)
        {
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
            label.text = option;
        }
    }

    void OnToggleChangedHandler(string name, bool isOn)
    {
        // Debug.Log($"Selection changed: {aspectName}: {name} to {isOn}");

        if (isOn)
        {
            CurrentSelections.Add(name);
        }
        else
        {
            CurrentSelections.Remove(name);
        }

        OnCurrentSelectionsChanged?.Invoke(this);
    }
}
