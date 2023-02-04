using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableProperty
{
    private Dictionary<string, float> properties = new Dictionary<string, float>();

    public void AddProperty(string name, float value)
    {
        if (properties.ContainsKey(name))
        {
            Dev.LogWarning("Property " + name + " is already included with value " + properties[name]);
        }
        else
        {
            properties.Add(name, value);
        }
    }

    public Dictionary<string, float> GetProperties()
    {
        return properties;
    }
}