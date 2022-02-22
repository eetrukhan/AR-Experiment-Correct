using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHighlight : MonoBehaviour
{
    public GameObject sprite;
    
    public void OnEnter()
    {
        sprite.GetComponent<SpriteRenderer>().material.SetColor("_Color",new Color(0.5f,0.5f,1));
    }

    public void OnExit()
    {
        sprite.GetComponent<SpriteRenderer>().material.SetColor("_Color",new Color(0,0,1));
    }
}
