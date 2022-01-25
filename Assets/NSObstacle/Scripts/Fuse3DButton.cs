using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Fuse3DButton : MonoBehaviour
{
    [Serializable]
    public class UnityOneArgEvent : UnityEvent<string>
    {
        public UnityOneArgEvent() : base() { }
    }

    [SerializeField]
    protected float _fuseTime = 1f;
    [SerializeField]
    protected string _code = null;
    public UnityOneArgEvent ButtonPressed = new UnityOneArgEvent();

    private Animator _animator;
    private TextMeshPro _label;

    virtual protected void Awake()
    {
        // Sanity check
        if (_fuseTime <= 0)
        {
            Debug.LogError("Fuse3DButton: The 'Fuse Time' value has to be greater than 0. Disabling the script");
            enabled = false;
            return;
        }

        Transform labelTransform = transform.Find("Label");
        if (labelTransform == null || (_label = labelTransform.GetComponent<TextMeshPro>()) == null)
        {
            Debug.LogError("Fuse3DButton: Couldn't find a child named 'Label'. Disabling the script");
            enabled = false;
            return;
        }

        if ((_animator = GetComponentInChildren<Animator>(true)) == null)
        {
            Debug.LogError("Fuse3DButton: Couldn't find an animator component in child objects. Disabling the script");
            enabled = false;
            return;
        }
        
        _animator.speed = 1f / _fuseTime;
    }

    public void PointerEnter()
    {
        if (!enabled) return;

        _animator.SetTrigger("Start Growing");
    }

    public void PointerExit()
    {
        if (!enabled) return;

        _animator.SetTrigger("Back to Doing Nothing");
    }

    // Wonder why it isn't private? Because if it was, there is no way to call it from a StateMachineBehaviour component.
    public void FireButtonPressed()
    {
        ButtonPressed.Invoke(String.IsNullOrEmpty(_code) ? _label.text : _code);
    }
}
