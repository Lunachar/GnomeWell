using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerDownHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Button _button;
    private AudioSource _audioSource;

    private void Start()
    {
        _button = GetComponent<Button>();
        
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && _button.interactable)
        {
            StartCoroutine(PlaySoundAndWait(hoverSound));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // if (clickSound != null && _button.interactable)
        // {
        //     StartCoroutine(PlaySoundAndWait(clickSound));
        // }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null && _button.interactable)
        {
            StartCoroutine(PlaySoundAndWait(clickSound));
        }
    }

    private IEnumerator PlaySoundAndWait(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
        yield return new WaitForSecondsRealtime(clip.length);
        Debug.Log($"sound is finishing playing");
    }

}
