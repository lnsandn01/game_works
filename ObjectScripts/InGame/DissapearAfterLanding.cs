using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class DissapearAfterLanding : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool falling;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb.velocity.y < 0f)
        {
            falling = true;
        }
        if(falling && rb.velocity.y >= -0.001f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
