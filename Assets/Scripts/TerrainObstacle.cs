using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObstacle : Obstacle
{
    protected override void Destruct()
    {
        //Destroy(this.gameObject);
        hp = 100000;
    }
}
