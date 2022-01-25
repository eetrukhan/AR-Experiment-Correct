using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public InputField Value;
    public Button MinusButton;

    void OnEnable()
    {
        MinusButton.interactable = Int32.Parse(Value.text) > 1;
    }

    public void Increase()
    {
        if (!enabled) return;
        int temp = Int32.Parse(Value.text);
        Value.text = (++temp).ToString();
        MinusButton.interactable = true;
    }

    public void Decrease()
    {
        if (!enabled) return;
        int temp = Int32.Parse(Value.text);
        Value.text = (--temp).ToString();
        if (temp == 1)
            MinusButton.interactable = false;
    }
}
