using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, AudioSource nextSource, float nextDuration, float nextVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        currentTime = 0;
        start = nextSource.volume;

        while (currentTime < nextDuration)
        {
            currentTime += Time.deltaTime;
            nextSource.volume = Mathf.Lerp(start, nextVolume, currentTime / nextDuration);
            yield return null;
        }
        yield break;
    }
}