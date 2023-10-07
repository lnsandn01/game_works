using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using GWCon = GWConst.GWConstManager;

public class GWControlsManager : MonoBehaviour
{
    public static PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Land.Jump.performed += onJump;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Land.Jump.performed -= onJump;
        playerControls.Disable();
        playerControls = null;
    }

    private void onJump(InputAction.CallbackContext cxt)
    {
        // serves as accept or jump button
        if (!GWStateManager.level_started)
        {
            return;
        }
        if (GWStateManager.text_box_active)
        {
            if (GWStateManager.writing_text || GWStateManager.selection_box) 
            {
                // text not completly written
                return;
            }
            TextBox.nextText();
        }
        else
        {

        }
    }

    public static IEnumerator playHaptics()
    {
        //TODO implement for fitting actions
        if(Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(1f, 1f);
            yield return new WaitForSeconds(.05f);
            InputSystem.ResetHaptics();
        }
    }

    public static bool touchesLayer(string layer_name, GameObject obj) {
        LayerMask layermask = 1 << LayerMask.NameToLayer(layer_name);
        if(layermask.value == -1)
        {
            Debug.LogError("layermask not defined, "+layer_name+" given");
            return false;
        }
        Array colliders = obj.GetComponents<Collider2D>();
        Rigidbody2D rigidb = obj.GetComponent<Rigidbody2D>();
        bool touches = false, boxcol = false, circlecol = false, capsulecol = false;
        float radius = 0f;
        Vector2 topLeftPoint, bottomLeftPoint, bottomRightPoint, circleCenterPoint, capsuleCenterPoint, capsuleSize;
        topLeftPoint = bottomLeftPoint = bottomRightPoint = circleCenterPoint = capsuleCenterPoint = capsuleSize = new Vector2(0, 0);
        CapsuleDirection2D capsuleDirection = default(CapsuleDirection2D);

        foreach (Collider2D collider in colliders)
        {
            if(collider is BoxCollider2D)
            {
                boxcol = true;
                topLeftPoint = new Vector2(collider.bounds.center.x - collider.bounds.extents.x,
                                                collider.bounds.center.y + collider.bounds.extents.y);
                bottomLeftPoint = new Vector2(collider.bounds.center.x - collider.bounds.extents.x,
                                                    collider.bounds.center.y - collider.bounds.extents.y);
                bottomRightPoint = new Vector2(collider.bounds.center.x + collider.bounds.extents.x,
                                                        collider.bounds.center.y - collider.bounds.extents.y);
            }
            if(collider is CircleCollider2D)
            {
                circlecol = true;
                circleCenterPoint = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
                radius = ((CircleCollider2D)collider).radius;
            }
            if(collider is CapsuleCollider2D)
            {
                capsulecol = true;
                capsuleCenterPoint = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
                capsuleSize = ((CapsuleCollider2D)collider).size;
                capsuleDirection = ((CapsuleCollider2D)collider).direction;
            }
        }

        switch (layer_name)
        {
            case GWCon.GROUND_LAYER:

                if (boxcol)
                {
                    touches = Physics2D.OverlapArea(bottomLeftPoint + new Vector2(0.05f, 0f), bottomRightPoint - new Vector2(0.05f, 0.1f), layermask);
                }
                if (capsulecol)
                {
                    touches = Physics2D.OverlapCapsule(capsuleCenterPoint, capsuleSize, capsuleDirection, 0, layermask);
                }
                
                if (rigidb)
                {
                    if (touches && rigidb.velocity.y <= 0.2f)
                    {
                        touches = true;
                    }
                    else
                    {
                        touches = false;
                    }
                }

                if (obj.name == "Player")
                {
                    GWStateManager.grounded = touches;
                }
                break;
            case GWCon.WATER_LAYER:
                if (boxcol)
                {
                    touches = Physics2D.OverlapArea(topLeftPoint, bottomRightPoint + new Vector2(0, 0.1f), layermask);
                }
                if (circlecol)
                {
                    touches = touches || Physics2D.OverlapCircle(circleCenterPoint, radius, layermask);
                }
                if (capsulecol)
                {
                    touches = touches || Physics2D.OverlapCapsule(capsuleCenterPoint, capsuleSize, capsuleDirection, 0, layermask);
                }
                if (obj.name == "Player")
                {
                    GWStateManager.swimming = touches;
                }
                break;
            default:
                foreach (Collider2D collider in colliders)
                {
                    touches = touches || collider.IsTouchingLayers(layermask);
                }
                break;
        }

        return touches;
    }
}
}
