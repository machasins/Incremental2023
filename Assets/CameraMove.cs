using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    public CameraController cam;
    public float deadZoneRadius;
    public float maximumRadius;
    public float speed;

    void FixedUpdate()
    {
        Vector2 pos = Mouse.current.position.ReadValue();
        pos = new Vector2(pos.x / Screen.width, pos.y / Screen.height);
        float distance = Mathf.Lerp(0, maximumRadius, 2 * Vector2.Distance(pos, new Vector2(0.5f, 0.5f)));

        if (distance > deadZoneRadius)
        {
            float adjustedSpeed = Mathf.Lerp(1.0f, speed, (distance - deadZoneRadius) / (maximumRadius - deadZoneRadius));
            cam.MoveCamera((pos - new Vector2(0.5f, 0.5f)).normalized * adjustedSpeed);
        }
        else
            cam.StopCamera();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, deadZoneRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, maximumRadius);
    }
}
