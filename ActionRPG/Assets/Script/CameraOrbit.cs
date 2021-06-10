using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float lookSensitivity;
    public float minXLook;
    public float maxXLook;
    public Transform camAnchor;

    public bool invertXRotation;

    private float curXRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Called at the end of each frame
    private void LateUpdate()
    {
        //get the mouse X and Y inputs
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        //Rotate the player left and right
        transform.eulerAngles += Vector3.up * x * lookSensitivity;

        if (invertXRotation)
            curXRot += y * lookSensitivity;
        else
            curXRot -= y * lookSensitivity;

        curXRot = Mathf.Clamp(curXRot, minXLook, maxXLook);

        Vector3 clampedAngle = camAnchor.eulerAngles;  //temporary variable to hold initial cameraanchor rotation
        clampedAngle.x = curXRot;                      // adding the x rotation i.e look up and down

        camAnchor.eulerAngles = clampedAngle;          //assigning the updated rotation values back to the camera anchor
    }
}
