using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class _3DButton : MonoBehaviour
{
    [Serializable]
    public class UnityOneArgEvent : UnityEvent<string>
    {
        public UnityOneArgEvent() : base() { }
    }

    public Material HighlightMaterial;

    public string Code = null;

    public UnityOneArgEvent ButtonPressed;

    private Material _defaultMaterial;

    private Transform _button;
    private TextMeshPro _label;
    private Transform _surface;

    void Awake()
    {
        if (HighlightMaterial == null)
        {
            Debug.LogError("Error: The HighlightMaterial field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        _button = transform.Find("Button");
        Transform label = transform.Find("Label");
        _surface = transform.Find("Surface");
        if (!_button || !label || !_surface)
        {
            Debug.LogError("Error: _3DButton couldn't find one of its children. Disabling the script");
            enabled = false;
            return;
        }

        _defaultMaterial = _button.GetComponent<MeshRenderer>().material;
        _label = label.GetComponent<TextMeshPro>();

        if (ButtonPressed == null)
            ButtonPressed = new UnityOneArgEvent();
    }

    void OnEnable()
    {
        _button.GetComponent<MeshRenderer>().material = _defaultMaterial;
    }

    public void PointerEnter()
    {
        if (!enabled) return;

        _button.GetComponent<MeshRenderer>().material = HighlightMaterial;
        ButtonPressed.Invoke(String.IsNullOrEmpty(Code) ? _label.text : Code);
    }

    public void PointerExit()
    {
        if (!enabled) return;

        _button.GetComponent<MeshRenderer>().material = _defaultMaterial;
    }
}
