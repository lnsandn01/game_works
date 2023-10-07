using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithCamera : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = Camera.main.transform.localScale;
    }
}
