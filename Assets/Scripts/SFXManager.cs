using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource SFX;

    public List<AudioClip> DamageSounds;
    public List<AudioClip> SwordSounds;
    public List<AudioClip> ClubSounds;
    public List<AudioClip> BowSounds;
    public List<AudioClip> WandSounds;


    public void PlayDamageSound()
    {
        var s = Random.Range(0, DamageSounds.Count);
        SFX.clip = DamageSounds[s];
        SFX.Play();
    }

    public void PlaySwordSound()
    {
        var s = Random.Range(0, SwordSounds.Count);
        SFX.clip = SwordSounds[s];
        SFX.Play();
    }

    public void PlayBowSound()
    {
        var s = Random.Range(0, BowSounds.Count);
        SFX.clip = BowSounds[s];
        SFX.Play();
    }
    public void PlayClubSound()
    {
        var s = Random.Range(0, ClubSounds.Count);
        SFX.clip = ClubSounds[s];
        SFX.Play();
    }
    public void PlayWandSound()
    {
        var s = Random.Range(0, WandSounds.Count);
        SFX.clip = WandSounds[s];
        SFX.Play();
    }

    public void PlaySound(AudioClip a)
    {
        SFX.clip = a;
        SFX.Play();
    }


}
