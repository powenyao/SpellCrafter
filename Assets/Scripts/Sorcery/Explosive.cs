using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [Tooltip("Positive for explosion, negative for blackhole.")]
    public float force = -16f;
    [Tooltip("How long to apply the force, in seconds.")]
    public float duration = 3f;
    [Tooltip("Radius within which bodies are affected.")]
    public float radius = 5f;

    public bool isActive { get; private set; } = false;
    public float elapsed { get; private set; } = float.PositiveInfinity;
    public List<GameObject> targetObjects { get; private set; }
    public List<Rigidbody> targetRbs { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        targetObjects = new List<GameObject>();
        targetRbs = new List<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Trigger")]
    void Trigger()
    {
        // Already active, do nothing
        if (isActive)
            return;

        elapsed = 0;

        targetObjects.Clear();
        targetRbs.Clear();
        foreach (Collider c in Physics.OverlapSphere(transform.position, radius))
        {
            c.TryGetComponent<Rigidbody>(out Rigidbody rb);
            if (rb)
            {
                targetObjects.Add(c.gameObject);
                targetRbs.Add(rb);
            }
        }

        isActive = true;
    }

    void FixedUpdate()
    {
        if (!isActive)
            return;

        elapsed += Time.fixedDeltaTime;
        if (elapsed < duration)
        {
            foreach(Rigidbody rb in targetRbs)
            {
                // No need to set explosion radius here, as we already filtered affected objects during trigger
                rb.AddExplosionForce(force, transform.position, 0, 0, ForceMode.Impulse);
            }
        }
        else
        {
            isActive = false;
            elapsed = float.PositiveInfinity;
        }
    }
}
