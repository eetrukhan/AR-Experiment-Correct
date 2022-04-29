using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderPlacer : MonoBehaviour
{
    private GameObject anchor;
    // Start is called before the first frame update
    void Start()
    {
        anchor = GameObject.FindGameObjectWithTag("Anchor");
        transform.position = anchor.transform.position;
        transform.rotation = anchor.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
