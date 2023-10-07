using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class FreezeAfterLanding : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(GWControlsManager.touchesLayer(GWCon.GROUND_LAYER, this.gameObject))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
