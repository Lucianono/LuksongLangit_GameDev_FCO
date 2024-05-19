using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeTrigger : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        // Get the CinemachineImpulseSource component
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Method to trigger the camera shake
    public void ShakeCamera()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }
}

