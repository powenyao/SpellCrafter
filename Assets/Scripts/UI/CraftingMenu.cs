using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class CraftingMenu : MonoBehaviour
{
    public GameObject panelContainer;
    public GameObject finalView;

    private bool selectionChanged = false;
    private Dictionary<CraftingAspects.AspectName, AspectSelector> aspectSelectors;
    private TextMeshProUGUI summaryText;

    private Subservice_Sorcery sorcery;
    void Awake()
    {
        aspectSelectors = new Dictionary<CraftingAspects.AspectName, AspectSelector>();
        foreach (var selector in panelContainer.transform.GetComponentsInChildren<AspectSelector>())
        {
            selector.OnCurrentSelectionsChanged += OnAspectsChangedHandler;
            aspectSelectors.Add(selector.aspectName, selector);
        }

        summaryText = finalView.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        sorcery = (Subservice_Sorcery)Core.Ins.Subservices.GetSubservice(nameof(Subservice_Sorcery));
    }
    
    void OnAspectsChangedHandler(AspectSelector selectorAffected)
    {
        selectionChanged = true;

        // Update summary text
        string summary = "";
        foreach (var selector in aspectSelectors.Values)
        {
            if (selector.CurrentSelections.Count != 0)
            {
                summary += string.Format("{0}: {1}\n", 
                    selector.aspectName, string.Join(',', selector.CurrentSelections));
            }
        }
        summaryText.text = summary;
    }

    void OnDisable()
    {
        if (!selectionChanged)
            return;

        PrepSpellWithAspectValues();
    }

    void PrepSpellWithAspectValues()
    {
        var elementSelector = aspectSelectors[CraftingAspects.AspectName.Elements];
        var shapeSelector = aspectSelectors[CraftingAspects.AspectName.Forms];
        var compSelector = aspectSelectors[CraftingAspects.AspectName.Components];

        SpellComposition composition = new SpellComposition(
            Enum.Parse<Enum_SpellShapes>(shapeSelector.CurrentSelections[0]),
            Enum.Parse<Enum_Elements>(elementSelector.CurrentSelections[0])
        );
        foreach(string item in compSelector.CurrentSelections)
        {
            composition.AddSpellComponent(Enum_SpellComponentCategories.Effects, item);
        }
        
        sorcery.PrepSpellWithComposition("Player", composition, replaceIfExists: true);

        Debug.Log("New spell prepped!");
    }
}
