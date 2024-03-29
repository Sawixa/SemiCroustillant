﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour

{
    #region VARIABLES

    public static AudioManager instance;
    public AudioFile[] audioFiles;
    private float timeToReset;
    private bool timerIsSet = false;

    private string tmpName;
    private float tmpVol;

    private bool isLowered = false;

    private bool fadeIn = false;
    private bool fadeOut = false;
    private string fadeInUsedString;
    private string fadeOutUsedString;

    #endregion



    void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (var s in audioFiles)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
            s.source.loop = s.isLooping;

            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }


    #region METHODS
    public static void PlaySFX(string name)
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
        if (s is null)

        {
            Debug.LogError("Sound name" + name + "not found!");
            return;
        }

        else
        {
            if (!s.source.isPlaying)
                s.source.Play();
        }
    }


    public static void StopSFX(string name)
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
        if (s is null)
        {
            Debug.LogError("Sound name" + name + "not found!");
            return;
        }

        else
        {
            s.source.Stop();
        }
    }


    public static void PauseSFX(string name)

    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);

        if (s is null)
        {
            Debug.LogError("Sound name" + name + "not found!");
            return;
        }

        else
        {
            s.source.Pause();
        }
    }


    public static void UnPauseSFX(string name)
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);

        if (s is null)
        {
            Debug.LogError("Sound name" + name + "not found!");
            return;
        }

        else
        {
            s.source.UnPause();
        }
    }



    public static AudioFile Find(string name)
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);

        if (s is null)
        {
            Debug.LogError("Sound name" + name + "not found!");
            return null;
        }

        else
        {
            return s;
        }
    }


    public static void LowerVolume(string name, float _duration)
    {
        if (instance.isLowered == false)
        {
            AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
            if (s is null)
            {
                Debug.LogError("Sound name" + name + "not found!");
                return;
            }

            else
            {
                instance.tmpName = name;
                instance.tmpVol = s.volume;
                instance.timeToReset = Time.time + _duration;
                instance.timerIsSet = true;
                s.source.volume = s.source.volume / 3;
            }
            instance.isLowered = true;
        }
    }


    public static void FadeOut(string name, float duration)
    {
        instance.StartCoroutine(instance.IFadeOut(name, duration));
    }


    public static void FadeIn(string name, float targetVolume, float duration)
    {
        instance.StartCoroutine(instance.IFadeIn(name, targetVolume, duration));
    }



    private IEnumerator IFadeOut(string name, float duration)
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);

        if (s is null)
        {
            Debug.LogError("Sound name" + name + "not found!");
            yield return null;
        }

        else
        {
            if (fadeOut == false)
            {
                fadeOut = true;
                float startVol = s.source.volume;
                fadeOutUsedString = name;

                while (s.source.volume > 0)
                {
                    s.source.volume -= startVol * Time.deltaTime / duration;
                    yield return null;
                }

                s.source.Stop();
                yield return new WaitForSeconds(duration);
                fadeOut = false;
            }


            else
            {
                Debug.Log("Could not handle two fade outs at once : " + name + " , " + fadeOutUsedString + "! Stopped the music " + name);
                StopSFX(name);
            }
        }
    }


    public IEnumerator IFadeIn(string name, float targetVolume, float duration)
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == name);
        if (s is null)
        {
            Debug.LogError("Sound name" + name + "not found!");
            yield return null;
        }

        else
        {
            if (fadeIn == false)
            {
                fadeIn = true;
                instance.fadeInUsedString = name;
                s.source.volume = 0f;
                s.source.Play();

                while (s.source.volume < targetVolume)
                {
                    s.source.volume += Time.deltaTime / duration;
                    yield return null;
                }

                yield return new WaitForSeconds(duration);
                fadeIn = false;
            }

            else
            {
                Debug.Log("Could not handle two fade ins at once: " + name + " , " + fadeInUsedString + "! Played the music " + name);
                StopSFX(fadeInUsedString);
                PlaySFX(name);
            }
        }
    }


    void ResetVol()
    {
        AudioFile s = Array.Find(instance.audioFiles, AudioFile => AudioFile.audioName == tmpName);
        s.source.volume = tmpVol;
        isLowered = false;
    }


    private void Update()
    {
        if (Time.time >= timeToReset && timerIsSet)
        {
            ResetVol();
            timerIsSet = false;
        }
    }

    #endregion
}