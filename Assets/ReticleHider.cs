using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReticleHider : MonoBehaviour
{
    [SerializeField] private Transform Camera;

    [SerializeField] private GameObject reticle;

    private int _reticleHideAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.transform.rotation.eulerAngles.x < 180 && Camera.transform.rotation.eulerAngles.x >= Mathf.Abs(_reticleHideAngle))
        {
            reticle.SetActive(false);
        }
        else
        {
            if(reticle.activeSelf == false)
                reticle.SetActive(true);
        }
    }
}
