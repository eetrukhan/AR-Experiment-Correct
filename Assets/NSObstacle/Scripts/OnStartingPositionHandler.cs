using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartingPositionHandler : MonoBehaviour
{
    public StartFrom WhichEndOfTheTrack;

    public AudioSource Audio;
    public AudioClip SoundToPlay;

    public event Action<StartFrom> OnCollisionDetected;
    public event Action OnLeftStartingPosition;

    void Start()
    {
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

        Audio.clip = SoundToPlay;
        Audio.Play();

        OnCollisionDetected(WhichEndOfTheTrack);
    }

    void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        Audio.Stop();

        OnLeftStartingPosition();
    }
}
