using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpecificObject : MonoBehaviour
{
    [SerializeField] private List<ushort> build_in;
    private bool other_activated;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if(!build_in.Contains(0) && !other_activated)
        {
            this.enabled = false;
            GameObject.Destroy(gameObject);
        }
        else
        {
            other_activated = true;
        }
#endif
#if UNITY_STANDALONE_WIN
        if (!build_in.Contains(1) && !other_activated)
        {
            this.enabled = false;
            GameObject.Destroy(gameObject);
        }
        else
        {
            other_activated = true;
        }
#endif
#if UNITY_IPHONE
        if (!build_in.Contains(2) && !other_activated)
        {
            this.enabled = false;
            GameObject.Destroy(gameObject);
        }
        else
        {
            other_activated = true;
        }
#endif
#if UNITY_ANDROID
        if (!build_in.Contains(3) && !other_activated)
        {
            this.enabled = false;
            GameObject.Destroy(gameObject);
        }
        else
        {
            other_activated = true;
        }
#endif
    }
}
