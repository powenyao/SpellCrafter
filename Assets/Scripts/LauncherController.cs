using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LauncherController : MonoBehaviour
{
    [SerializeField]
    InputActionReference castAction;

    [SerializeField]
    LauncherBase _launcher;

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
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
