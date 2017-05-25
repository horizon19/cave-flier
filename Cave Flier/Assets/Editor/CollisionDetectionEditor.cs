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

    //these are the properties we want to serialize and save when we run the code
    //these are the floats
    SerializedProperty min, lay2, lay3, max;
    //these are bools
    SerializedProperty debug;

    /**
    * Date:             May 20, 2017
    * Author:           Jay Coughlan
    * Description:
    *                   This function runs the moment the editor exists. 
    *                   It pulls the values from the script into the editor so they can be saved.
    */
    protected virtual void OnEnable()
    {
        min = this.serializedObject.FindProperty("min");
        lay2 = this.serializedObject.FindProperty("layer2");
        lay3 = this.serializedObject.FindProperty("layer3");
        max = this.serializedObject.FindProperty("max");
        debug = this.serializedObject.FindProperty("debug");
    }

    /**
    * Date:             May 16, 2017
    * Author:           Jay Coughlan
    * Description:
    *                   This is the main function, overriding Unity's default behavior and creating my own.
    */
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        //this is the script we want to modify, we get the version of it we're looking at
        collisionDetection myTarget = (collisionDetection)target;


        //now if debug is off, we don't render the spheres at all.
        EditorGUI.BeginChangeCheck();
        //first we reveal the debug toggle for easy debugability
        debug.boolValue = EditorGUILayout.Toggle("Debug", myTarget.debug);
        if (EditorGUI.EndChangeCheck())
        {
            maxRend = myTarget.maximumSphere.GetComponent<Renderer>();
            lay3Rend = myTarget.layer3Sphere.GetComponent<Renderer>();
            lay2Rend = myTarget.layer2Sphere.GetComponent<Renderer>();
            minRend = myTarget.minimumSphere.GetComponent<Renderer>();

            if (debug.boolValue)
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
        }

        //these lines make a field to input the floats for the various distances, then clamps them so they can't be
        //larger or smaller than each other
        EditorGUI.BeginChangeCheck();//we start listening for a change
        min.floatValue = EditorGUILayout.FloatField("Innermost Layer",
            Mathf.Clamp(min.floatValue, 0, myTarget.layer2 - 0.001f));//we apply the new value if it has changed
        if(EditorGUI.EndChangeCheck())
        {
            //if there is a change we change the size of the object.
            myTarget.minimumSphere.transform.localScale = new Vector3(myTarget.min * 2, myTarget.min * 2, myTarget.min * 2);
        }
        //rinse, repeat for layer2
        EditorGUI.BeginChangeCheck();
        lay2.floatValue = EditorGUILayout.FloatField("Second Layer", 
            Mathf.Clamp(myTarget.layer2, myTarget.min + 0.001f, myTarget.layer3 - 0.001f));
        if(EditorGUI.EndChangeCheck())
        {
            myTarget.layer2Sphere.transform.localScale = new Vector3(myTarget.layer2 * 2, myTarget.layer2 * 2, myTarget.layer2 * 2);
        }
        //rinse repeat for layer3
        EditorGUI.BeginChangeCheck();
        lay3.floatValue = EditorGUILayout.FloatField("Third Layer", 
            Mathf.Clamp(myTarget.layer3, myTarget.layer2 + 0.001f, myTarget.max - 0.001f));
        if(EditorGUI.EndChangeCheck())
        {
            myTarget.layer3Sphere.transform.localScale = new Vector3(myTarget.layer3 * 2, myTarget.layer3 * 2, myTarget.layer3 * 2);
        }

        //rinse repeat for the max layer
        EditorGUI.BeginChangeCheck();
        max.floatValue = EditorGUILayout.FloatField("Outermost Layer", 
            Mathf.Clamp(myTarget.max, myTarget.layer3 + 0.001f, 10000f));
        if(EditorGUI.EndChangeCheck())
        {
            myTarget.maximumSphere.transform.localScale = new Vector3(myTarget.max * 2, myTarget.max * 2, myTarget.max * 2);
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
        //EditorUtility.SetDirty(this);
        this.serializedObject.ApplyModifiedProperties();
    }


}
