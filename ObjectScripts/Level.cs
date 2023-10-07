using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWCon = GWConst.GWConstManager;

public class Level : MonoBehaviour
{
    private void Start()
    {
        GWStateManager.current_menu = "";
        StartCoroutine(sendStartEvent());
    }

    private IEnumerator sendStartEvent()
    {
        while (!GWStateManager.managers_reinited)
        {
            yield return new WaitForSeconds(0.01f);
        }
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWCon.LEVEL_START_EVENT_TAG, default(System.DateTime), false, true));
        GWStateManager.level_started = true;
        yield return null;
    }
}
