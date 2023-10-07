using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using GWCon = GWConst.GWConstManager;

public class LanguageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textmp;
    [SerializeField] private bool is_language_setting_itself;
    [SerializeField] private int id;
    private void Awake()
    {
        GWEventManager.settings_event += onSettings;
    }

    private void OnEnable()
    {
        if (is_language_setting_itself)
        {
            textmp.text = GWConst.GWConstManager.supported_languages[GWStateManager.language];
            return;
        }
        changeText();
    }

    private void Start()
    {
        if (is_language_setting_itself)
        {
            textmp.text = GWConst.GWConstManager.supported_languages[GWStateManager.language];
            return;
        }
        changeText();
    }

    private void OnDisable()
    {
        GWEventManager.settings_event -= onSettings;
    }

    private void onSettings(GameEvent game_event)
    {
        if(((SettingsEventValue)game_event.value).setting_name == "ChangeLanguage")
        {
            if (is_language_setting_itself)
            {
                textmp.text = GWConst.GWConstManager.supported_languages[GWStateManager.language];
                return;
            }
            changeText();
        }
    }

    private void changeText()
    {
        TextBoxText tbt = Array.Find(
            GWCon.other_texts, item => item.id == id && GWStateManager.language == item.language);
        if (tbt == null)
        {
            Debug.LogWarning("text_id" + id + " not translated into given language " + GWStateManager.language);
            tbt = Array.Find(GWCon.other_texts, item => item.id == id);
        }
        if(tbt == null)
        {
            Debug.LogWarning("text_id" + id + " not given at all in GWCon.other_texts");
            return;
        }
        textmp.text = tbt.text;
    }
}
