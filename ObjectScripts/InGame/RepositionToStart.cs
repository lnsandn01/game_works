using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionToStart : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private float below_y;
    [SerializeField] private float left_of_x;
    [SerializeField] private bool freezeRotation;
    [SerializeField] private bool check_collision_in_child = true;
    [SerializeField] private float wait_time = 0.2f;
    Vector3 start_pos;
    Rigidbody2D rb;
    private bool fell;
    private bool set_constraints;
    private GameObject ob;
    private float time_waited = 0f;
    void Start()
    {
        start_pos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        rb = gameObject.GetComponent<Rigidbody2D>();
        ob = gameObject;
        if (check_collision_in_child)
        {
            ob = transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        if (GWStateManager.paused || GWStateManager.interrupted)
        {
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (rb.velocity.y < 0)
        {
            if (!set_constraints)
            {
                set_constraints = true;
                if (freezeRotation)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                transform.localEulerAngles = Vector3.zero;
                if (holder)
                {
                    holder.SetActive(false);
                }
            }
            fell = true;
        }
        if (fell && (GWControlsManager.touchesLayer(GWConst.GWConstManager.PLAYER_LAYER, ob)
            || GWControlsManager.touchesLayer(GWConst.GWConstManager.SLIPPERY_LAYER, ob)
            || (rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f)
            || (below_y != 0 && transform.position.y < below_y)
            || (left_of_x != 0 && transform.position.x < left_of_x)))
        {
            if (below_y != 0 && transform.position.y > below_y){
                if (left_of_x != 0)
                {
                    if(transform.position.x > left_of_x)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            if(left_of_x != 0 && transform.position.x > left_of_x && (below_y != 0 && transform.position.y > below_y))
            {
                return;
            }
            time_waited += Time.deltaTime;
            if(time_waited > wait_time)
            {
                transform.position = start_pos;
                rb.velocity = new Vector2(0, 0);
                transform.localEulerAngles = Vector3.zero;
                fell = false;
                time_waited = 0f;
                rb.constraints = RigidbodyConstraints2D.None;
                if (freezeRotation)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}
