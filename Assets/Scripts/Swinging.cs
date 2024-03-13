using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    public float swingSensivity = 100.0f;

    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            Destroy(this);
            return;
        }

        float swing = InputManager.instance.sidewaysMotion;

        Vector2 force = new Vector2(swing * swingSensivity, 0);
        
        GetComponent<Rigidbody2D>().AddForce(force);
    }
}
