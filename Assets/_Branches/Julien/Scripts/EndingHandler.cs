using System.Collections;
using Managers;
using UnityEngine;

public class EndingHandler : MonoBehaviour
{
    private bool _wasTaked;
    public string SceneName;
    [SerializeField] private float _timeOfSound;
    [SerializeField] private GameObject _teleport;
    
    public void PlayEnding()
    {
        // Set le _timeOfSound par rapport à la duréer du temp des paroles
        if (_wasTaked) return;
        _wasTaked = true;
        _timeOfSound = 5;

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        Debug.Log("Start delay");
        yield return new WaitForSeconds(_timeOfSound + 3f);
        _teleport.SetActive(true);
        Debug.Log("finished");
    }
}
