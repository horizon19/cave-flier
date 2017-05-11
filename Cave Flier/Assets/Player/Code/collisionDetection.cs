using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour
{
    /**
     * These are the public variables for the code, visible in the editor
     * */
    public GameObject maximumSphere, layer3Sphere, layer2Sphere, minimumSphere; //this is the sphere the player will use to detect collision with an object
    public float min = 1, layer2 = 4, layer3 = 7, max = 10; //these will hold the minimum and maximum distances from the player to the maximum detecting distance.
    //these colours colour the debug line determined by what range the object collided with is in
    public Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue };
    public bool debug = false; //this turns on and off the debug logs and features.

    /**
     * These are the private variables still visible in the editor
     */
    [SerializeField] private List<GameObject> collidedObjects = new List<GameObject>();
    [SerializeField] private List<string> collidedNames = new List<string>();

    /**
     * These are ther private variables you cannot see
     */
    private float distance = 0;
    private Color tmp = Color.black;
    private Vector3 thisPosition;
    private Renderer maxRend, lay3Rend, lay2Rend, minRend;
    private List<float> collidedMinDistances = new List<float>();

    // Use this for initialization
    void Start()
    {
        //make sure this array is empty
        collidedObjects.Clear();
        thisPosition = this.transform.position;

        //we have to grab the renderer components from the game objects we're using to see the bounds of the layers
        maxRend = maximumSphere.GetComponent<Renderer>();
        lay3Rend = layer3Sphere.GetComponent<Renderer>();
        lay2Rend = layer2Sphere.GetComponent<Renderer>();
        minRend = minimumSphere.GetComponent<Renderer>();

        //we turn off the collider rendering during gameplay if we're not debugging
        if (!debug)
        {
            maxRend.enabled = false;
            lay3Rend.enabled = false;
            lay2Rend.enabled = false;
            minRend.enabled = false;
        }
        else //if debug is active, we want to turn them on
        {
            maxRend.enabled = true;
            lay3Rend.enabled = true;
            lay2Rend.enabled = true;
            minRend.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //instead of finding this objects position all the time, we simply put it into a static variable and call it once a frame.
        thisPosition = this.transform.position;

        //we are going to clamp the values of the layers now to make sure they fit appropriately within each other
        min = Mathf.Clamp(min, 0, layer2 - 0.01f);
        layer2 = Mathf.Clamp(layer2, min + 0.01f, layer3 - 0.01f);
        layer3 = Mathf.Clamp(layer3, layer2 + 0.01f, max - 0.01f);
        max = Mathf.Max(layer2 + 0.01f, max);

        //for each object we're currently colliding with, we want to check how far away the object is.
        //Depending on what threshold it falls into, we want to do specific defined behaviour 
        for (int index = 0; index < collidedObjects.Count; index++)
        {
            //first we grab the distance between us and the colliding object
            distance = Vector3.Distance(this.transform.position, collidedObjects[index].transform.position);

            //this is the minimum distance, and will be used to determine the result on OnTriggerExit()
            if (distance < collidedMinDistances[index])
            {
                collidedMinDistances[index] = distance;
            }

            //we catch the distance between layers and act accordingly
            //with the exception of death, you likely won't want to put much code in this logic, because it will ocurr on every update
            //for most logic you want to put it into the "OnTriggerExit()" method, so it occurs once.
            //if the object is within the minimum bounds.
            if (distance < min)
            {
                //likely death state will be acitvated here

                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[0]);
                }
            }
            //if the object is within layer 2
            else if (distance < layer2)
            {
                //probably max pointage
                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[1]);
                }
            }
            //if the object is within layer 3
            else if (distance < layer3)
            {
                //minor pointage goes here

                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[2]);
                }
            }
            //if the object is within the maximum bounds
            else if (distance < max)
            {
                //likely nothing happens here
                if (debug)
                {
                    Debug.DrawLine(thisPosition, collidedObjects[index].transform.position, colors[3]);
                }
            }
        }

        //we turn off the collider rendering during gameplay if we're not debugging
        //we run this code again in update in case the user wants to check debugging mid run time
        //we check if maxRend is enabled, if it is we assume they're all enabled. if it's not we assume everything else is disabled as well.
        //no point in running disabling code on disabled functions
        if (!debug && maxRend.enabled == true)
        {
            maxRend.enabled = false;
            lay3Rend.enabled = false;
            lay2Rend.enabled = false;
            minRend.enabled = false;
        }
        else if (debug && maxRend.enabled == false)//Same as above, but in reverse.
        {
            maxRend.enabled = true;
            lay3Rend.enabled = true;
            lay2Rend.enabled = true;
            minRend.enabled = true;
        }

        //we also want to make sure the size of the collider is the same as our max size, otherwise there's not much point in having a max size
        maximumSphere.transform.localScale = new Vector3(max * 2, max * 2, max * 2);
        layer2Sphere.transform.localScale = new Vector3(layer2 * 2, layer2 * 2, layer2 * 2);
        layer3Sphere.transform.localScale = new Vector3(layer3 * 2, layer3 * 2, layer3 * 2);
        minimumSphere.transform.localScale = new Vector3(min * 2, min * 2, min * 2);
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
        //we don't want to react if the collided object is the player.
        if (other.gameObject.tag != "Player")
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
                //we're adding it's distance here, and then referencing the object's index in it's own list to remove these distances later.
                collidedMinDistances.Add(Vector3.Distance(this.transform.position, go.transform.position));
                //for testing, we're adding the object's name
                collidedNames.Add(go.name);
            }
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
        int index;

        if (debug)
        {
            Debug.Log("Removing " + go.name + " at " + go.transform.position + "; distance: " + Vector3.Distance(this.transform.position, go.transform.position));
        }

        if (collidedObjects.Contains(go))//we want to make sure this object wasn't, by some miracle, removed from the list prematurely
        {
            //first get the objects index
            index = collidedObjects.IndexOf(go);

            //here we decide what we want to do when the object leaves the area
            if (collidedMinDistances[index] < min)
            {
                //within the bounds of our innermost layer. Likely we should not hit this either, as a death should be dealt with in the update loop
            }
            else if (collidedMinDistances[index] > min && collidedMinDistances[index] < layer2)
            {
                //second most inner layer, many many points
            }
            else if (collidedMinDistances[index] > layer2 && collidedMinDistances[index] < layer3)
            {
                //third most inner layer, some points maybe?
            }
            else if (collidedMinDistances[index] > layer3 && collidedMinDistances[index] < max)
            {
                //if it is within the bounds of our outer layer
            }
            else
            {
                //this statement is here just in case, but should never be encountered since the distance would have to be larger than the collider
            }

            //now we remove it's distance
            collidedMinDistances.RemoveAt(index);
            collidedNames.RemoveAt(index);
            //now we remove the object from the list
            collidedObjects.Remove(go);
        }
    }

    class Pair
    {
        GameObject go;
        float distance;
    }
}
