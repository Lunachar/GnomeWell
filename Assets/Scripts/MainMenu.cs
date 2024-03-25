using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;

    public RectTransform loadingOverlay;

    private AsyncOperation sceneLoadingOperation;

    private float _delay = 1f;

    private void Start()
    {
        loadingOverlay.gameObject.SetActive(false);

        sceneLoadingOperation =
            SceneManager.LoadSceneAsync(sceneToLoad);
        sceneLoadingOperation.allowSceneActivation = false;
    }

    public void LoadScene()
    {
        loadingOverlay.gameObject.SetActive(true);

        StartCoroutine(ExecuteWithDelay());

        sceneLoadingOperation.allowSceneActivation = true;
    }

    IEnumerator ExecuteWithDelay()
    {
        yield return new WaitForSeconds(_delay);
    }
}
