using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /**
    * Date:             May 3, 2017
    * Author:           Jay Coughlan
    * Interface:        void OnTriggerEnter ()
    * Description:
    *                   Updates the object when it comes into contact with another object. Temporarily returns the distance, angle, and name of the
    *                   object we've encountered. Future versions will return events based on how close you get.
    */
    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;//the object we've encountered

        Debug.Log("Hit " + go.name + " at " + go.transform.position + "; distance: " + Vector3.Distance(this.transform.position, go.transform.position) + 
            " angle: " + Vector3.Angle(this.transform.forward, go.transform.position));
    }
}
