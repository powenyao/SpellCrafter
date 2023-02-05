using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary>
/// Core is a class using the Singleton Design Pattern. It is meant as the one stop for your common needs.
/// Under Core, there are a variety of Modules (may be called Manager or Controllers) that will perform commonly used
/// functionalities for their respective area. These modules should not contain scene-specific functionalities.  
/// For example, the Audio Module allows other scripts to adjust volume levels or request a new song.
/// A in-game VR music player with buttons would go through Core's Audio Module to play music and adjust volume, but
/// it will have code for responding to button pushes in a separate script.
/// 
/// Setup Note:
/// In Project Setting, Script Execution Order, Core should be very early to ensure it is available for other scripts.
/// Otherwise Public variables may not be assigned in inspectors when others try to interact with it in Awake
/// </summary>
public class Core : MonoBehaviour
{
    #region Singleton Setup

    public static Core Ins { get; private set; } = null;

    private void Awake()
    {
        // if the static reference to singleton has already been initialized somewhere AND it's not this one, then this
        // GameObject is a duplicate and should not exist
        if (Ins != null && Ins != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Ins = this;
            //So this singleton will stay when we change scenes.
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion Singleton Setup

    public void Start()
    {
#if UNITY_EDITOR
        SceneVisibilityManager.instance.DisablePicking(this.gameObject, true);
#endif
        //SystemMenu.LoadModule();

        //SceneManager.activeSceneChanged += HandleSceneChange;
    }

    private void HandleSceneChange(Scene arg0, Scene arg1)
    {
        Dev.Log("[Core] Handle Scene Change");
        this.Subservices.ClearSubservices();
    }

    // [Header("Sensory Services & Settings")]
    // [Tooltip("Manages Audio Volume, play Audio Clip, etc")]
    // public Controller_Audio AudioManager;
    //
    // [Tooltip("Manages incoming audio, record sound, etc")]
    // public Manager_Microphone Microphone;
    //
    // [Tooltip("Manages visual effects such as fade in, crossfade, brightness")]
    // public Controller_Visual VisualManager;
    //
    [Tooltip("Manages UI such as popup, references for UI values in one handy location")]
    public Controller_UIEffects UIEffectsManager;
    //
    // public Controller_Screenshot ScreenshotManager;
    //
    // public Manager_Privacy Privacy;
    //
    // [Header("User Services")]
    // [Tooltip("Manages deployment of User Avatars")]
    // public Manager_Avatar Avatar;
    //
    // [Tooltip("Manages User Account Information")]
    // public Manager_Account Account;
    //
    // [Tooltip("")]
    // public Manager_User User;
    //
    // [Tooltip("")]
    // public Controller_HumanScale HumanScaleManager;
    //
    // [Header("XR Services")]
    // public Controller_XR XRManager;
    //
    // public Manager_VES VES;
    // public Manager_SystemMenu SystemMenu;
    //
    // [Header("Support Services")]
    // [Tooltip("Manages scene transition")]
    // public Controller_Scene SceneManager;
    //
    // public Manager_InGameMessages Messages;
    public Manager_Debug Debug;
    // public Manager_DataCollection DataCollection;
    //
    // [Tooltip("")]
    // public Controller_Scenario ScenarioManager;

    [Header("Additional Services")]
    public Manager_Subservices Subservices;
}