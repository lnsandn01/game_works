using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class FallOnActive : MonoBehaviour
{
    [SerializeField] private int trap_id;
    private Rigidbody2D rb;
    private AudioSource hit_sound;
    private AudioSource fall_sound;
    private bool colliding = true;

    private void Awake()
    {
        GWEventManager.activate_trap_event += onActivateTrap;
    }

    private void OnDisable()
    {
        GWEventManager.activate_trap_event -= onActivateTrap;
    }

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        hit_sound = GWAudioManager.getSoundOfObject(this.gameObject, "HitSound");
        fall_sound = GWAudioManager.getSoundOfObject(this.gameObject, "FallSound");
    }

    private void onActivateTrap(GameEvent game_event)
    {
        if((int)game_event.value == trap_id)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            if (fall_sound)
            {
                fall_sound.Play();
            }
        }
    }

    private void Update()
    {
        if (GWStateManager.interrupted)
        {
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            if(!GWControlsManager.touchesLayer(GWCon.GROUND_LAYER, this.gameObject))
            {
                colliding = false;
            }else if (!colliding)
            {
                if (hit_sound)
                {
                    hit_sound.Play();
                }
                colliding = true;
            }
        }
    }
}
