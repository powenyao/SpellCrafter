using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public delegate void Delegate_NewSubserviceRegistered(string name);

public class Manager_Subservices : MonoBehaviour
{
    public static event Delegate_NewSubserviceRegistered EVENT_NewSubserviceRegistered;

    private Dictionary<string, XrosSubservice> _subservices = new Dictionary<string, XrosSubservice>();

    public void RegisterService(string serviceName, XrosSubservice subservice)
    {
        if (_subservices.ContainsKey(serviceName))
        {
            _subservices.Remove(serviceName);
        }
        _subservices.Add(serviceName, subservice);

        //So we have less subservices floating around
        //subservice.transform.SetParent(this.transform);

        Dev.Log("[Manager_Subservice] Register Subservice > " + serviceName + " / " + _subservices.Count);
        EVENT_NewSubserviceRegistered?.Invoke(serviceName);
    }

    public void UnregisterService(string serviceName, XrosSubservice subservice)
    {
        _subservices.Remove(serviceName);
    }

    public XrosSubservice GetSubservice(string serviceName)
    {
        if (_subservices.ContainsKey(serviceName))
        {
            return _subservices[serviceName];    
        }
        else
        {
            Dev.LogWarning("[Manager_Subservice] Get Subservice > "+serviceName + " does not exist");
        }

        return null;
    }

    public void ClearSubservices()
    {
        // foreach (var s in _subservices)
        // {
        //     Destroy(s.transform.gam);
        // }
        
        _subservices.Clear();
    }
}