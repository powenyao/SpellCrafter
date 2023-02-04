using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTransformProperty : MonoBehaviour
{
    public Enum_SpawnType spawnType = Enum_SpawnType.Aerial;
    
    private Subservice_WaveGenerator _waveGenerator;
    // Start is called before the first frame update
    void Start()
    {
        _waveGenerator = (Subservice_WaveGenerator)Core.Ins.Subservices.GetSubservice(nameof(Subservice_WaveGenerator));
        _waveGenerator.RegisterSpawnTransform(this);
    }
}
