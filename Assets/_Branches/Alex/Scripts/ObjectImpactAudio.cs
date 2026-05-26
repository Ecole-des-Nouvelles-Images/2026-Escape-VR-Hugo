using Core.Audio;
using FMODUnity;
using UnityEngine;

public class ObjectImpactAudio : MonoBehaviour
{
    [SerializeField] private EventReference impactSound;
    
    [Header("Impact Settings")]
    [SerializeField] private float velocityThreshold = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        float velocity = collision.relativeVelocity.magnitude;
        if (velocity > velocityThreshold)
        {
            AudioManager.Instance.PlayAtPosition(impactSound, transform.position);
        }
    }
}
