using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : SpellBase
{
    private Dictionary<IDamageReceiver, float> listOfValidTargets = new Dictionary<IDamageReceiver, float>();
    private float timer = 1f;
    private void OnTriggerEnter(Collider other)
    {
        IDamageReceiver receiver;
        if (other.TryGetComponent<IDamageReceiver>(out receiver))
        {
            //TODO there will be issues with receiver dying but AoE still trying to access it
            listOfValidTargets.Add(receiver, Time.time);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageReceiver receiver;
        if (other.TryGetComponent<IDamageReceiver>(out receiver))
        {
            listOfValidTargets.Remove(receiver);
        }
    }

    private void Update()
    {
        foreach (var kvp  in listOfValidTargets)
        {
            if (kvp.Value + timer < Time.time)
            {
                Dev.Log("Apply Spell");
            }
        }
    }
}