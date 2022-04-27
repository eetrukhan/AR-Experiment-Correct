using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	// Use this for initialization
	/*void Start () {
        DontDestroyOnLoad (gameObject);
	}
	*/
	

	private void Awake()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Anchor");
		if (objs.Length > 1)
		{
			// The 'Awake' method is called on a new instance of the prefab, not on the original one, so we should kill this.gameObject
			Destroy(gameObject);
			return;
		}

		//GetComponent<RsDevice>().enabled = true;
		//DontDestroyOnLoad(gameObject);
	}
	

	// Update is called once per frame
	void Update () {
		
	}
}
