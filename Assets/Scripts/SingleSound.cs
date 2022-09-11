using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSound : MonoBehaviour
{
    public List<AudioClip> Sounds;
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        var s = Random.Range(0, Sounds.Count);
        audio.clip = Sounds[s];
        audio.volume = PlayerPrefs.GetFloat("SFX Volume") *0.8f;
        audio.Play();
    }
}
