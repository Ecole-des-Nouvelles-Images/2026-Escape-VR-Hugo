using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRSceneRefresh : MonoBehaviour
{
    XRRayInteractor[] interactors;

    void Awake()
    {
        interactors = GetComponentsInChildren<XRRayInteractor>(true);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        yield return null;

        foreach (var i in interactors)
        {
            i.enabled = false;
            i.enabled = true;
        }
    }
}