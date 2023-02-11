using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackyParticleCleanup : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps;

    // Update is called once per frame
    void Update()
    {
        if (!ps.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
