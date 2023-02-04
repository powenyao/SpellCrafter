using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    [SerializeField]
    private float hp = 10;

    [SerializeField]
    private float mp = 0;
    
    private ConsumableProperty property;

    private MeshRenderer _meshRenderer; 
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = this.GetComponent<MeshRenderer>();
        property.AddProperty(ENUM_Character_Stats_Type.Health.ToString(), hp);
        property.AddProperty(ENUM_Character_Stats_Type.Mana.ToString(), mp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
        
    }

    
    public ConsumableProperty Consume()
    {
        this._meshRenderer.enabled = false;
        //
        return property;
    }

    public void OnConsumed()
    {
        Destroy(this.gameObject);
    }
}