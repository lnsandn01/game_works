using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] private float waitForSeconds;
    [SerializeField] private int event_tag;
    private bool animation_finished = false;

    private void Awake()
    {
        GWEventManager.blend_out_event += onBlendOut;         
        GWEventManager.lost_game_event += onLostGame;         
    }

    private void OnDisable()
    {
        GWEventManager.blend_out_event -= onBlendOut;
        GWEventManager.lost_game_event -= onLostGame;
    }

    private void Start()
    {
        StartCoroutine(JustBlendIn(0f, null));
    }

    private void Update()
    {
        //gameObject.transform.localScale = Camera.main.transform.localScale;
    }

    public IEnumerator JustBlendIn(float waitFor, System.Action action)
    {
        yield return new WaitForSeconds(waitFor);
        animator.SetTrigger("Start");
        float time_given = 0;
        // set by an animation event
        animation_finished = false;
        while (!animation_finished)
        {
            time_given += Time.deltaTime;
            if(time_given > 2)
            {
                animation_finished = true;
            }
            yield return null;
        }
        animation_finished = false;

        if (action != null)
        {
            action();
        }
    }

    public IEnumerator JustBlendOut(float waitFor, System.Action action)
    {
        GWStateManager.blent_out = false;
        yield return new WaitForSeconds(waitFor);
        animator.SetTrigger("End");
        // set by an animation event
        float time_given = 0f;
        animation_finished = false;
        while (!animation_finished)
        {
            time_given += Time.deltaTime;
            if (time_given > 2f)
            {
                animation_finished = true;
            }
            yield return null;
        }
        GWStateManager.blent_out = true;
        animation_finished = false;

        if (action != null)
        {
            action();
        }
    }

    public void setAnimationFinished()
    {
        animation_finished = true;
    }

    // scene change
    IEnumerator EndScreen(bool with_cam_zoom)
    {
        if (with_cam_zoom)
        {
            int i = 0;
            while (i < 15)
            {
                if (GWStateManager.paused)
                {
                    yield return null;
                    continue;
                }
                yield return new WaitForSeconds(0.1f);
                i++;
            }
        }

        if (GWStateManager.lives <= 0 && with_cam_zoom)
        {
            StartCoroutine(JustBlendOut(0f, loadLostGameMenu));
        }
        else
        {
            StartCoroutine(JustBlendOut(0f, sendLevelStartEvent));
        }

        yield return null;
    }

    private void loadLostGameMenu()
    {
        // load next menu
        int i = Array.IndexOf(GWConst.GWConstManager.menus, "LostGameMenu");
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWConst.GWConstManager.CHANGE_MENU_EVENT_TAG, default(DateTime), false, i));
    }

    private void sendLevelStartEvent()
    {
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWCon.LEVEL_START_EVENT_TAG, default(System.DateTime), false, false));

        GWStateManager.scene_ended = true;
    }

    IEnumerator StartScreen()
    {
        while (!GWStateManager.managers_reinited)
        {
            yield return null;
        }
        StartCoroutine(JustBlendIn(0f, setRespawning));
        
        yield return null;
    }

    // on receiving blendout event only used while dying
    private void onBlendOut(GameEvent game_event)
    {
        BlendOutEventValue value = (BlendOutEventValue)game_event.value;
        if (value.blend_out)
        {
            StartCoroutine(EndScreen(value.with_cam_zoom));
        }
        else
        {
            StartCoroutine(StartScreen());
        }
    }

    private void setRespawning()
    {
        GWStateManager.respawning = false;
    }

    private void onLostGame(GameEvent game_event)
    {
        StartCoroutine(EndScreen(true));
    }
}
