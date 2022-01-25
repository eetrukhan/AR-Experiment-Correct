using System.Collections;
using UnityEngine;

public class CameraFieldView : MonoBehaviour
{
    private static readonly float DELAY = 1f; // sec

    private void Start()
    {
        StartCoroutine(UpdateFieldOfView());
    }

    private IEnumerator UpdateFieldOfView()
    {
        yield return new WaitForSeconds(DELAY);

        Camera c = Camera.main;
        if (c != null)
            GetComponent<Camera>().fieldOfView = c.fieldOfView;
    }
}