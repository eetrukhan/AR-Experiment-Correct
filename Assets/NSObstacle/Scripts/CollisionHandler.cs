using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Material CollisionMaterial;

    public GameObject CollisionIndicator;
    public AudioSource Audio;
    public AudioClip SoundToPlay;

    public event Action OnRanIntoGroundObstacle;
    public event Action OnRanIntoHighObstacle;

    void Start()
    {
        if (CollisionMaterial == null)
        {
            Debug.LogError("Error: The CollisionMaterial can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (CollisionIndicator == null)
        {
            Debug.LogError("Error: The CollisionIndicator can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (Audio == null)
        {
            Debug.LogError("Error: The Audio can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (SoundToPlay == null)
        {
            Debug.LogError("Error: The SoundToPlay can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            CollisionIndicator.GetComponent<Renderer>().material = CollisionMaterial;
            CollisionIndicator.SetActive(true);

            Audio.clip = SoundToPlay;
            Audio.Play();

            if (other.gameObject.tag == "GroundObstacle")
                OnRanIntoGroundObstacle();
            else if (other.gameObject.tag == "HighObstacle")
                OnRanIntoHighObstacle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            CollisionIndicator.SetActive(false);
            Audio.Stop();
        }
    }
}
