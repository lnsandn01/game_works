using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;


public class TriggerZone : MonoBehaviour
{
    [SerializeField] private bool self_destroy_after_enter;
    [SerializeField] private bool activate_on_enter;
    [SerializeField] private int speech_bubble_id;
    [SerializeField] private int text_box_id;
    [SerializeField] private int trap_id;
    [SerializeField] private int previous_trap_id; // set to -1 if previous trap triggered

    private void Start()
    {
        if(previous_trap_id != 0)
        {
            GWEventManager.activate_trap_event += onActivateTrap;
        }
    }

    private void OnDisable()
    {
        if(previous_trap_id != 0)
        {
            GWEventManager.activate_trap_event -= onActivateTrap;
        }
    }

    private void onActivateTrap(GameEvent game_event)
    {
        if((int)game_event.value == previous_trap_id)
        {
            previous_trap_id = -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if another trap needs to be triggered before this one
        if (previous_trap_id > 0)
        {
            return;
        }
        if (col.name == "Seni")
        {
            if(speech_bubble_id != 0)
            {
                // speech bubble trigger
                SpeechBubbleEventValue value = new SpeechBubbleEventValue();
                value.activate = activate_on_enter;
                value.speech_bubble_id = speech_bubble_id;
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWCon.SPEECH_BUBBLE_EVENT_TAG, default(DateTime), false, value));
            }
            if (text_box_id != 0)
            {
                // text box trigger
                TextBoxEventValue value = new TextBoxEventValue();
                value.activate = activate_on_enter;
                value.lang = 0;
                value.text_box_id = text_box_id;
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWCon.TEXT_BOX_EVENT_TAG, default(DateTime), false, value));
            }
            if(trap_id != 0)
            {
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWCon.ACTIVATE_TRAP_EVENT_TAG, default(DateTime), false, trap_id));
            }
            if (self_destroy_after_enter)
            {
                GameObject.Destroy(this);
            }
        }
    }
}
