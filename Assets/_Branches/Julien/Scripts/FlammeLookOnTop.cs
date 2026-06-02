using System;
using UnityEngine;

public class FlammeLookOnTop : MonoBehaviour
{
    private void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    }
}
