using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void OnSpellLaunchedHandler();

public class LauncherController : MonoBehaviour
{
    [SerializeField]
    InputActionReference castAction;

    [SerializeField]
    LauncherBase _launcher;

    public event OnSpellLaunchedHandler OnSpellLaunched;

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
            OnSpellLaunched?.Invoke();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
