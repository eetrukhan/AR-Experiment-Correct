using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerSceneController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private HMDAdministratedTaskController HMDAdministeredTask;
    [SerializeField]
    private VideoTaskController videoTask;
#pragma warning restore 649

    private void Start()
    {
        if (HMDAdministeredTask != null && HMDAdministeredTask.gameObject.activeSelf)
        {
            HMDAdministeredTask.GenerateFiled();
            HMDAdministeredTask.Play();
        }

        if (videoTask != null)
            videoTask.Shuffle();
    }

    private void Update()
    {
        if (videoTask == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
            videoTask.Play();

        if (Input.GetKeyDown(KeyCode.P))
            videoTask.PrepareFragment();
    }
}
