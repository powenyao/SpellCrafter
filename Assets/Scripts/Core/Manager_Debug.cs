using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_KeyState
{
    None,
    KeyUp,
    KeyDown,
    Key
}

/// <summary>
/// 
/// </summary>
public struct DebugCodeStructure
{
    public Action Action;
    public string ScriptName;
    public string GameObjectName;
    public string MethodName;
    public Enum_KeyState KeyState;

    public DebugCodeStructure(Action action, string scriptname, string goname, string methodname,
        Enum_KeyState state = Enum_KeyState.KeyUp)
    {
        Action = action;
        ScriptName = scriptname;
        GameObjectName = goname;
        MethodName = methodname;
        KeyState = state;
    }
}

public class Manager_Debug : MonoBehaviour
{
    private Dictionary<KeyCode, DebugCodeStructure> _debugCodeKeyboardKeyUpList = new Dictionary<KeyCode, DebugCodeStructure>();
    private List<DebugCodeStructure> _debugCodeList = new List<DebugCodeStructure>();
    
    public void AddDebugCode(GameObject go, string scriptName, string methodName, Action a)
    {
        _debugCodeList.Add(new DebugCodeStructure(a, scriptName, go.name, methodName));
    }
    
    public void AddDebugCodeForKeyboard(GameObject go, string scriptName, string methodName, KeyCode kc, Action a,
        Enum_KeyState keyState = Enum_KeyState.KeyDown)
    {
        if (_debugCodeKeyboardKeyUpList.ContainsKey(kc))
        {
            Dev.LogWarning("KeyCode " + kc + " from " + go + " " + scriptName + " is already in use by GO " +
                           _debugCodeKeyboardKeyUpList[kc].GameObjectName + " and script " + _debugCodeKeyboardKeyUpList[kc].ScriptName);
        }
        else
        {
            _debugCodeKeyboardKeyUpList.Add(kc, new DebugCodeStructure(a, scriptName, go.name, methodName, keyState));
        }
    }

    public void RemoveDebugCodeForKeyboard(GameObject go, string scriptName, string methodName, KeyCode kc, Action a,
        Enum_KeyState keyState = Enum_KeyState.KeyDown)
    {
        if (_debugCodeKeyboardKeyUpList.ContainsKey(kc))
        {
            
            _debugCodeKeyboardKeyUpList.Remove(kc);
        }
        else
        {
            //_debugCodeKeyboardKeyUpList.Add(kc, new DebugCodeStructure(a, scriptName, go.name, methodName, keyState));
            _debugCodeKeyboardKeyUpList.Remove(kc);
            Dev.LogWarning("KeyCode " + kc + " from " + go + " " + scriptName + " is already in use by GO " +
                           _debugCodeKeyboardKeyUpList[kc].GameObjectName + " and script " + _debugCodeKeyboardKeyUpList[kc].ScriptName);
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        /*
        foreach (var keyValuePair in _debugCodeKeyboardKeyUpList)
        {
            if (keyValuePair.Value.KeyState == Enum_KeyState.KeyUp && Input.GetKeyUp(keyValuePair.Key))
            {
                Dev.Log(keyValuePair.Key.ToString() + " " + keyValuePair.Value.ScriptName + " in GO " +
                        keyValuePair.Value.GameObjectName);
                keyValuePair.Value.Action.Invoke();
            }

            if (keyValuePair.Value.KeyState == Enum_KeyState.Key && Input.GetKey(keyValuePair.Key))
            {
                Dev.Log(keyValuePair.Key.ToString() + " " + keyValuePair.Value.ScriptName + " in GO " +
                        keyValuePair.Value.GameObjectName);
                keyValuePair.Value.Action.Invoke();
            }

            if (keyValuePair.Value.KeyState == Enum_KeyState.KeyDown && Input.GetKeyDown(keyValuePair.Key))
            {
                Dev.Log(keyValuePair.Key.ToString() + " " + keyValuePair.Value.ScriptName + " in GO " +
                        keyValuePair.Value.GameObjectName);
                keyValuePair.Value.Action.Invoke();
            }
        }
        */
    }

    private int debugButtonStartX = 10 + 200*0;
    private int debugButtonStartY = 10;
    private int debugButtonWidth = 200;
    private int debugButtonHeight = 50;

    private bool showDebugCode = true;

    private void OnGUI()
    {
        /*
        int i = 0;
        if (GUI.Button(new Rect(debugButtonStartX, debugButtonStartY + debugButtonHeight * i, debugButtonWidth,
                    debugButtonHeight), "Show/Hide Debug Code " + _debugCodeKeyboardKeyUpList.Count))
        {
            showDebugCode = !showDebugCode;
        }

        if (!showDebugCode)
            return;

        i++;
        foreach (var debugCode in _debugCodeList)
        {
            if (GUI.Button(
                    new Rect(debugButtonStartX, debugButtonStartY + debugButtonHeight * i, debugButtonWidth,
                        debugButtonHeight), debugCode.MethodName))
            {
                debugCode.Action.Invoke();
            }

            i++;
        }
        foreach (var keyValuePair in _debugCodeKeyboardKeyUpList)
        {
            if (GUI.Button(
                    new Rect(debugButtonStartX+debugButtonWidth, debugButtonStartY + debugButtonHeight * i, debugButtonWidth,
                        debugButtonHeight), keyValuePair.Key.ToString() + " : " + keyValuePair.Value.MethodName))
            {
                keyValuePair.Value.Action.Invoke();
            }

            i++;
        }
        */
    }
}