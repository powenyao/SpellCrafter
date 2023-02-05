using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DamageReceiverBase
{
    //[SerializeField]
    //protected new float hp = 100000;

    void Start()
    {
        hp = 99999;
    }
    protected override void Destruct()
    {
        Destroy(this.gameObject);
    }
}
