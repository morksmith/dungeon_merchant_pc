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
    public bool isSFX;
    public AudioType Type;

    private AudioSource musicSource;
    private AudioSource sfxSource;


    void Start()
    {
        musicSource = GameObject.FindObjectOfType<MusicManager>().gameObject.GetComponent<AudioSource>();
        sfxSource = GameObject.FindObjectOfType<SFXManager>().gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("Game Started") == 0)
        {
            if (Type == AudioType.Music)
            {
                AudioSlider.value = 1;
                PlayerPrefs.SetFloat("Music Volume", 1);
                if(musicSource!= null)
                {
                    musicSource.GetComponent<AudioSource>().volume = 1;

                }
            }
            else
            {
                Debug.Log("I am an SFX Slider");
                AudioSlider.value = 1;
                PlayerPrefs.SetFloat("SFX Volume", 1);
                if(sfxSource != null)
                {
                    sfxSource.GetComponent<AudioSource>().volume = 1;

                }
            }
            
        }
        else
        {
            if (Type == AudioType.Music)
            {
                AudioSlider.value = PlayerPrefs.GetFloat("Music Volume");
                musicSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
            }
            else
            {
                AudioSlider.value = PlayerPrefs.GetFloat("SFX Volume");
                sfxSource.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
            }
            
        }


    }






    public void SetVolume()
    {
        if (Type == AudioType.SFX)
        {
            PlayerPrefs.SetFloat("SFX Volume", AudioSlider.value);
            if(sfxSource != null)
            {
                sfxSource.volume = PlayerPrefs.GetFloat("SFX Volume");

            }
        }
        else
        {
            PlayerPrefs.SetFloat("Music Volume", AudioSlider.value);
            if(musicSource != null)
            {
                musicSource.volume = PlayerPrefs.GetFloat("Music Volume");

            }
        }
    }
}
