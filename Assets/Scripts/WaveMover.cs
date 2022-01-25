using System.Collections;
using UnityEngine;

public class WaveMover : MonoBehaviour
{
    public float speedWave;
    public float durationWave;

    private Vector3 finishPos;
    private Vector3 startPos;
    private bool reachedEnd;

    void Start()
    {
        reachedEnd = false;
        startPos = transform.localPosition;
        finishPos = startPos;
        finishPos.x = startPos.x + 50;
        StartCoroutine(DeleteObject(gameObject, durationWave));
    }

    private void Update()
    {
        float posSpeed = Time.deltaTime * speedWave;
        if (!reachedEnd)
        {
            transform.localPosition = Vector3.SlerpUnclamped(transform.localPosition, finishPos, posSpeed);
            if(transform.localPosition.x >= finishPos.x - 0.05f)
            {
                reachedEnd = true;
            }
        }
        else
        {
            transform.localPosition = Vector3.SlerpUnclamped(transform.localPosition, startPos, posSpeed);
            if (transform.localPosition.x <= startPos.x + 0.05f)
            {
                reachedEnd = false;
            }
        }
    }

    IEnumerator DeleteObject(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(obj.transform.parent.gameObject);
    }
}
