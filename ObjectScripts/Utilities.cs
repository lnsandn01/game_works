using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static GameObject[] FindGameObjectsWithLayer(int layer)
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();
        List<GameObject> output = new System.Collections.Generic.List<GameObject>();
        foreach(GameObject obj in objs)
        {
            if(obj.layer == layer)
            {
                output.Add(obj);
            }
        }
        return output.ToArray();
    }
}