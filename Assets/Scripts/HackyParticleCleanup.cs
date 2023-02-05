using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackyParticleCleanup : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
