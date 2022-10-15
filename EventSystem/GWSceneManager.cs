using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using GWCon = GWConst.GWConstManager;

public class GWSceneManager : MonoBehaviour
{ 
    void Awake()
    {
        GWEventManager.reinit_managers_event += onReinitManagers;
        GWEventManager.scene_event += onScene;
        GWEventManager.change_menu_event += onChangeMenu;
    }

    private void OnDisable()
    {
        GWEventManager.reinit_managers_event -= onReinitManagers;
        GWEventManager.scene_event -= onScene;
        GWEventManager.change_menu_event -= onChangeMenu;
        GWControlsManager.playerControls.Land.Pause.performed -= onPause;
    }

    private void onReinitManagers(GameEvent game_event)
    {

    }

    private void Start()
    {
        GWControlsManager.playerControls.Land.Pause.performed += onPause;
        GWStateManager.managers_reinited = true;
        GWStateManager.current_scene = SceneManager.GetActiveScene().name;
    }

    IEnumerator LoadScene(string scene_to_load)
    {
        // blend out screen
        BlendOutEventValue value = new BlendOutEventValue(true, false);
        if (GWStateManager.dying)
        {
            value = new BlendOutEventValue(true, true);
        }
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWCon.BLEND_OUT_SCENE_EVENT_TAG, default(DateTime), false, value));
        while (!GWStateManager.scene_ended)
        {
            yield return new WaitForSeconds(0.1f);
        }
        GWStateManager.managers_reinited = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_to_load);
        float progress = .0f;
        float old_progress = .0f;
        while (!operation.isDone)
        {
            progress = progress = Mathf.Round(Mathf.Clamp01(operation.progress / .9f) * 100f) / 100f;
            if (progress != old_progress)
            {
                old_progress = progress;
                // TODO fire event for loading screen update
                yield return new WaitForSeconds(0.3f);
            }
        }
        GWStateManager.scene_ended = false;
        GWStateManager.current_scene = scene_to_load;
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWCon.REINIT_MANAGERS_GAME_EVENT_TAG, default(DateTime), false, true));
        value.blend_out = false;
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWCon.BLEND_OUT_SCENE_EVENT_TAG, default(DateTime), false, value));
    }

    private void onScene(GameEvent game_event)
    {
        // load new scene
        EventValueScene value = (EventValueScene)game_event.value;
        StartCoroutine(LoadScene(value.scene_to_load));
    }

    public void onChangeMenu(GameEvent game_event)
    {
        // load menu
        var canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        if (canvases.Length > 0)
        {
            foreach(Canvas can in canvases)
            {
                if(can.name == GWCon.menus[(int)game_event.value] 
                    || can.name == "LoadingScreen" || can.name == "SpeechBoxes")
                {
                    if (can.gameObject.activeSelf && can.name != "LoadingScreen" && can.name != "SpeechBoxes")
                    {
                        can.gameObject.SetActive(false);
                        GWStateManager.current_menu = "";
                    }
                    else
                    {
                        can.gameObject.SetActive(true);
                        if(can.name != "LoadingScreen" && can.name != "SpeechBoxes")
                        {
                            GWStateManager.current_menu = can.name;
                        }
                    }
                }
                else
                {
                    can.gameObject.SetActive(false);
                }
            }
        }
    }

    private void onPause(InputAction.CallbackContext cxt)
    {
        if(GWStateManager.current_scene == "StartScene")
        {
            return;
        }
        if(GWStateManager.current_menu == "PauseMenu")
        {
            GWStateManager.interrupted = false;
        }else
        {
            GWStateManager.interrupted = true;
        }
        int i = Array.IndexOf(GWConst.GWConstManager.menus, "PauseMenu");
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWConst.GWConstManager.CHANGE_MENU_EVENT_TAG, default(DateTime), false, i));
    }
}
