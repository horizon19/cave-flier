/*-----------------------------------------------------------------------
--  SOURCE FILE:    CollisionDetectionEditor.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  Void OnInspectorGUI()
--                
--  DATE:           May 16th, 2017
--
--  DESIGNER:       Jay Coughlan
--
--  NOTES:
--		            This script overrides the default inspector behavior when looking at 
--                  collisionDetection components on objects. It is designed to make the 
--                  elements in the script make a little more sense and easier to read.
----------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//this defines what kind of component/script we want to override
[CustomEditor(typeof(collisionDetection))]
//this uses the editor classes in unity
public class CollisionDetectionEditor : Editor
{
    //these are the renderer components for the spheres.
    private Renderer maxRend, lay3Rend, lay2Rend, minRend;
    //this is the debug bool to display
    private bool myDebug = false;


    /**
    * Date:             May 16, 2017
    * Author:           Jay Coughlan
    * Description:
    *                   This is the main function, overriding Unity's default behavior and creating my own.
    */
    public override void OnInspectorGUI()
    {
        //this is the script we want to modify, we get the version of it we're looking at
        collisionDetection myTarget = (collisionDetection)target;

        //first we reveal the debug toggle for easy debugability
        myTarget.debug = EditorGUILayout.Toggle("Debug", myTarget.debug);

        //these lines make a field to input the floats for the various distances, then clamps them so they can't be
        //larger or smaller than each other
        myTarget.min = EditorGUILayout.FloatField("Innermost Layer", 
            Mathf.Clamp(myTarget.min, 0, myTarget.layer2 - 0.001f));
        myTarget.layer2 = EditorGUILayout.FloatField("Second Layer", 
            Mathf.Clamp(myTarget.layer2, myTarget.min + 0.001f, myTarget.layer3 - 0.001f));
        myTarget.layer3 = EditorGUILayout.FloatField("Third Layer", 
            Mathf.Clamp(myTarget.layer3, myTarget.layer2 + 0.001f, myTarget.max - 0.001f));
        myTarget.max = EditorGUILayout.FloatField("Outermost Layer", 
            Mathf.Clamp(myTarget.max, myTarget.layer3 + 0.001f, 10000f));
        myTarget.headOnRange = EditorGUILayout.FloatField("Head On Collision Angle", myTarget.headOnRange);

        //now we check to make sure the sphere has actually changed. If it has, we change sizes. if not, we don't
        if(myTarget.minimumSphere.transform.localScale.x != myTarget.min * 2)
        {
            myTarget.minimumSphere.transform.localScale = new Vector3(myTarget.min * 2, myTarget.min * 2, myTarget.min * 2);
        }
        if (myTarget.minimumSphere.transform.localScale.x != myTarget.layer2 * 2)
        {
            myTarget.layer2Sphere.transform.localScale = new Vector3(myTarget.layer2 * 2, myTarget.layer2 * 2, myTarget.layer2 * 2);
        }
        if (myTarget.layer3Sphere.transform.localScale.x != myTarget.layer3 * 2)
        {
            myTarget.layer3Sphere.transform.localScale = new Vector3(myTarget.layer3 * 2, myTarget.layer3 * 2, myTarget.layer3 * 2);
        }
        if (myTarget.maximumSphere.transform.localScale.x != myTarget.max * 2)
        {
            myTarget.maximumSphere.transform.localScale = new Vector3(myTarget.max * 2, myTarget.max * 2, myTarget.max * 2);
        }

        //now if debug is off, we don't render the spheres at all.
        if(myTarget.debug != myDebug)
        {
            maxRend = myTarget.maximumSphere.GetComponent<Renderer>();
            lay3Rend = myTarget.layer3Sphere.GetComponent<Renderer>();
            lay2Rend = myTarget.layer2Sphere.GetComponent<Renderer>();
            minRend = myTarget.minimumSphere.GetComponent<Renderer>();

            if (myTarget.debug)
            {
                maxRend.enabled = true;
                lay3Rend.enabled = true;
                lay2Rend.enabled = true;
                minRend.enabled = true;
            }
            else
            {
                maxRend.enabled = false;
                lay3Rend.enabled = false;
                lay2Rend.enabled = false;
                minRend.enabled = false;
            }

            myDebug = myTarget.debug;
        }

        //this is to reveal the array of obstacles. The label is the title.
        EditorGUILayout.LabelField("Obstacles");
        //this begins the vertical space to show the obstacles
        EditorGUILayout.BeginVertical();
        for(int iii = 0; iii < myTarget.collidedObjects.Count; iii ++)
        {
            //for each obstacle we place the name, the closest collision, the angle, and the minimum distance.
            //thse don't show up until the array populates.
            myTarget.collisions[iii] = EditorGUILayout.Vector3Field(myTarget.collidedObjects[iii].gameObject.name, myTarget.collisions[iii]);
            myTarget.collisionAngles[iii] = EditorGUILayout.FloatField("Angle", myTarget.collisionAngles[iii]);
            myTarget.collidedMinDistances[iii] = EditorGUILayout.FloatField("Min Distance", myTarget.collidedMinDistances[iii]);
            
        }
        EditorGUILayout.EndVertical();//end the vertical section to hold those

        //rinse and repeat obstacles for consumables
        EditorGUILayout.LabelField("Consumables");
        EditorGUILayout.BeginVertical();
        for (int iii = 0; iii < myTarget.consumedObjects.Count; iii++)
        {

            myTarget.consumedCollisions[iii] = EditorGUILayout.Vector3Field(myTarget.consumedObjects[iii].gameObject.name, 
                myTarget.consumedCollisions[iii]);
            myTarget.consumedAngles[iii] = EditorGUILayout.FloatField("Angle", myTarget.consumedAngles[iii]);
            myTarget.consumedDistances[iii] = EditorGUILayout.FloatField("Distance", myTarget.consumedDistances[iii]);
        }
        EditorGUILayout.EndVertical();//end that vertical as well
    }


}
