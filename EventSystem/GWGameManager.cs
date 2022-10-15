using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using GWCon = GWConst.GWConstManager;

public class GWGameManager : MonoBehaviour
{
    private GameObject player;
    private GameObject startpoint;
    private GameObject endpoint;

    private void Awake()
    {
        GWEventManager.dying_event += onDyingEvent;
        GWEventManager.level_start_event += onLevelStartEvent;
        GWEventManager.reinit_managers_event += onReinitManagers;
        GWEventManager.start_new_game_event += onStartNewGame;
    }

    private void OnDisable()
    {
        GWEventManager.dying_event -= onDyingEvent;
        GWEventManager.level_start_event -= onLevelStartEvent;
        GWEventManager.reinit_managers_event -= onReinitManagers;
    }
    void Start()
    {
        /*EventValueScene evs = new EventValueScene();
        evs.scene_to_load = "testScene2";
        GameEvent ge = new GameEvent(5, 1, DateTime.UtcNow, false, evs);
        GWEventManager.gwevent_manager.TriggerEvent(ge);*/
    }

    private void FixedUpdate()
    {
        if(player != null)
        {
            if(player.transform.position.x >= endpoint.transform.position.x && !GWStateManager.respawning)
            {
                GWStateManager.respawning = true;
                // load next level
                int i = Array.IndexOf(GWCon.levels, GWStateManager.current_scene);
                EventValueScene scene = new EventValueScene();
                scene.scene_to_load = GWCon.levels[i + 1];
                GameEvent scene_event = new GameEvent(GWCon.SCENE_EVENT_TAG, DateTime.UtcNow, false, scene);
                GWEventManager.gwevent_manager.TriggerEvent(scene_event);
            }
        }
    }

    private void onReinitManagers(GameEvent game_event)
    {
        if (player == null)
        {
            player = GameObject.Find("Seni");
        }
        if (startpoint == null)
        {
            startpoint = GameObject.Find("Start");
        }
        if (endpoint == null)
        {
            endpoint = GameObject.Find("End");
        }
    }
    async private void onDyingEvent(GameEvent game_event)
    {
        DieEventValue value = (DieEventValue)game_event.value;
        if(value.dead_or_alive && value.mob.name == "Seni")
        {
            GWStateManager.dying = true;
            GWStateManager.interrupted = true;
            GWStateManager.lives--;
            if (GWStateManager.lives <= 0)
            {
                // lost game
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWCon.LOST_GAME_EVENT_TAG, default(DateTime),false,true));
                // wait until loading screen has finished the fade out
                if (!GWStateManager.scene_ended)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2.5d));
                }
                // load next menu
                int i = Array.IndexOf(GWConst.GWConstManager.menus, "LostGameMenu");
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWConst.GWConstManager.CHANGE_MENU_EVENT_TAG, default(DateTime), false, i));
                return;
            }
            // reload level
            EventValueScene scene = new EventValueScene();
            scene.scene_to_load = GWStateManager.current_scene;
            GameEvent scene_event = new GameEvent(GWCon.SCENE_EVENT_TAG, DateTime.UtcNow, false, scene);
            GWEventManager.gwevent_manager.TriggerEvent(scene_event);
        }
    }

    private void onLevelStartEvent(GameEvent game_event)
    {
        if ((bool)game_event.value)
        {
            if (player == null)
            {
                player = GameObject.Find("Seni");
            }
            if (startpoint == null)
            {
                startpoint = GameObject.Find("Start");
            }
            if (endpoint == null)
            {
                endpoint = GameObject.Find("End");
            }
            player.transform.position = startpoint.transform.position;
            if (!GWStateManager.not_started_once)
            {
                GWStateManager.not_started_once = true;
                // TODO move to button of main menu
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWConst.GWConstManager.START_NEW_GAME_EVENT_TAG, default(System.DateTime), false, true));
            }
        }
        else
        {

        }
    }

    private void onStartNewGame(GameEvent game_event)
    {
        GWStateManager.lives = GWStateManager.start_lives;
        // load InGamUI
        int i = Array.IndexOf(GWConst.GWConstManager.menus, "InGameUI");
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWConst.GWConstManager.CHANGE_MENU_EVENT_TAG, default(DateTime), false, i));
    }
}
