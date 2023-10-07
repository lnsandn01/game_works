using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public static class AudioFadeIn
{
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float endVolume, AudioMixer mixer=null)
    {
        if(audioSource == null)
        {
            // fade in mixer
            float volume = 0f;
            while (volume < endVolume)
            {
                volume += endVolume * Time.deltaTime / FadeTime;
                mixer.SetFloat("InGameVolume", volume);

                yield return null;
            }
        }
        else
        {
            // fade in specific sound source
            audioSource.Play();
            while (audioSource.volume < endVolume)
            {
                audioSource.volume += endVolume * Time.deltaTime / FadeTime;

                yield return null;
            }
        }
        yield return null;
    }
}