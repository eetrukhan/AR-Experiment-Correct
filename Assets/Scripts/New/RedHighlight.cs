using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHighlight : MonoBehaviour
{
    public GameObject sprite;

    public void OnEnter()
    {
        sprite.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 0.5f, 0.5f));
    }

    public void OnExit()
    {
        sprite.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 0, 0));
    }
}
