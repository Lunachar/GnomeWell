using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
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

        _button.onClick.AddListener(PlayClickSound);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && _button.interactable)
        {
            _audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && _button.interactable)
        {
            _audioSource.PlayOneShot(clickSound);
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null && _button.interactable)
        {
            _audioSource.PlayOneShot(clickSound);
        }
    }
}
