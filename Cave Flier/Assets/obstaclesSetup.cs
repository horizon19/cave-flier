using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclesSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.grey;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
