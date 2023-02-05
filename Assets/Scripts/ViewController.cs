using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewController : MonoBehaviour
{
    [Header("Look")]

    [SerializeField]
    InputActionReference lookAction;
    [SerializeField]
    float hSensitivity = 0.2f;
    [SerializeField]
    float vSensitivity = -0.2f;
    [SerializeField]
    GameObject actuators;

    [Header("Move (X Axis)")]
    
    [SerializeField]
    InputActionReference moveAction;
    [SerializeField]
    float moveSpeed = 3f;
    [SerializeField]
    Transform leftBound;
    [SerializeField]
    Transform rightBound;

    private float leftBoundX;
    private float rightBoundX;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        leftBoundX = leftBound ? leftBound.position.x : transform.position.x;
        rightBoundX = rightBound ? rightBound.position.x : transform.position.x;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
//        Debug.LogWarning("Do not move the mouse after clicking 'Play' to start facing the center.");
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

        // Move horizontally
        float moveDelta = moveAction.action.ReadValue<float>();
        if (moveDelta == 0)
            return;
        float moveOffset = moveDelta * moveSpeed * Time.deltaTime;
        float curX = transform.position.x;
        float newX = Mathf.Clamp(curX + moveOffset, leftBoundX, rightBoundX);
        transform.position = transform.position + new Vector3(newX - curX, 0, 0);
    }
}
