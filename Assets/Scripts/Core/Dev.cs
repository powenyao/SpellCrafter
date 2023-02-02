using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for Dev tools. Right now only consists of Dev Log feature
///We move the Log feature under our own Dev.Log so we can easily disable/enable the error messages if necessary.
///The messages printed here should go to a log file.
///Note: Print is only available in mohobehavior object. 
/// </summary>
public enum Enum_XYZ
{
    X,
    Y,
    Z
}

public delegate void EventHandler_NewLog(string logMessage);

public static class Dev
{
    public static event EventHandler_NewLog EVENT_NewLog;

    #region DevLog

//Input - use this to track all player Input? This could be expanded into a Log System
//IO - related to file system input, output.
//Unit - anything to do with Unit
//UnitAI - anything to do with Unit decision making
//UI - Unity UI
//ControlGroup - anything to do with CG
//GameMode 
//VR - VR
//Performance - for improving performance
//Utility - utility, tools
//Debug - debug features
//Network - Photon, network related
//Cheat - for showing cheats are activated
//Localization - for localizing languages
//Customization - User Customization, Preference, etc
//Other
    public enum LogCategory
    {
        Input,
        IO,
        Unit,
        UI,
        UnitAI,
        ControlGroup,
        GameMode,
        XR,
        Performance,
        Utility,
        Debug,
        Network,
        Cheat,
        Other,
        Localization,
        Customization,
        Event,
        Camera,
        Tool,
        SteamVR,
        Audio,
        Gameplay
    };


    //Unity LogType
    //Error
    //LogType used for Errors
    //Assert
    //LogType used for Asserts. (These could also indicate an error inside Unity itself.)
    //Warning
    //LogType used for Warnings
    //Log
    //LogType used for regular log Messages
    //Exception
    //LogType used for Exceptions

    public static void Log(object message, LogCategory logCategory = LogCategory.Other)
    {
        //Dev.Log(message, null, logCategory);
        LogActual("[" + logCategory.ToString() + "] " + message, null);
    }

    /*
       public static void Log(object message, Object context, LogCategory logCategory)
       {
           switch (logCategory)
           {
               case LogCategory.Input:
   
                   break;
               case LogCategory.Unit:
   
                   break;
               case LogCategory.UI:
   
                   break;
               case LogCategory.UnitAI:
   
                   break;
               case LogCategory.GameMode:
   
                   break;
               case LogCategory.Performance:
   
                   break;
               case LogCategory.Network:
   
                   break;
               case LogCategory.ControlGroup:
   
                   break;
               case LogCategory.Utility:
   
                   break;
               case LogCategory.Cheat:
   
                   break;
               case LogCategory.IO:
   
                   break;
               case LogCategory.Localization:
   
                   break;
               case LogCategory.Customization:
   
                   break;
               case LogCategory.Event:
   
                   break;
               case LogCategory.Camera:
   
                   break;
               case LogCategory.Audio:
   
                   break;
               default:
                   LogActual("[*" + logCategory.ToString() + "] " + message, context);
                   break;
           }
       }
   */
    private static void LogActual(string s, Object context)
    {
        EVENT_NewLog?.Invoke(s);
        Debug.Log(s);
    }

    public static void LogError(string s, LogCategory c = LogCategory.Other)
    {
        //s = "[" + c.ToString() + "] " + s;
        s = $"[{c}] {s}";
        EVENT_NewLog?.Invoke(s);
        Debug.LogError(s);
    }

    public static void LogWarning(string s, LogCategory c = LogCategory.Other)
    {
        s = $"[{c}] {s}";
        EVENT_NewLog?.Invoke(s);
        Debug.LogWarning(s);
    }

    #endregion Dev Log

    //Helper method that can be called anywhere to check if a variable that should be assigned in Inspector is assigned
    //It does not fix the issue. It's only purpose is to provide warning to the developer.
    public static bool CheckAssignment<T>(T t, Transform go)
    {
        if (t != null) return true;

        var hierarchyPath = go.name;
        do
        {
            go = go.transform.parent;
            hierarchyPath = go.name + "/" + hierarchyPath;
        } while (go.transform.parent != false);

        //hierarchyPath = go.name + "/" + hierarchyPath;
        Dev.LogWarning("Missing Inspector Assignment in " + hierarchyPath);

        return false;
    }

    /// <summary>
    /// To get a full path leading to the component in the Hierarchy
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    public static string GetPath(Transform current)
    {
        // if (current.parent == null)
        //     return "/" + current.name;
        // return current.parent.GetPath() + "/" + current.name;
        var hierarchyPath = current.name;
        do
        {
            current = current.transform.parent;
            if (current)
            {
                hierarchyPath = current.name + "/" + hierarchyPath;    
            }
            else
            {
                break;
            }
        } while (current.transform.parent != false);

        //hierarchyPath = current.name + "/" + hierarchyPath;

        return hierarchyPath;
    }
    
    public static string GetPath(MonoBehaviour mb)
    {
        return GetPath(mb.transform);
    }
}