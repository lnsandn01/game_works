using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    private void FixedUpdate()
    {
        if (GWStateManager.interrupted)
        {
            return;
        }
        if (player != null)
        {
            if(player.transform.position.x >= endpoint.transform.position.x && !GWStateManager.respawning)
            {
                GWStateManager.respawning = true;

                // update gained xp
                if (Regex.Matches(GWStateManager.current_scene, "lvl[0-9]+(?<!_before)$").Count != 0)
                {
                    int lvl = Int32.Parse(Regex.Replace(GWStateManager.current_scene, "lvl", ""));
                    uint xp = (uint)xpPerLevel(lvl);
                    GWStateManager.xp += xp;
                    if(livesPerXp(GWStateManager.xp) > GWStateManager.start_lives)
                    {
                        GWStateManager.lives++;
                    }
                    GWStateManager.start_lives = livesPerXp(GWStateManager.xp);
                    GWEventManager.gwevent_manager.TriggerEvent(new GameEvent(GWCon.XP_EVENT_TAG, DateTime.UtcNow, true, xp));
                }
                loadNextLvl();
            }
        }
    }

    private void loadNextLvl()
    {
        // load next level
        int i = Array.IndexOf(GWCon.levels, GWStateManager.current_scene);
        EventValueScene scene = new EventValueScene();
        scene.scene_to_load = GWCon.levels[i + 1];
        GameEvent scene_event = new GameEvent(GWCon.SCENE_EVENT_TAG, DateTime.UtcNow, false, scene);
        GWEventManager.gwevent_manager.TriggerEvent(scene_event);
    }

    private void onReinitManagers(GameEvent game_event)
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
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
        if(value.dead_or_alive && value.mob_name == "Player" && !GWStateManager.dying)
        {
            GWStateManager.dying = true;
            GWStateManager.interrupted = true;
            GWStateManager.lives--;

            // add lvl 5 to loaded story lvls
            addLoadedStoryLvl();

            if (GWStateManager.lives <= 0)
            {
                // lost game
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWCon.LOST_GAME_EVENT_TAG, default(DateTime),false,true));
                return;
            }
            // reload level
            EventValueScene scene = new EventValueScene();
            scene.scene_to_load = GWStateManager.current_scene;
            GameEvent scene_event = new GameEvent(GWCon.SCENE_EVENT_TAG, default(DateTime), false, scene);
            GWEventManager.gwevent_manager.TriggerEvent(scene_event);
        }
    }

    private void onLevelStartEvent(GameEvent game_event)
    {
        if ((bool)game_event.value)
        {
            if (player == null)
            {
                player = GameObject.Find("Player");
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
            GWStateManager.dying = false;

            if (GWStateManager.not_started_once)
            {
                GWStateManager.not_started_once = false;
                // TODO move to button of main menu
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWConst.GWConstManager.START_NEW_GAME_EVENT_TAG, default(System.DateTime), false, true));
            }
        }
    }

    private void onStartNewGame(GameEvent game_event)
    {
        GWStateManager.lives = GWStateManager.start_lives;

        StartCoroutine(waitForSceneLoadedToLoadUI());
    }

    private IEnumerator waitForSceneLoadedToLoadUI()
    {
        // used to load ingameui
        yield return null;
    }

    public static uint xpPerLevel(int lvl)
    {
        return (uint)Mathf.Pow(lvl, 3);
    }

    public static int livesPerXp(uint xp)
    {
        return (int)Mathf.Pow(xp, 1f / 3f);
    }

    // percentage of xp, compared to the required xp for the next live
    public static float percentage_of_xp(uint xp)
    {
        uint exp_until_next_live = xpPerLevel(GWStateManager.start_lives + 1);
        return ((float)GWStateManager.xp)/ exp_until_next_live;
    }
}
