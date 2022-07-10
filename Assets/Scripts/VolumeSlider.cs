using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider AudioSlider;
    public enum AudioType
    {
        SFX,
        Music
    }
    public AudioType Type;

    private AudioSource musicSource;
    private AudioSource sfxSource;


    void Start()
    {
        musicSource = GameObject.FindObjectOfType<MusicManager>().gameObject.GetComponent<AudioSource>();
        sfxSource = GameObject.FindObjectOfType<SFXManager>().gameObject.GetComponent<AudioSource>();

        if (Type == AudioType.SFX)
        {
            AudioSlider.value = PlayerPrefs.GetFloat("SFX Volume");
            sfxSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");

        }
        else
        {
            AudioSlider.value = PlayerPrefs.GetFloat("Music Volume");
            musicSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");

        }


    }

    public void SetVolume()
    {
        if (Type == AudioType.SFX)
        {
            PlayerPrefs.SetFloat("SFX Volume", AudioSlider.value);
            sfxSource.volume = PlayerPrefs.GetFloat("SFX Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Music Volume", AudioSlider.value);
            musicSource.volume = PlayerPrefs.GetFloat("Music Volume");
        }
    }
}
