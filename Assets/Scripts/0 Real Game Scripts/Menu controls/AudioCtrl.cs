using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class AudioCtrl : MonoBehaviour
{


    //Audio variables
    public Slider masterParam, musicParam, sfxParam, ambienceParam;
    public AudioMixer mainAudioMixer;

    //audioo methods
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterParam", masterParam.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicParam", musicParam.value);
    }
    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFXParam", sfxParam.value);
    }
    public void ChangeAmbienceVolume()
    {
        mainAudioMixer.SetFloat("AmbienceParam", ambienceParam.value);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
