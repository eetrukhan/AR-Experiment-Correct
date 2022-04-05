using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;

public class StartingButton : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSession()
    {
        GeneratorRunner.startPressed = true;
        canvas.SetActive(false);
    }
}
