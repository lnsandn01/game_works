using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWStateManager : MonoBehaviour
{
#region scene
    public static bool level_started;
    public static bool scene_ended;
    public static bool managers_reinited;
    public static string current_menu;
    public static string current_scene;
#endregion

#region game
    public static bool grounded;
    public static bool jumping;
    public static bool swimming;
    public static bool respawning;
    public static bool cancel_jumping;
    public static bool uhitdangerous;
    public static bool dying;
    public static bool interrupted;
    public static bool text_box_active;
    public static int current_text_box_id;
    public static bool writing_text;
    public static bool selection_box;
    public static bool multi_lives_unlocked;
    public static int lives;
    public static int start_lives = 10;
    public static bool not_started_once;
    public static bool looking_left;
#endregion

    private void Awake()
    {
        GWEventManager.reinit_managers_event += onReinitManagers;
    }

    private void OnDisable()
    {
        GWEventManager.reinit_managers_event -= onReinitManagers;
    }

    private void onReinitManagers(GameEvent game_event)
    {
        grounded = false;
        jumping = false;
        swimming = false;
        respawning = false;
        cancel_jumping = false;
        uhitdangerous = false;
        dying = false;
        interrupted = false;
        text_box_active = false;
        managers_reinited = true;
        writing_text = false;
        selection_box = false;
        current_menu = "";
        looking_left = false;
    }
}
