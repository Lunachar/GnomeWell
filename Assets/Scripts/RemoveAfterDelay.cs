using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterDelay : MonoBehaviour
{
    public float delay = 1.0f;

    private void Start()
    {
        StartCoroutine("Remove");
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
