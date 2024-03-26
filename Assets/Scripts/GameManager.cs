using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject startingPoint; // The starting point for the gnome.

    public Rope rope; // Reference to the rope object.

    public CameraFollow cameraFollow; // Reference to the camera follow script.

    private Gnome _currentGnome; // Reference to the current gnome in the game.

    public GameObject gnomePrefab; // Prefab for the gnome.

    public RectTransform mainMenu; // Reference to the main menu UI.

    public RectTransform gameplayMenu; // Reference to the gameplay menu UI.

    public RectTransform gameOverMenu; // Reference to the game over menu UI.

    public bool gnomeInvicible { get; set; } // Flag to indicate if the gnome is invincible.

    public float delayAfterDeath = 2.0f; // Delay after the gnome's death before resetting the game.

    public AudioClip gnomeDiedSound; // Sound played when the gnome dies.

    public AudioClip gameOverSound; // Sound played when the game is over.

    private void Start()
    {
        Reset(); // Initialize the game.
    }

    public void Reset()
    {
        // Deactivate the game over menu if it exists.
        if (gameOverMenu)
        {
            gameOverMenu.gameObject.SetActive(false);
        }

        // Deactivate the main menu if it exists.
        if (mainMenu)
        {
            mainMenu.gameObject.SetActive(false);
        }

        // Activate the gameplay menu.
        if (gameplayMenu)
        {
            gameplayMenu.gameObject.SetActive(true);
        }

        GameObject[] partsToDelete = GameObject.FindGameObjectsWithTag("Temp");
        if (partsToDelete != null)
        {
            foreach (var part in partsToDelete)
            {
                Destroy(part);
            }
        }
        // Reset all objects that implement the Resettable interface.
        var resetObjects = FindObjectsOfType<Resettable>();
        foreach (Resettable r in resetObjects)
        {
            r.Reset();
        }

        CreateNewGnome(); // Create a new gnome.
        Time.timeScale = 1.0f; // Reset the time scale.
    }

    void CreateNewGnome()
    {
        RemoveGnome(); // Remove the existing gnome, if any.

        // Instantiate a new gnome at the starting point.
        GameObject newGnome = Instantiate(gnomePrefab, startingPoint.transform.position, Quaternion.identity);
        _currentGnome = newGnome.GetComponent<Gnome>(); // Get the Gnome component.

        rope.gameObject.SetActive(true); // Activate the rope.
        rope.connectedObject = _currentGnome.ropeBody; // Set the connected object of the rope.
        rope.ResetLength(); // Reset the length of the rope.

        cameraFollow.target = _currentGnome.cameraFollowTarget; // Set the camera's target to follow the gnome.
    }

    void RemoveGnome()
    {
        // Check if the gnome is invincible before removing.
        if (gnomeInvicible)
            return;

        rope.gameObject.SetActive(false); // Deactivate the rope.

        cameraFollow.target = null; // Remove the camera target.

        // Check if the current gnome exists.
        if (_currentGnome != null)
        {
            _currentGnome.holdingTreasure = false; // Set holdingTreasure to false.
            _currentGnome.gameObject.tag = "Untagged"; // Reset the gnome's tag.

            // Reset the tags of all child objects of the gnome.
            foreach (Transform child in _currentGnome.transform)
            {
                child.gameObject.tag = "Untagged";
            }

            _currentGnome = null; // Set the current gnome to null.
        }
    }

    void KillGnome(Gnome.DamageType damageType)
    {
        var audio = GetComponent<AudioSource>(); // Get the AudioSource component.
        if (audio)
        {
            audio.PlayOneShot(this.gnomeDiedSound); // Play the gnome died sound.
        }

        _currentGnome.ShowDamageEffect(damageType); // Show damage effect on the gnome.

        // Check if the gnome is invincible.
        if (gnomeInvicible == false)
        {
            _currentGnome.DestroyGnome(damageType); // Destroy the gnome.

            RemoveGnome(); // Remove the gnome.

            StartCoroutine(ResetAfterDelay()); // Reset the game after a delay.
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterDeath); // Wait for the specified delay.
        Reset(); // Reset the game.
    }

    // Handle trap interaction - slicing damage.
    public void TrapTouched()
    {
        KillGnome(Gnome.DamageType.Slicing);
    }

    // Handle trap interaction - burning damage.
    public void FireTrapTouched()
    {
        KillGnome(Gnome.DamageType.Burning);
    }

    // Handle treasure collection.
    public void TreasureCollected()
    {
        _currentGnome.holdingTreasure = true;
    }

    // Handle reaching the exit.
    public void ExitReached()
    {
        // Check if the current gnome exists and is holding treasure.
        if (_currentGnome != null && _currentGnome.holdingTreasure)
        {
            var audio = GetComponent<AudioSource>(); // Get the AudioSource component.
            if (audio)
            {
                audio.PlayOneShot
                    (this.gameOverSound); // Play the game over sound.
            }

            Time.timeScale = 0.0f; // Pause the game.

            // Display the game over menu.
            if (gameOverMenu)
            {
                mainMenu.gameObject.SetActive(false);
                gameplayMenu.gameObject.SetActive(false);
                gameOverMenu.gameObject.SetActive(true);
            }
        }
    }

    // Set the game to paused or resumed state.
    public void SetPaused(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0.0f; // Set the time scale to zero to pause the game.
            mainMenu.gameObject.SetActive(true); // Display the main menu.
            gameplayMenu.gameObject.SetActive(false); // Hide the gameplay menu.
        }
        else
        {
            Time.timeScale = 1.0f; // Set the time scale to normal to resume the game.
            mainMenu.gameObject.SetActive(false); // Hide the main menu.
            gameplayMenu.gameObject.SetActive(true); // Display the gameplay menu.
        }
    }

    // Restart the game.
    public void RestartGame()
    {
        Destroy(_currentGnome.gameObject); // Destroy the current gnome game object.
        _currentGnome = null; // Set the current gnome reference to null.

        Reset(); // Reset the game.
    }
}

