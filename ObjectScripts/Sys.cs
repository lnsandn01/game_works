using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Purchasing;

public class Sys : MonoBehaviour
{
    [SerializeField]private GameObject system_prefab;
    [SerializeField]private GameObject system_menus_prefab;
    [SerializeField]private GameObject system_sounds_prefab;

    async void Awake()
    {
        // settings
        Application.targetFrameRate = 60;
        // missing object instantiation
        if (!GameObject.Find("System") && !GameObject.Find("System(Clone)"))
        {
            Instantiate(system_prefab, new Vector3(0,0,0), Quaternion.identity);
        }
        if (!GameObject.Find("SystemMenus") && !GameObject.Find("SystemMenus(Clone)"))
        {
            Instantiate(system_menus_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject lostGameMenu = GameObject.Find("LostGameMenu");
            if(lostGameMenu)
            {
                lostGameMenu.SetActive(false);
            }
            GameObject ingameui = GameObject.Find("InGameUI");
            if (ingameui)
            {
                if(GWStateManager.current_scene == "StartScene")
                {
                    ingameui.SetActive(false);
                }
            }
        }
        if (!GameObject.Find("SystemSounds") && !GameObject.Find("SystemSounds(Clone)"))
        {
            Instantiate(system_sounds_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

}
