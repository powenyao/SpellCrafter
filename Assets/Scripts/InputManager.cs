using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset asset;
    public InputActionReference toggleCraftingModeAction;

    public ViewController viewController;
    public GameObject craftingUI;

    private bool craftingMode = false;

    private void Awake()
    {
        toggleCraftingModeAction.action.performed += ToggleCraftingMenuHandler;
    }

    void OnEnable()
    {
        asset.Enable();
        toggleCraftingModeAction.action.Enable();
    }

    void Start()
    {
        // Start in crafting mode by default
        craftingMode = true;
        UpdateActiveControls();
    }

    private void ToggleCraftingMenuHandler(InputAction.CallbackContext obj)
    {
        craftingMode = !craftingMode;
        UpdateActiveControls();
    }

    private void UpdateActiveControls()
    {
        viewController.enabled = !craftingMode;
        craftingUI.SetActive(craftingMode);
    }
}