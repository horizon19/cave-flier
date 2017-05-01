using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setup : MonoBehaviour {
    public GameObject[] thegameobjects;
    public Material materialtobeassigned;

    public void AssignMaterialToTheThreeObjects()
    {
        foreach (var obj in thegameobjects)
        {
            obj.GetComponent<Renderer>().sharedMaterial = materialtobeassigned;
        }
    }
}
