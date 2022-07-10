using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicManager : MonoBehaviour
{

    public List<AudioClip> MusicTracks;
    public int CurrentTrack;
    public AudioSource MusicSource;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        if (!MusicSource.isPlaying)
        {
            var pick = Random.Range(0, MusicTracks.Count);
            if(pick != CurrentTrack)
            {
                MusicSource.clip = MusicTracks[pick];
                MusicSource.Play();
            }
        }
    }

    
}
