using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class SignalOnTouch : MonoBehaviour
{
    // Event triggered when the object is touched.
    public UnityEvent onTouch;

    // Flag to determine whether to play audio on touch.
    public bool playAudioOnTouch = true;

    // Called when another collider enters this collider (2D physics only).
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Send the signal when the player enters the collider.
        SendSignal(collider.gameObject);
    }

    // Called when this collider contacts another collider (2D physics only).
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Send the signal when the player collides with this object.
        SendSignal(collision.gameObject);
    }

    // Send the signal and play audio (if enabled) when touched by the player.
    void SendSignal(GameObject objectThatHit)
    {
        if (objectThatHit.CompareTag("Player"))
        {
            // Play audio if enabled and an AudioSource component exists and is active.
            if (playAudioOnTouch)
            {
                var audio = GetComponent<AudioSource>();
                if (audio && audio.gameObject.activeInHierarchy)
                {
                    audio.Play();
                }
            }
            
            // Trigger the onTouch event.
            onTouch.Invoke();
        }
    }
}