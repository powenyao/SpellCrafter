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

    [Tooltip("The VFX system to configure on activation.")]
    public GameObject vfxSystem;

    public bool isActive { get; private set; } = false;
    public float elapsed { get; private set; } = float.PositiveInfinity;
    public List<GameObject> targetObjects { get; private set; }
    public List<Rigidbody> targetRbs { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        if (targetObjects == null)
        {
            targetObjects = new List<GameObject>();
        }

        if (targetRbs == null)
        {
            targetRbs = new List<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TryTrigger()
    {
//        Dev.Log("[Explosive] TryTrigger");
        Trigger();
    }

    [ContextMenu("Trigger")]
    void Trigger()
    {
        // Already active, do nothing
        if (isActive)
            return;

        isActive = true;
        elapsed = 0;

        //Dev.Log("[Explosive] Trigger");
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

        //Dev.Log("[Explosive] Trigger > " + targetObjects.Count);
        //Dev.Log("[Explosive] Trigger > " + targetRbs.Count);
        if (vfxSystem != null)
        {
            ParticleSystem ps = vfxSystem.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule main = ps.main;
            main.duration = duration;
            main.startSpeed = 2 * radius * Mathf.Sign(force);

            ParticleSystem.ShapeModule shape = ps.shape;
            shape.radius = radius;

            ps.Play();
        }
    }

    void FixedUpdate()
    {
        if (!isActive)
            return;

        elapsed += Time.fixedDeltaTime;
        //Dev.Log("[Explosive] FixedUpdate > elapsed " + elapsed);
        //Dev.Log("[Explosive] FixedUpdate > duration " + duration);
        if (elapsed < duration)
        {
            //Dev.Log("[Explosive] FixedUpdate > targetRbs Count " + targetRbs.Count);
            foreach (Rigidbody rb in targetRbs)
            {
                if (rb != null)
                {
                    // No need to set explosion radius here, as we already filtered affected objects during trigger
                    rb.AddExplosionForce(force, transform.position, 0, 0, ForceMode.Impulse);
                }
            }
        }
        else
        {
            isActive = false;
            elapsed = float.PositiveInfinity;

            Destroy(this.gameObject);
        }
    }
}