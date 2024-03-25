using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    // Reference to the camera follow target.
    public Transform cameraFollowTarget;
    
    // Reference to the rope body.
    public Rigidbody2D ropeBody;

    // Sprites for the arm holding empty and holding treasure.
    public Sprite armHoldingEmpty;
    public Sprite armHoldingTreasure;

    // Reference to the sprite renderer for holding arm.
    public SpriteRenderer holdingArm;

    // Prefabs for gnome death, flame death, and ghost.
    public GameObject deathPrefab;
    public GameObject flameDeathPrefab;
    public GameObject ghostPrefab;

    // Delay before removing the gnome and releasing the ghost.
    public float delayBeforeRemoving = 3.0f;
    public float delayBeforeReleasingGhost = 0.25f;

    // Prefab for blood fountain.
    public GameObject bloodFountainPrefab;

    // Flag indicating if the gnome is holding treasure.
    private bool _holdingTreasure = false;

    // Flag indicating if the gnome is dead.
    private bool _dead = false;

    // Position of gnome's death.
    private Vector3 _deathPosition;

    // Property to get and set the holding treasure flag.
    public bool holdingTreasure
    {
        get
        {
            return _holdingTreasure;
        }
        set
        {
            if (_dead == true)
            {
                return; // Do nothing if the gnome is dead.
            }

            _holdingTreasure = value;
            // Update the holding arm sprite based on the holding treasure flag.
            if (holdingArm != null)
            {
                if (_holdingTreasure)
                {
                    holdingArm.sprite = armHoldingTreasure;
                }
                else
                {
                    holdingArm.sprite = armHoldingEmpty;
                }
            }
        }
    }

    // Enum for damage types.
    public enum DamageType
    {
        Slicing,
        Burning
    }

    // Show the damage effect based on the damage type.
    public void ShowDamageEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Burning:
                if (flameDeathPrefab != null)
                {
                    Instantiate(flameDeathPrefab,
                        cameraFollowTarget.position, cameraFollowTarget.rotation);
                }
                break;
            case DamageType.Slicing:
                if (deathPrefab != null)
                {
                    Instantiate(deathPrefab,
                        cameraFollowTarget.position, cameraFollowTarget.rotation);
                }
                break;
        }
    }

    // Destroy the gnome based on the damage type.
    public void DestroyGnome(DamageType type)
    {
        _deathPosition = GameObject.Find("Prototype Body").transform.position;
        holdingTreasure = false;
        _dead = true;
        
        StartCoroutine(ReleaseGhost(_deathPosition));

        foreach (BodyPart part in GetComponentsInChildren<BodyPart>())
        {
            switch (type)
            {
                case DamageType.Burning:
                    bool shouldBurn = Random.Range(0, 2) == 0;
                    if (shouldBurn)
                    {
                        part.ApplyDamageSprite(type);
                    }
                    break;
                case DamageType.Slicing:
                    part.ApplyDamageSprite(type);
                    break;
            }

            bool shouldDetach = Random.Range(0, 2) == 0;

            if (shouldDetach)
            {
                part.Detach();

                if (type == DamageType.Slicing)
                {
                    if (part.bloodFountainOrigin != null && bloodFountainPrefab != null)
                    {
                        GameObject fountain = Instantiate(
                            bloodFountainPrefab,
                            part.bloodFountainOrigin.position,
                            part.bloodFountainOrigin.rotation
                            ) as GameObject;
                        fountain.transform.SetParent(
                            this.cameraFollowTarget,
                            false);
                    }
                }

                var allJoints = part.GetComponentsInChildren<Joint2D>();
                foreach (Joint2D joint in allJoints)
                {
                    Destroy(joint);
                }
            }
        }

        var remove = gameObject.AddComponent<RemoveAfterDelay>();
        remove.delay = delayBeforeRemoving;

        //StartCoroutine(ReleaseGhost());
    }

    // Release the ghost with a delay after the gnome's death.
    public IEnumerator ReleaseGhost(Vector3 deathPosition)
    {
        if (ghostPrefab == null)
        {
            yield break;
        }

        GameObject ghost = Instantiate(ghostPrefab, deathPosition, Quaternion.identity);
        Rigidbody2D ghostRigidbody = ghost.GetComponent<Rigidbody2D>();
        ghostRigidbody.velocity = new Vector2(0, 2);
        Destroy(ghost, 2f);
        yield return new WaitForSeconds(2f);
    }
}
