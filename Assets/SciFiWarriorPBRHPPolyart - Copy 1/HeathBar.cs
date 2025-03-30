using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathBar : MonoBehaviour
{
    private Transform cam;

    public void SetCamera(Transform cameraTransform)
    {
        cam = cameraTransform;
    }

    void Update()
    {
        if (cam != null)
        {
            transform.LookAt(cam); // Example of using the camera reference
        }
    }
}
