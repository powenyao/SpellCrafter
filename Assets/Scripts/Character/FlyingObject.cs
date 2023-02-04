using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyingObject : MonoBehaviour
{
    private enum FlyingType
    {
        Circular,
        Vertical,
        Horizontal
    }

    [SerializeField] private FlyingType flyingType = FlyingType.Circular;

    [SerializeField] private float speed;

    [SerializeField] private int size = 1;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var dir = Vector3.zero;
        switch (flyingType)
        {
            case FlyingType.Circular:
                dir.x = Mathf.Cos(Time.time * speed) * size;
                dir.y = Mathf.Sin(Time.time * speed) * size;
                break;
            case FlyingType.Horizontal:
                dir.x = Mathf.Cos(Time.time * speed) * size;
                break;
            case FlyingType.Vertical:
                dir.y = Mathf.Sin(Time.time * speed) * size;
                break;
        }

        _rigidbody.velocity = dir;
    }
}