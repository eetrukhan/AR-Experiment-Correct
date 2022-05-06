using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeHider : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            cube.SetActive(true);
        }
        else
        {
            cube.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
