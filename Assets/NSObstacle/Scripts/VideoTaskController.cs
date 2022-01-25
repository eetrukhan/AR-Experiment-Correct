using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoTaskController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private VideoClip[] videoFragments;
#pragma warning restore 649

    private VideoPlayer player;
    private GameObject screen;

    private int lastPlayedIndex = -1;
    private int lastPreparedIndex = -1;
    private List<int> indexes = null;

    public string LastVideoName
    {
        get => lastPlayedIndex < 0 ? string.Empty : videoFragments[indexes[lastPlayedIndex]].name;
    }

    protected virtual void Start()
    {
        player = GetComponent<VideoPlayer>();

        // Setting event handlers
        player.loopPointReached += EndReached;

        if ((screen = transform.Find("Screen")?.gameObject) == null)
        {
            Debug.LogError("VideoTaskController: Couldn't find the 'Screen' game object. Disabling the script");
            enabled = false;
            return;
        }
    }

    public virtual void Shuffle()
    {
        if (videoFragments == null || videoFragments.Length == 0)
            throw new Exception("VideoTaskController: Cannot shuffle because the list of video fragments is empty");

        // Creating a sorted list of indices
        indexes = new List<int>(videoFragments.Length);
        for (int i = 0; i < videoFragments.Length; i++)
            indexes.Add(i);

        // Borrowed the idea of shuffling from here: https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
        indexes = indexes.OrderBy(x => UnityEngine.Random.value).ToList();

        // Gotta reset the index
        lastPlayedIndex = -1;
    }

    public virtual void BalancedLatinSquareSort(int orderId) // participantId
    {
        if (videoFragments == null || videoFragments.Length == 0)
            throw new Exception("VideoTaskController: Cannot sort because the list of video fragments is empty");

        // Adapted from here: https://cs.uwaterloo.ca/~dmasson/tools/latin_square/
        indexes = new List<int>(videoFragments.Length);
        for (int i = 0, j = 0, h = 0; i < videoFragments.Length; i++)
        {
            int val = 0;
            if (i < 2 || i % 2 != 0)
            {
                val = j++;
            }
            else
            {
                val = videoFragments.Length - h - 1;
                ++h;
            }

            var idx = (val + orderId) % videoFragments.Length;
            indexes.Add(idx);
        }

        if (videoFragments.Length % 2 != 0 && orderId % 2 != 0)
            indexes.Reverse();

        // Gotta reset the index
        lastPlayedIndex = -1;
    }

    public virtual void PrepareFragment(int fragmentIndex = -1) // TrialNo
    {
        if (!enabled)
            throw new Exception("VideoTaskController: Cannot call the 'PrepareFragment' method on a disabled component");
        if (indexes == null)
            throw new Exception("VideoTaskController: The list of video fragments hasn't beeh sorted yet. Call the 'Shuffle' method first");

        // If fragmentIndex is less then 0, we play the next fragment
        int i = fragmentIndex < 0 ? lastPlayedIndex + 1 : fragmentIndex;
        i %= videoFragments.Length;

        // Prepare it
        player.clip = videoFragments[indexes[i]];
        player.Prepare();

        lastPreparedIndex = i;
    }

    public virtual void Play(int fragmentIndex = -1) // TrialNo
    {
        if (!enabled)
            throw new Exception("VideoTaskController: Cannot call the 'Play' method on a disabled component");
        if (indexes == null)
            throw new Exception("VideoTaskController: The list of video fragments hasn't beeh sorted yet. Call the 'Shuffle' method first");

        // If fragmentIndex is less then 0, we play the next fragment
        int i = fragmentIndex < 0 ? lastPlayedIndex + 1 : fragmentIndex;
        i %= videoFragments.Length;

        // Actually play it
        if (i != lastPreparedIndex)
            player.clip = videoFragments[indexes[i]];
        player.Play();
        screen.SetActive(true);

        lastPlayedIndex = i;
        lastPreparedIndex = -1;
    }

    protected virtual void EndReached(VideoPlayer p)
    {
        screen.SetActive(false);
    }
}
