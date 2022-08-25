using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] bool invert;

    Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            // f** u with all your "blah-blah hard-coded numbers" i could store it in some int fuckYouAsshole = -1; but you know.. fuck you.
            // this one is for proper visualization of text position and it's look to camera
            transform.LookAt(transform.position + dirToCamera * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
