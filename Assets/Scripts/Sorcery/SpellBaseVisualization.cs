using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBaseVisualization : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private ParticleSystem _particleSystem2;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeElement(Enum_Elements elementType)
    {
        var elementalColor = Core.Ins.UIEffectsManager.ColorDictionaryForElemental[elementType];

        // if (!_renderer)
        // {
        //     _renderer = this.GetComponent<Renderer>();
        // }
        // else
        // {
        //     Dev.LogWarning("Cannot find renderer to ChangeElement to " + elementType.ToString());
        // }

        if (_renderer)
        {
            _renderer.material.SetColor("_BaseColor", elementalColor);
            _renderer.material.SetColor("_EmissionColor", elementalColor * Mathf.LinearToGammaSpace(2f));
            //_renderer.material.SetColor("_EmissionColor",
        }
        else
        {
            Dev.LogWarning("Cannot find renderer to ChangeElement to " + elementType.ToString());
        }

        // if (_particleSystem)
        // {
        //     var main = _particleSystem.main;
        //     main.startColor = elementalColor;
        //     //0, 0
        //     //255, 15.5
        //     //249, 90.9
        //     //0, 100
        //
        //     //37.6
        //     //52.8
        //     //66.5
        //     var time1 = 0.376f;
        //     //var time2 = 0.528f;
        //     var time3 = 0.665f;
        //     Gradient grad = new Gradient();
        //     grad.SetKeys(
        //         new GradientColorKey[]
        //         {
        //             new GradientColorKey(elementalColor, time1), /*new GradientColorKey(Color.white, time2),*/
        //             new GradientColorKey(elementalColor, time3)
        //         },
        //         new GradientAlphaKey[]
        //         {
        //             new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(255.0f, 0.155f),
        //             new GradientAlphaKey(249.0f, 0.909f), new GradientAlphaKey(0f, 1.0f)
        //         });
        //
        //     var col = _particleSystem.colorOverLifetime;
        //     col.color = grad;
        // }
        // else
        // {
        //     Dev.LogWarning("Cannot find _particleSystem to ChangeElement to " + elementType.ToString());
        // }
        //
        // if (_particleSystem2)
        // {
        //     var main = _particleSystem2.main;
        //     main.startColor = elementalColor;
        //     
        //     var time1 = 0.376f;
        //     //var time2 = 0.528f;
        //     var time3 = 0.665f;
        //     Gradient grad = new Gradient();
        //     grad.SetKeys(
        //         new GradientColorKey[]
        //         {
        //             new GradientColorKey(elementalColor, time1), /*new GradientColorKey(Color.white, time2),*/
        //             new GradientColorKey(elementalColor, time3)
        //         },
        //         new GradientAlphaKey[]
        //         {
        //             new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(255.0f, 0.155f),
        //             new GradientAlphaKey(249.0f, 0.909f), new GradientAlphaKey(0f, 1.0f)
        //         });
        //
        //     var col = _particleSystem.colorOverLifetime;
        //     col.color = grad;
        // }
        // else
        // {
        //     Dev.LogWarning("Cannot find _particleSystem2 to ChangeElement to " + elementType.ToString());
        // }
    }
}