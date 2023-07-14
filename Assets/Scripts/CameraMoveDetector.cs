using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveDetector : MonoBehaviour
{
    public CameraController cam;
    public int moveDirection;

    void OnMouseOver()
    {
        cam.MoveCamera(moveDirection);
    }

    void OnMouseExit()
    {
        cam.StopCamera();
    }
}
