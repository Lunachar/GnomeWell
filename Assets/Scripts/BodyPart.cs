using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    public Sprite detachedSprite;

    public Sprite burnedSprite;

    public Transform bloodFountainOrigin;

    private bool detached = false;
    public void Detach()
    {
        detached = true;
        this.tag = "Untagged";
        transform.SetParent(null, true);
    }

    private void Update()
    {
        if (!detached) return;

        var rigidbodyT = GetComponent<Rigidbody2D>();
        if (rigidbodyT/*.IsSleeping()*/)
        {
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

        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }
    
}
