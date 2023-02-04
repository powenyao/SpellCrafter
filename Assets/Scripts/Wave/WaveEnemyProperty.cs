using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by both WaveEnemy and SpawnTransform
//denotes whether an enemy belongs to the ground or in the air
//denotes whether a spawn is on the ground or in the air
public enum Enum_SpawnType
{
    Any,
    Ground,
    Aerial
}

public class WaveEnemyProperty : MonoBehaviour
{
    public Enum_SpawnType spawnType = Enum_SpawnType.Ground;
    public Vector3 spawnOffset;
}