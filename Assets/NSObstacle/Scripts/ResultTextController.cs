using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class ResultTextController : MonoBehaviour
{
#pragma warning disable 649
    /*[SerializeField]
    private Transform _volume;
    [SerializeField]
    private bool _placeUnderVolume;*/
    [SerializeField]
    private uint _hideDelay = 5;
#pragma warning restore 649

    private TextMeshPro label;
    private string defaultText;

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
        defaultText = label.text;
    }

    private void OnEnable()
    {
        Invoke("SetDefaultText", _hideDelay);
    }

    private void Update()
    {
        /*if (_volume.hasChanged && _placeUnderVolume)
        {
            float y = (_volume.localScale.x / 2f) + 0.05f;
            transform.localPosition = new Vector3(0f, -y, 0f);
        }*/
    }

    private void SetDefaultText()
    {
        label.text = defaultText;
    }
}
