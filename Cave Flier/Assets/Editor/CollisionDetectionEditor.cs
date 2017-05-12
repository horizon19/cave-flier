﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/*
[CustomEditor(typeof(collisionDetection))]
public class CollisionDetectionEditor : Editor
{
    private Renderer maxRend, lay3Rend, lay2Rend, minRend;
    private bool myDebug = false;

    public override void OnInspectorGUI()
    {
        collisionDetection myTarget = (collisionDetection)target;

        myTarget.debug = EditorGUILayout.Toggle("Debug", myTarget.debug);

        myTarget.min = EditorGUILayout.FloatField("Innermost Layer", myTarget.min);
        myTarget.layer2 = EditorGUILayout.FloatField("Second Layer", myTarget.layer2);
        myTarget.layer3 = EditorGUILayout.FloatField("Third Layer", myTarget.layer3);
        myTarget.max = EditorGUILayout.FloatField("Outermost Layer", myTarget.max);

        myTarget.minimumSphere.transform.localScale = new Vector3(myTarget.min, myTarget.min, myTarget.min);
        myTarget.layer2Sphere.transform.localScale = new Vector3(myTarget.layer2, myTarget.layer2, myTarget.layer2);
        myTarget.layer3Sphere.transform.localScale = new Vector3(myTarget.layer3, myTarget.layer3, myTarget.layer3);
        myTarget.maximumSphere.transform.localScale = new Vector3(myTarget.max, myTarget.max, myTarget.max);

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
        
    }


}*/
