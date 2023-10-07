using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowOnActive : MonoBehaviour
{
    [SerializeField] private int trap_id;
    [SerializeField] private int optional_trap_id;
    [SerializeField] private float scale_x;
    [SerializeField] private float scale_y;
    [SerializeField] private float scale_z;
    [SerializeField] private float moving_factor;
    [SerializeField] private int time = 120;
    private bool started_growing;
    private AudioSource grow_sound;

    void Start()
    {
        grow_sound = GWAudioManager.getSoundOfObject(this.gameObject, "GrowingSound");
        GWEventManager.activate_trap_event += onActivateTrap;
        if (scale_x == 0)
        {
            scale_x = transform.localScale.x;
        }
        if (scale_y == 0)
        {
            scale_y = transform.localScale.y;
        }
        if (scale_z == 0)
        {
            scale_z = transform.localScale.z;
        }
    }

    private void OnDisable()
    {
        GWEventManager.activate_trap_event -= onActivateTrap;
    }

    private void onActivateTrap(GameEvent game_event)
    {
        if ((int)game_event.value != trap_id && (int)game_event.value != optional_trap_id)
        {
            return;
        }
        if (!started_growing)
        {
            StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow()
    {
        started_growing = true;
        int interpolationFramesCount = time; // Number of frames to completely interpolate between the 2 positions
        int elapsedFrames = 0;
        float interpolationRatio = 0;
        Vector3 oldScale;
        if (grow_sound)
        {
            grow_sound.Play();
        }
        while ((transform.localScale.x < scale_x - 0.001f || transform.localScale.x > scale_x + 0.001f)
            || (transform.localScale.y < scale_y - 0.001f || transform.localScale.y > scale_y + 0.001f)
            || (transform.localScale.z < scale_z - 0.001f || transform.localScale.z > scale_z + 0.001f))
        {
            if (GWStateManager.interrupted)
            {
                if (grow_sound)
                {
                    grow_sound.Pause();
                }
                yield return null;
                continue;
            }
            else
            {
                if (grow_sound)
                {
                    grow_sound.UnPause();
                }
            }
            interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
            // scaling
            oldScale = transform.localScale;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale_x, scale_y, scale_z), interpolationRatio);
            // repositioning
            if(transform.localScale.x < scale_x - 0.001f)
            {
                transform.position = new Vector3(transform.position.x + Mathf.Abs(transform.localScale.x - oldScale.x) * moving_factor, transform.position.y, transform.position.z);
            }
            if (transform.localScale.x > scale_x + 0.001f)
            {
                transform.position = new Vector3(transform.position.x - Mathf.Abs(transform.localScale.x - oldScale.x) * moving_factor, transform.position.y, transform.position.z);
            }
            if (transform.localScale.y < scale_y - 0.001f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Abs(transform.localScale.y - oldScale.y) * moving_factor, transform.position.z);
            }
            if (transform.localScale.y > scale_y + 0.001f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - Mathf.Abs(transform.localScale.y - oldScale.y) * moving_factor, transform.position.z);
            }
            if (transform.localScale.z < scale_z - 0.001f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Mathf.Abs(transform.localScale.z - oldScale.z) * moving_factor);
            }
            if (transform.localScale.z > scale_z + 0.001f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Mathf.Abs(transform.localScale.z - oldScale.z) * moving_factor);
            }
            elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)
            yield return null;
        }
        if (grow_sound)
        {
            grow_sound.Stop();
        }
        yield return null;
    }
}
