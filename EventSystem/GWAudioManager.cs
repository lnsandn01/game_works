using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GWAudioManager : MonoBehaviour
{
    [SerializeField]private AudioMixer master_mixer;

    private void Awake()
    {
        GWEventManager.sound_event += onSound;
        GWEventManager.level_start_event += onLevelStart;
    }

    private void Update()
    {
        master_mixer.SetFloat("MasterVolume", -80 + 10*GWStateManager.volume);
    }

    private void onSound(GameEvent game_event)
    {
        SoundEventValue value = (SoundEventValue)game_event.value;
        var audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach(AudioSource audio in audioSources)
        {
            if (!audio.gameObject.GetComponent<ObjectId>())
            {
                continue;
            }
            if (audio.gameObject.GetComponent<ObjectId>().id != value.id)
            {
                continue;
            }
            if (value.activate)
            {
                if (value.fade)
                {
                    StartCoroutine(AudioFadeIn.FadeIn(audio, value.fade_time, value.end_volume));
                }
                else
                {
                    audio.gameObject.SetActive(true);
                    if (!audio.playOnAwake)
                    {
                        audio.Play();
                    }
                }
            }
            else
            {
                if (value.fade)
                {
                    StartCoroutine(AudioFadeOut.FadeOut(audio, value.fade_time));
                }
                else
                {
                    audio.gameObject.SetActive(false);
                }
            }
            return;      
        }
    }

    private void onLevelStart(GameEvent game_event)
    {
        if ((bool)game_event.value)
        {
            // level is starting
            StartCoroutine(AudioFadeIn.FadeIn(null, 0.1f, 1f, master_mixer));
        }
        else
        {
            // level is ending

        }
    }

    public static AudioSource getSoundOfObject(GameObject go, string name)
    {
        Transform sounds = go.transform.Find("Sounds");

        if (sounds)
        {
            foreach (Transform child in sounds)
            {
                if (child.name == name)
                {
                    return child.GetComponent<AudioSource>();
                }
            }
        }
        return null;
    }

    public static List<AudioSource> getSoundsOfObject(GameObject go)
    {
        Transform sounds = go.transform.Find("Sounds");
        List<AudioSource> audio_sources = new List<AudioSource>();
        if (sounds)
        {
            foreach (Transform child in sounds)
            {
                audio_sources.Add(child.GetComponent<AudioSource>());
            }
        }
        return audio_sources;
    }
}
