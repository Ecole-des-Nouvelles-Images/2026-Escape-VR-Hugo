using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public class VrRecenter : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform targetHeading;

    IEnumerator Start()
    {
        // On attend 2 frames pour être sûr que le SDK XR est réveillé
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        ExecuteRecenter();
    }

    public void ExecuteRecenter()
    {
        xrOrigin.MatchOriginUpCameraForward(targetHeading.up, targetHeading.forward);
    }
}
