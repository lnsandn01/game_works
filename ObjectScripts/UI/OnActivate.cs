using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using GWCon = GWConst.GWConstManager;
using UnityEngine.Analytics;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.UI;
using UnityEngine.Purchasing.Extension;

public class OnActivate : MonoBehaviour, IDetailedStoreListener
{
    [SerializeField] private ushort which;
    [SerializeField] private string open_menu;
    [SerializeField] private string open_scene;
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;
    private UnityEngine.Purchasing.Product product;
    private bool disable_purchasing_button = true;
    private bool loaded = false;

    public async void onActivate()
    {
        GWEventManager.gwevent_manager.TriggerEvent(new GameEvent(
                GWConst.GWConstManager.SOUND_EVENT_TAG, default(System.DateTime), false,
                new SoundEventValue(3, false, 0, true, 0.3f)));
        switch (which)
        {
            case 0:
                break;
            case 1:
                // open menu
                int menu_index = Array.IndexOf(GWConst.GWConstManager.menus, open_menu);
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWCon.CHANGE_MENU_EVENT_TAG, default(DateTime), false, menu_index));
                break;
            case 2:
                // open scene
                if (GWStateManager.dying && GWStateManager.current_menu == "PauseMenu")
                {
                    GWStateManager.paused = false;
                    return;
                }
                if(GWStateManager.current_menu == "PauseMenu")
                {
                    close_menu("PauseMenu");
                    GWStateManager.paused = false;
                }
                load_scene();
                break;
            case 3:
                // end game -> load start menu or close whole game
                if (open_scene == "End")
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#else
                    
                    Application.Quit();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
                    return;
                }
                
                open_scene = "StartScene";
                load_scene();
                break;
            case 4:
                // close pause menu
                close_menu("PauseMenu");
                GWStateManager.interrupted = false;
                break;
            case 5:
                // restart game
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWConst.GWConstManager.START_NEW_GAME_EVENT_TAG, default(System.DateTime), false, true));
                open_scene = "Level_1";
                load_scene();
                break;
            case 6:
                GWStateManager.volume = (ushort)Math.Min((ushort)10, (ushort)(GWStateManager.volume + short.Parse(open_menu)));
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWConst.GWConstManager.SETTINGS_EVENT_TAG, default(System.DateTime), true,
                    new SettingsEventValue("AdjustVolume", false, 0)));
                break;
            case 7:
                GWStateManager.language = (ushort)((ushort)Math.Abs(GWStateManager.language + short.Parse(open_menu)) % GWCon.supported_languages.Length);
                GWEventManager.gwevent_manager.TriggerEvent(
                    new GameEvent(GWConst.GWConstManager.SETTINGS_EVENT_TAG, default(System.DateTime), true,
                    new SettingsEventValue("ChangeLanguage", false, 0)));
                break;
        default:
            Debug.LogError("The function with the which number "+which+" hasn't been defined");
            break;
        }
    }

    public static void close_menu(String menu)
    {
        int i = Array.IndexOf(GWConst.GWConstManager.menus, menu);
        GWEventManager.gwevent_manager.TriggerEvent(
            new GameEvent(GWConst.GWConstManager.CHANGE_MENU_EVENT_TAG, default(DateTime), false, i));
        GWStateManager.paused = false;
    }

    public void load_scene()
    {
        EventValueScene scene = new EventValueScene();
        scene.scene_to_load = open_scene;
        if (open_scene == "")
        {
            scene.scene_to_load = SceneManager.GetActiveScene().name;
        }
        GameEvent scene_event = new GameEvent(
            GWCon.SCENE_EVENT_TAG, DateTime.UtcNow, false, scene);
        GWEventManager.gwevent_manager.TriggerEvent(scene_event);
    }
}
