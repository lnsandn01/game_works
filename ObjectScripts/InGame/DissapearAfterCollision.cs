using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class DisappearAfterCollision : MonoBehaviour
{
    private void Update()
    {
        if (GWControlsManager.touchesLayer(GWCon.GROUND_LAYER,this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
