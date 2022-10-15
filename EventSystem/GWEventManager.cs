using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class GWEventManager : MonoBehaviour
{
    public static GWEventManager gwevent_manager;
    public EventSafe event_safe;

    public static event Action<GameEvent> loop_event;
    public static event Action<GameEvent> controler_event;
    public static event Action<GameEvent> player_event;
    public static event Action<GameEvent> sound_event;
    public static event Action<GameEvent> scene_event;
#region menu_events
    public static event Action<GameEvent> open_main_menu_event;
    public static event Action<GameEvent> in_main_menu_event;
    public static event Action<GameEvent> change_menu_event;
    public static event Action<GameEvent> level_start_event;
    public static event Action<GameEvent> reinit_managers_event;
    public static event Action<GameEvent> blend_out_event;
#endregion
#region in_game_events
    public static event Action<GameEvent> grounded_event;
    //public static event Action<GameEvent> jump_event;
    public static event Action<GameEvent> dying_event;
    public static event Action<GameEvent> speechbubble_event;
    public static event Action<GameEvent> textbox_event;
    public static event Action<GameEvent> activate_trap_event;
    public static event Action<GameEvent> lost_game_event;
    public static event Action<GameEvent> start_new_game_event;
    #endregion

    private void Awake()
    {
        gwevent_manager = this;
    }

    public void TriggerEvent(GameEvent new_game_event)
    {
        switch (new_game_event.tag)
        {
            case GWCon.LOOP_EVENT_TAG:
                if(loop_event != null)
                {
                    loop_event(new_game_event);
                }
                break;
            case GWCon.CONTROLER_EVENT_TAG:
                if (controler_event != null)
                {
                    controler_event(new_game_event);
                }
                break;
            case GWCon.PLAYER_EVENT_TAG:
                if (player_event != null)
                {
                    player_event(new_game_event);
                }
                break;
            case GWCon.SOUND_EVENT_TAG:
                if (sound_event != null)
                {
                    sound_event(new_game_event);
                }
                break;
            case GWCon.SCENE_EVENT_TAG:
                if(scene_event != null)
                {
                    scene_event(new_game_event);
                }
                break;
            case GWCon.OPEN_MAIN_MENU_EVENT_TAG:
                if(open_main_menu_event != null)
                {
                    open_main_menu_event(new_game_event);
                }
                break;
            case GWCon.GROUNDED_EVENT_TAG:
                if(grounded_event != null)
                {
                    grounded_event(new_game_event);
                }
                break;
            /*case GWCon.JUMP_EVENT_TAG:
                if(jump_event != null)
                {
                    jump_event(new_game_event);
                }
                break;*/
            case GWCon.DYING_EVENT_TAG:
                if(dying_event != null)
                {
                    dying_event(new_game_event);
                }
                break;
            case GWCon.LEVEL_START_EVENT_TAG:
                if(level_start_event != null)
                {
                    level_start_event(new_game_event);
                }
                break;
            case GWCon.REINIT_MANAGERS_GAME_EVENT_TAG:
                if (reinit_managers_event != null)
                {
                    reinit_managers_event(new_game_event);
                }
                break;
            case GWCon.CHANGE_MENU_EVENT_TAG:
                if (change_menu_event != null)
                {
                    change_menu_event(new_game_event);
                }
                break;
            case GWCon.SPEECH_BUBBLE_EVENT_TAG:
                if (speechbubble_event != null)
                {
                    speechbubble_event(new_game_event);
                }
                break;
            case GWCon.TEXT_BOX_EVENT_TAG:
                if (textbox_event != null)
                {
                    textbox_event(new_game_event);
                }
                break;
            case GWCon.ACTIVATE_TRAP_EVENT_TAG:
                if (activate_trap_event != null)
                {
                    activate_trap_event(new_game_event);
                }
                break;
            case GWCon.LOST_GAME_EVENT_TAG:
                if(lost_game_event != null)
                {
                    lost_game_event(new_game_event);
                }
                break;
            case GWCon.START_NEW_GAME_EVENT_TAG:
                if(start_new_game_event != null)
                {
                    start_new_game_event(new_game_event);
                }
                break;
            case GWCon.BLEND_OUT_SCENE_EVENT_TAG:
                if (blend_out_event != null)
                {
                    blend_out_event(new_game_event);
                }
                break;
            default:
                Debug.Log("event couldn't be triggered! tag: " + new_game_event.tag + " unknown! edit gweventmanager main switch");
                break;
        }
    }
}
