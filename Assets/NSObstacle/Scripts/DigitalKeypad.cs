using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DigitalKeypad : MonoBehaviour
{
    public TextMeshPro Display;

    public event Action<uint> ValueSubmitted;

    private const int MAX_LENGTH = 9;

    private uint _value;

    void Start()
    {
        if (Display == null)
        {
            Debug.LogError("Error: The Display field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }
    }

    public void OnNumberKeyPressed(string value)
    {
        if (!enabled || Display.text.Length >= MAX_LENGTH) return;

        if (_value == 0)
            Display.text = value;
        else
            Display.text += value;

        _value = UInt32.Parse(Display.text);
    }

    public void OnBackspaceKeyPressed(string value)
    {
        if (!enabled) return;

        if (value != "CLR")
            throw new Exception("The wrong key is associated with the OnBackspaceKeyPressed event handler.");

        if (Display.text.Length == 1)
            Display.text = "0";
        else
            Display.text = TrimLastCharacter(Display.text);
        _value = UInt32.Parse(Display.text);
    }

    public void OnSubmitKeyPressed(string value)
    {
        if (!enabled) return;

        if (value != "SUBMIT")
            throw new Exception("The wrong key is associated with the OnSubmitKeyPressed event handler.");

        ValueSubmitted(_value);
        Clear();
    }

    private void Clear()
    {
        _value = 0;
        Display.text = _value.ToString("D");
    }

    private static string TrimLastCharacter(string str)
    {
        if (String.IsNullOrEmpty(str))
            return str;
        
        return str.Substring(0, str.Length - 1);
    }
}
