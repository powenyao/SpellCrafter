using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewController : MonoBehaviour
{
    [SerializeField]
    InputActionReference lookAction;

    [SerializeField]
    float hSensitivity = 0.2f;
    [SerializeField]
    float vSensitivity = -0.2f;

    [SerializeField]
    GameObject actuators;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Do not move the mouse after clicking 'Play' to start facing the center.");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();

        // For horizontal looking, rotate the entire player
        float hTurn = lookDelta.x * hSensitivity;
        transform.Rotate(0, hTurn, 0);

        // For vertical looking, only rotate the camera and arms (i.e. the actuators)
        float vTurn = lookDelta.y * vSensitivity;
        actuators.transform.Rotate(vTurn, 0, 0);
    }
}
