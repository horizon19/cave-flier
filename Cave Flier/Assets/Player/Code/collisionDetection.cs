/*-----------------------------------------------------------------------
--  SOURCE FILE:    collisionDetection.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                  private void Update()
--                  void OnTriggerEnter(Collider other)
--                  void OnTriggerExit(Collider other)
--                  void OnTriggerStay(Collider other)
--                  float calcCollisionAngle(Vector3 obj)
--                
--  DATE:           May 3, 2017
--
--  DESIGNER:       Jay Coughlan
--
--  NOTES:
--		            This script detects collisions of all objects, determines various information 
--                  about them, and determines what player behavior to run, if any.
----------------------------------------------------------------------------*/
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
    public float headOnRange = 10;
    //these colours colour the debug line determined by what range the object collided with is in

    public bool debug = false; //this turns on and off the debug logs and features.

    /**
* These are the private variables still visible in the editor
* edit:
* **I have taken control of the editor. It shows what -I- choose now. 
* **MWAHAHAHAHAHAHA
*/

    public List<GameObject> collidedObjects = new List<GameObject>();
    public List<string> collidedNames = new List<string>();
    public List<Vector3> collisions = new List<Vector3>();
    public List<float> collisionAngles = new List<float>();
    public List<float> collidedMinDistances = new List<float>();

    public List<GameObject> consumedObjects = new List<GameObject>();
    public List<Vector3> consumedCollisions = new List<Vector3>();
    public List<float> consumedAngles = new List<float>();
    public List<float> consumedDistances = new List<float>();

    /**
* These are ther private variables you cannot see
*/
    private float distance = 0, literalHeadOnRange;
    private Color tmp = Color.black;
    private Vector3 thisPosition, thisForward;

    private Renderer maxRend, lay3Rend, lay2Rend, minRend;
    [SerializeField] private GameObject player;
    private playerMovement pmScript;
    private Ray ray;
    private RaycastHit hit;
    private Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue };

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

        pmScript = (playerMovement)GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent(typeof(playerMovement));

        //set the ray to start at our position
        ray.origin = this.transform.position;
        //set it's direction
        ray.direction = this.transform.forward;
        // literalHeadOnRange = 180 - headOnRange;
    }

    /**
    * Date:             May 3, 2017
    * Author:           Jay Coughlan
    * Interface:        void Update ()
    * Description:
    *                   Update is called once per frame.
    *
    * Revision:			Aing Ragunathan (May 17, 2017) - Added call to playerMovement.cs to update speed when consumable is obtained
    */
    void Update()
    {
        //instead of finding this objects position all the time, we simply put it into a static variable and call it once a frame.
        thisPosition = this.transform.position;
        thisForward = this.transform.forward;

        //we update our raycast
        //set the ray to start at our position
        ray.origin = thisPosition;
        //set it's direction
        ray.direction = thisForward;

        //we are going to clamp the values of the layers now to make sure they fit appropriately within each other
        min = Mathf.Clamp(min, 0, layer2 - 0.01f);
        layer2 = Mathf.Clamp(layer2, min + 0.01f, layer3 - 0.01f);
        layer3 = Mathf.Clamp(layer3, layer2 + 0.01f, max - 0.01f);
        max = Mathf.Max(layer2 + 0.01f, max);

        //now we need to make sure we're testing the proper headOnRange
        //literalHeadOnRange = 180 - headOnRange;
        if (debug)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
        }

        if (Physics.Raycast(ray, out hit, Vector3.Magnitude(ray.direction)))
        {
            switch (hit.collider.tag)
            {
                case "Entrance":
                //so far entrance behavior is the same as obstacle, so we flow through
                case "Walls":
                //so far wall behavior is the same as obstacle so we flow through
                case "Obstacle":
                    //show off our fancy raycast skillz and turn it red.
                    if (debug)
                    {
                        Debug.LogWarning("The raycast has hit " + hit.collider.name);
                        Debug.DrawRay(ray.origin, ray.direction, Color.red);
                    }
                    //if the raycast has hit (and if we're here, it has)
                    //you ded son

                    pmScript.lowerHealth(pmScript.getHealth());
                    break;
                case "Start Volume":
                    break;
                case "End Volume":
                    break;
                default:
                    //if we hit anything that's not walls, the entrance, or an obstacle, turn it yellow, but continue.
                    if (debug)
                    {
                        Debug.LogWarning("The raycast has hit " + hit.collider.name);
                        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                    }
                    break;
            }

        }
        //for each object we're currently colliding with, we want to check how far away the object is.
        //Depending on what threshold it falls into, we want to do specific defined behaviour 
        for (int index = 0; index < collidedObjects.Count; index++)
        {
            //first we grab the distance between us and the colliding object
            distance = Vector3.Distance(thisPosition, collisions[index]);//.transform.position);

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
                    Debug.DrawLine(thisPosition, collisions[index], colors[0]);
                }
                //here we will start with damaging code
                pmScript.lowerHealth(1);
            }
            //if the object is within layer 2

            else if (distance < layer2)
            {
                //probably max pointage
                if (debug)
                {
                    Debug.DrawLine(thisPosition, collisions[index], colors[1]);
                }
            }
            //if the object is within layer 3
            else if (distance < layer3)
            {
                //minor pointage goes here

                if (debug)
                {
                    Debug.DrawLine(thisPosition, collisions[index], colors[2]);
                }
            }
            //if the object is within the maximum bounds
            else if (distance < max)
            {
                //likely nothing happens here
                if (debug)
                {
                    Debug.DrawLine(thisPosition, collisions[index], colors[3]);
                }
            }
            else
            {
                Debug.DrawLine(thisPosition, collisions[index], Color.cyan);
            }
        }

        //for each object we're currently colliding with, we want to check how far away the object is.
        //Depending on what threshold it falls into, we want to do specific defined behaviour 
        for (int index = 0; index < consumedObjects.Count; index++)
        {
            //first we grab the distance between us and the colliding object
            distance = Vector3.Distance(thisPosition, consumedCollisions[index]);//.transform.position);

            //this is the minimum distance, and will be used to determine the result on OnTriggerExit()
            /* if (distance < collidedMinDistances[index])
            {
                collidedMinDistances[index] = distance;
            }*/

            //we catch the distance between layers and act accordingly
            //with the exception of death, you likely won't want to put much code in this logic, because it will ocurr on every update
            //for most logic you want to put it into the "OnTriggerExit()" method, so it occurs once.
            //if the object is within the minimum bounds.
            if (distance < min)
            {
                //likely death state will be acitvated here
                if (debug)
                {
                    Debug.DrawLine(thisPosition, consumedCollisions[index], colors[0]);
                }

                //here we will start with damaging code
                //pmScript.lowerHealth(1);

                //change player's properties depneding on the consumable object
                pmScript.consume(consumedObjects[index]);    //increase speed of player and disable it
                consumedCollisions.RemoveAt(index);
                consumedAngles.RemoveAt(index);
                consumedDistances.RemoveAt(index);
                consumedObjects.RemoveAt(index);
                index--;
            }
            //if the object is within layer 2
            else if (distance < layer2)
            {
                //probably max pointage
                if (debug)
                {
                    Debug.DrawLine(thisPosition, consumedCollisions[index], colors[1]);
                }
            }
            //if the object is within layer 3
            else if (distance < layer3)
            {
                //minor pointage goes here

                if (debug)
                {
                    Debug.DrawLine(thisPosition, consumedCollisions[index], colors[2]);
                }
            }
            //if the object is within the maximum bounds
            else if (distance < max)
            {
                //likely nothing happens here
                if (debug)
                {
                    Debug.DrawLine(thisPosition, consumedCollisions[index], colors[3]);
                }
            }
            else
            {
                Debug.DrawLine(thisPosition, consumedCollisions[index], Color.cyan);
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
        GameObject go = other.gameObject;//the object we've encountered
        thisPosition = this.transform.position;


        switch (other.tag)
        {
            case "Entrance":
            //we want entrance to fall through to obstacles because the behavior is the same.
            case "Walls":
            //we want walls to fall through to obstacles because the behavior is precisely the same.
            case "Obstacle":

                if (debug)
                {
                    Debug.Log("Hit obstacle " + go.name + " at " + go.transform.position + "; distance: " + Vector3.Distance(thisPosition, go.transform.position) +
                    " angle: " + calcCollisionAngle(other.ClosestPoint(thisPosition)));
                }

                //now we add the GO to the list
                if (!collidedObjects.Contains(go)) //we make sure we're not somehow adding a duplicate object to the list.
                {
                    collidedObjects.Add(go);
                    //we're adding it's distance here, and then referencing the object's index in it's own list to remove these distances later.
                    collidedMinDistances.Add(Vector3.Distance(thisPosition, go.transform.position));
                    //we add the closest point here
                    collisions.Add(other.ClosestPoint(thisPosition));
                    //and now the angle
                    collisionAngles.Add(calcCollisionAngle(other.ClosestPoint(thisPosition)));
                    //for testing, we're adding the object's name
                    //collidedNames.Add(go.name); No longer required
                }
                break;
            case "Consumable":
                if (debug)
                {
                    Debug.Log("Hit consumable " + go.name + " at " + go.transform.position + "; distance: " + Vector3.Distance(thisPosition, go.transform.position) +
                    " angle: " + calcCollisionAngle(other.ClosestPoint(thisPosition)));
                }
                //now we add the GO to the list
                if (!consumedObjects.Contains(go)) //we make sure we're not somehow adding a duplicate object to the list.
                {
                    consumedObjects.Add(go);
                    //we're adding it's distance here, and then referencing the object's index in it's own list to remove these distances later.
                    consumedDistances.Add(Vector3.Distance(thisPosition, go.transform.position));
                    //we add the closest point here
                    consumedCollisions.Add(other.ClosestPoint(thisPosition));
                    //and now the angle
                    consumedAngles.Add(calcCollisionAngle(other.ClosestPoint(thisPosition)));
                    //for testing, we're adding the object's name
                    //collidedNames.Add(go.name); No longer required
                }
                break;
            case "Player":
                break;
            case "Start Volume":
                break;
            case "End Volume":
                pmScript.setPlayerState(PlayerState.victory);
                break;
            default:
                break;
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
        switch (other.tag)
        {
            case "Entrance":
            //we want entrance to fall through to obstacles because the behavior is the same
            case "Walls":
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
                    collisions.RemoveAt(index);
                    collisionAngles.RemoveAt(index);
                    //collidedNames.RemoveAt(index);
                    //now we remove the object from the list
                    collidedObjects.Remove(go);
                }
                break;
            case "Obstacle":
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
                        pmScript.addPoints(3);
                    }
                    else if (collidedMinDistances[index] > layer2 && collidedMinDistances[index] < layer3)
                    {
                        //third most inner layer, some points maybe?
                        pmScript.addPoints(2);
                    }
                    else if (collidedMinDistances[index] > layer3 && collidedMinDistances[index] < max)
                    {
                        //if it is within the bounds of our outer layer
                        pmScript.addPoints();
                    }
                    else
                    {
                        //this statement is here just in case, but should never be encountered since the distance would have to be larger than the collider
                    }
                    //now we remove it's distance
                    collidedMinDistances.RemoveAt(index);
                    collisions.RemoveAt(index);
                    collisionAngles.RemoveAt(index);
                    //collidedNames.RemoveAt(index);
                    //now we remove the object from the list
                    collidedObjects.Remove(go);
                }
                break;
            case "Consumable":
                if (collidedObjects.Contains(go))//we want to make sure this object wasn't, by some miracle, removed from the list prematurely
                {
                    //first get the objects index
                    index = consumedObjects.IndexOf(go);
                    //now we remove it's distance
                    consumedDistances.RemoveAt(index);
                    consumedCollisions.RemoveAt(index);
                    consumedAngles.RemoveAt(index);
                    //collidedNames.RemoveAt(index);
                    //now we remove the object from the list
                    collidedObjects.Remove(go);
                }
                break;
            case "Player":
                break;
            case "Start Volume":
                break;
            case "End Volume":
                break;
            default:
                break;
        }
    }
    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void OnTriggerStay(COllider other)
    * Description:
    *                   keeps track and updates the collision points for the objects colided with.
    */
    public void OnTriggerStay(Collider other)
    {
        GameObject go = other.gameObject;
        thisPosition = this.transform.position;
        int index = 0;
        switch (other.tag)
        {
            case "Entrance":
                //we want entrance to fall through to obstacles as well because the behavior is the same.
            case "Walls":
            //we want walls to fall through to obstacles because the behavior is precisely the same.
            case "Obstacle":
                if (collidedObjects.Contains(go))
                {
                    //getting the index in the collisions array
                    index = collidedObjects.IndexOf(go);
                    //update the vector 3
                    collisions[index] = other.ClosestPoint(thisPosition);
                    collisionAngles[index] = calcCollisionAngle(other.ClosestPoint(thisPosition));
                    if (debug)
                    {
                        Debug.Log("Hit obstacle " + go.name + " at " + go.transform.position + "; distance: " +
                            Vector3.Distance(thisPosition, go.transform.position) +
                        " angle: " + calcCollisionAngle(other.ClosestPoint(thisPosition)));
                    }
                }
                break;
            case "Consumable":
                if (consumedObjects.Contains(go))
                {
                    //getting the index in the collisions array
                    index = consumedObjects.IndexOf(go);
                    //update the vector 3
                    consumedCollisions[index] = other.ClosestPoint(thisPosition);
                    consumedAngles[index] = calcCollisionAngle(other.ClosestPoint(thisPosition));
                    if (debug)
                    {
                        Debug.Log("Hit consumable" + go.name + " at " + go.transform.position + "; distance: " +
                            Vector3.Distance(thisPosition, go.transform.position) +
                        " angle: " + calcCollisionAngle(other.ClosestPoint(thisPosition)));
                    }
                }
                break;
            case "Player":
                break;
            case "Start Volume":
                break;
            case "End Volume":
                break;
            default:
                break;
        }
    }
    /**
    * Date:             May 15, 2017
    * Author:           Jay Coughlan
    * Interface:        void calcCollisionAngle(Vector3 onj)
    * Description:      This is a wrapper around the basic Vector3.Angle function with the purpose of keeping the math consistant
    *                   throughout the script. I want to make sure I'm using vectors of the correct directions in all instances.
    *                   
    */
    private float calcCollisionAngle(Vector3 obj)
    {
        float angle = 0;
        //the vectors we're using is from this.position towards the forward, and this.position towards the obj
        angle = Vector3.Angle(this.transform.forward - this.transform.position, obj - this.transform.position);
        //return this calculated angle
        return angle;
    }
    /**
    * Date:             May 15, 2017
    * Author:           Jay Coughlan
    * Interface:        void purgeCollissions()
    * Description:      This empties out all of our collisions so we don't act upon old data when we restart
    *                   
    */
    public void purgeCollisions()
    {
        //purge all obstacles
        collidedObjects.Clear();
        collidedMinDistances.Clear();
        collisions.Clear();
        collisionAngles.Clear();
        //purge all consumables
        consumedAngles.Clear();
        consumedCollisions.Clear();
        consumedDistances.Clear();
        consumedObjects.Clear();
    }
}
