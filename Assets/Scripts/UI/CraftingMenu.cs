using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public GameObject panelContainer;
    public GameObject finalView;

    private AspectSelector[] aspectSelectors;
    private TextMeshProUGUI summaryText;

    void Awake()
    {
        aspectSelectors = panelContainer.transform.GetComponentsInChildren<AspectSelector>();
        foreach (var selector in aspectSelectors)
        {
            selector.OnCurrentSelectionsChanged += OnAspectsChangedHandler;
        }

        summaryText = finalView.GetComponent<TextMeshProUGUI>();
    }

    void OnAspectsChangedHandler(AspectSelector selectorAffected)
    {
        // Update summary text
        string summary = "";
        foreach (var selector in aspectSelectors)
        {
            if (selector.CurrentSelections.Count != 0)
            {
                summary += string.Format("{0}: {1}\n", 
                    selector.aspectName, string.Join(',', selector.CurrentSelections));
            }
        }
        summaryText.text = summary;
    }
}
