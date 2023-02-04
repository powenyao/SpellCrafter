using UnityEngine;
using UnityEngine.Serialization;
//using UnityEngine.XR.Interaction.Toolkit;

//public class MissileLauncher : XrosGrabInteractableSubscriber_Base
public class MissileLauncher : MonoBehaviour
{
    // cool down time
    public float timeBetweenLaunch = 1f;

    [Header("Audio SFX")]
    [SerializeField]
    public AudioSource audioSource;

    public AudioClip clipSFX;

    [Header("Missile Pool")]
    public GameObject missilePrefab;

    public int missilePoolerSize = 20;
    public ObjectPooler MissilePooler;

    private bool _hasLaunched;

    private void Awake()
    {
        if (TryGetComponent(out AudioSource audio))
        {
            audioSource = audio;
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        MissilePooler.Create(missilePrefab, missilePoolerSize);
    }

    // protected override void OnActivated(ActivateEventArgs args)
    // {
    //     Launch();
    // }

    private void Launch()
    {
        // cannot launch during cool down period
        if (_hasLaunched)
            return;

        // spawn missile from pool
        GameObject obj = MissilePooler.SpawnFromPool(transform.position, Quaternion.identity, transform);
        obj.GetComponent<Missile>().launcher = this;

        // play sound effect
        if (clipSFX != null)
        {
            audioSource.PlayOneShot(clipSFX);
        }

        _hasLaunched = true;
        Invoke(nameof(ResetLaunch), timeBetweenLaunch);
    }

    private void ResetLaunch()
    {
        _hasLaunched = false;
    }

    public void ReturnMissileToPool(GameObject obj)
    {
        MissilePooler.ReturnToPool(obj);
    }
}