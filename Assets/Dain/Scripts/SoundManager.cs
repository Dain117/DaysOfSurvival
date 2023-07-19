using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    Slider volume;
    void Start()
    {
        volume = GetComponent<Slider>();

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolum", 1);
            Load();
        }
        else
            Load();
    }


    public void MusicVolume()
    {
        AudioListener.volume=volume.value;
        Save();
    }
    
    public void Load()
    {
        volume.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volume.value);
    }
}
