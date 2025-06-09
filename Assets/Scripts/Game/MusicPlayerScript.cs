using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    public List<AudioClip> songs; // zieh die Songs hier rein im Inspector
    public AudioSource audioSource;

    private List<int> playedIndices = new List<int>();

    void Start()
    {
        PlayRandomSong();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomSong();
        }
    }

    void PlayRandomSong()
    {
        if (songs.Count == 0) return;

        // Optional: Alle Songs wurden gespielt, Liste zurücksetzen
        if (playedIndices.Count >= songs.Count)
        {
            playedIndices.Clear();
        }

        int index;
        do
        {
            index = Random.Range(0, songs.Count);
        } while (playedIndices.Contains(index));

        playedIndices.Add(index);

        audioSource.clip = songs[index];
        audioSource.Play();
    }
}
