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
            if(PlayerPrefs.GetInt("Game Started") == 0)
            {
                AudioSlider.value = 1;
                PlayerPrefs.SetFloat("SFX Volume", 1);
                sfxSource.GetComponent<AudioSource>().volume = 1;
            }
            else
            {
                AudioSlider.value = PlayerPrefs.GetFloat("SFX Volume");
                sfxSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
            }
            

        }
        else
        {
            if (PlayerPrefs.GetInt("Game Started") == 0)
            {
                AudioSlider.value = 1;
                PlayerPrefs.SetFloat("Music Volume", 1);
                musicSource.GetComponent<AudioSource>().volume = 1;
            }
            else
            {
                AudioSlider.value = PlayerPrefs.GetFloat("Music Volume");
                musicSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
            }

        }

        PlayerPrefs.SetInt("Game Started", 1);


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
