using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour
{
    /**
     * These are the public variables for the code, visible in the editor
     * */
    public GameObject collisionSphere; //this is the sphere the player will use to detect collision with an object
    public float min = 1, layer2 = 4, layer3 = 7, max = 10; //these will hold the minimum and maximum distances from the player to the maximum detecting distance.
    //these colours colour the debug line determined by what range the object collided with is in
    public Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue };
    public bool debug = false; //this turns on and off the debug logs and features.

    /**
     * These are the private variables still visible in the editor
     */
    [SerializeField] private List<GameObject> collidedObjects = new List<GameObject>();


    /**
     * These are ther private variables you cannot see
     */
    private float distance = 0;
    private Color tmp = Color.black;
    private Vector3 thisPosition;

    // Use this for initialization
    void Start()
    {
        //make sure this array is empty
        collidedObjects.Clear();
        thisPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //instead of finding this objects position all the time, we simply put it into a static variable and call it once a frame.
        thisPosition = this.transform.position;

        //for each object we're currently colliding with, we want to check how far away the object is.
        //Depending on what threshold it falls into, we want to do specific defined behaviour 
        for(int index = 0; index < collidedObjects.Count; index++)
        {
            //first we grab the distance between us and the colliding object
            distance = Vector3.Distance(this.transform.position, collidedObjects[index].transform.position);

            //we catch the distance between layers and act accordingly
            if(distance < min)
            {
                //likely death state will be acitvated here

                if(debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[0]);
                }
            }
            else if(distance < layer2)
            {
                //probably max pointage
                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[1]);
                }
            }
            else if(distance < layer3)
            {
                //minor pointage goes here

                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[2]);
                }
            }
            else if(distance < max)
            {
                //likely nothing happens here
                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[3]);
                }
            }
        }


        //we also want to make sure the size of the collider is the same as our max size, otherwise there's not much point in having a max size
        this.transform.localScale = new Vector3(max, max, max);
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
        if (debug)
        {
            Debug.Log("Hit " + go.name + " at " + go.transform.position + "; distance: " + Vector3.Distance(this.transform.position, go.transform.position) +
            " angle: " + Vector3.Angle(this.transform.forward, go.transform.position));
        }

        //now we add the GO to the list
        if (!collidedObjects.Contains(go)) //we make sure we're not somehow adding a duplicate object to the list.
        {
            collidedObjects.Add(go);
        }
    }

    /**
    * Date:             May 5, 2017
    * Author:           Jay Coughlan
    * Interface:        void OnTriggerExit()
    * Description:
    *                   Updates an object when it leaves the collider area.
    */
    private void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;

        if (debug)
        {
            Debug.Log("Removing " + go.name + " at " + go.transform.position + "; distance: " + Vector3.Distance(this.transform.position, go.transform.position));
        }

        if(collidedObjects.Contains(go))//we want to make sure this object wasn't, by some miracle, removed from the list prematurely
        {
            //now we remove the object from the list
            collidedObjects.Remove(go);
        }
    }
}
