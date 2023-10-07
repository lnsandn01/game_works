using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class PlaySoundOnCollision : MonoBehaviour
{
    private AudioSource hit_sound;
    private AudioSource water_sound;
    private bool falling;
    private Rigidbody2D rigid_body;

    void Start()
    {
        hit_sound = GWAudioManager.getSoundOfObject(this.gameObject, "HitSound");
        water_sound = GWAudioManager.getSoundOfObject(this.gameObject, "WaterSound");
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rigid_body)
        {
            if(rigid_body.velocity.y > 0.01f || rigid_body.velocity.y < -0.01f)
            {
                falling = true;
            }
            else if(falling && rigid_body.velocity.y > -0.01f && rigid_body.velocity.y < 0.01f)
            {
                playSound();
                falling = false;
            }
        }
    }

    private void playSound()
    {
        if(GWControlsManager.touchesLayer(GWCon.WATER_LAYER, this.gameObject))
        {
            if (water_sound)
            {
                water_sound.Play();
            }
        }
        else
        {
            if (hit_sound)
            {
                hit_sound.Play();
            }
        }
    }
}