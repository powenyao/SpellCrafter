using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void OnSpellLaunchedHandler(float cost);

public class LauncherController : MonoBehaviour
{
    [SerializeField]
    InputActionReference castAction;

    [SerializeField]
    LauncherBase _launcher;

    [SerializeField]
    CraftingMenu craftingMenu;

    public event OnSpellLaunchedHandler OnSpellLaunched;

    private float lastSpellCost = 1f;

    void Awake()
    {
        craftingMenu.OnSpellPrepped += composition =>
        {
            lastSpellCost = 1 + 0.5f * composition.GetEffects().Count;
            lastSpellCost += composition.GetTracking() == Enum_SpellComponents_Tracking.None ? 0 : 0.5f;
            lastSpellCost += composition.GetTrigger() == Enum_SpellComponents_Trigger.None ? 0 : 0.5f;
            lastSpellCost += composition.GetPath() == Enum_SpellComponents_Path.None ? 0 : 0.5f;
        };
    }

    void OnEnable()
    {
        castAction.action.performed += CastPerformedHandler;
    }

    void OnDisable()
    {
        castAction.action.performed -= CastPerformedHandler;
    }

    private void CastPerformedHandler(InputAction.CallbackContext obj)
    {
        //Debug.Log("Attempting to launch!");

        if (_launcher != null)
        {
            _launcher.TryLaunch();
            OnSpellLaunched?.Invoke(lastSpellCost);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
