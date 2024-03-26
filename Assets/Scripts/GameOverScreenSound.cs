using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameOverScreenSound : MonoBehaviour
{
    public List<AudioClip> songs;
    private AudioSource _audioSource;
    private int _currentSongIndex = 0;
    private bool loopSecondSong = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.clip = songs[_currentSongIndex];
        Debug.Log($"{_audioSource.clip.name}");
        _audioSource.Play();
        StartCoroutine(WaitForSongEnd());
        
    }

    IEnumerator WaitForSongEnd()
    {
        yield return new WaitForSecondsRealtime(songs[0].length);
        _audioSource.loop = true;
        _audioSource.clip = songs[_currentSongIndex + 1];
        Debug.Log($"{_audioSource.clip.name}");
        _audioSource.Play();
    }
}
