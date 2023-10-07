using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private bool sound_event;
    [SerializeField] private int sound_id;
    [SerializeField] private bool fade;
    [SerializeField] private float fade_time;
    [SerializeField] private bool sound_activate;
    [SerializeField] private float end_volume;

    private void Start()
    {
        if (sound_event)
        {
            GWEventManager.gwevent_manager.TriggerEvent(new GameEvent(
                GWConst.GWConstManager.SOUND_EVENT_TAG, default(System.DateTime), false,
                new SoundEventValue(sound_id, fade,fade_time,sound_activate,end_volume)));
        }
    }
}
