using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    // Sprite for the detached body part.
    public Sprite detachedSprite;

    // Sprite for the burned body part.
    public Sprite burnedSprite;

    // Origin point for blood fountain effect.
    public Transform bloodFountainOrigin;

    // Flag indicating if the body part is detached.
    private bool detached = false;

    // Detach the body part.
    public void Detach()
    {
        detached = true;
        this.tag = "Untagged";
        transform.SetParent(null, true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!detached) return;

        var rigidbodyT = GetComponent<Rigidbody2D>();
        if (rigidbodyT/*.IsSleeping()*/)
        {
            // Destroy joints, rigidbodies, and colliders of the detached body part.
            foreach (Joint2D joint in GetComponentsInChildren<Joint2D>())
            {
                Destroy(joint);
            }

            foreach (Rigidbody2D body in GetComponentsInChildren<Rigidbody2D>())
            {
                Destroy(body);
            }

            foreach (Collider2D colliderT in GetComponentsInChildren<Collider2D>())
            {
                Destroy(colliderT);
            }
            // Log detachment and destruction of the body part.
            Debug.Log($"{gameObject.name} is detached {detached}");
            Destroy(this);
            Debug.Log($"{gameObject.name} is Destoyed");
            Debug.Log($"{gameObject.transform.position.y}");
        }

        // if (gameObject.transform.position.y < -0f)
        // {
        //     Destroy(this);
        // }
    }

    // Apply the damage sprite based on the damage type.
    public void ApplyDamageSprite(Gnome.DamageType damageType)
    {
        Sprite spriteToUse = null;

        switch (damageType)
        {
            case Gnome.DamageType.Burning:
                spriteToUse = burnedSprite;
                break;
            
            case Gnome.DamageType.Slicing:
                spriteToUse = detachedSprite;
                break;
        }

        // Set the sprite of the body part.
        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }
}
