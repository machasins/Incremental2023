using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    public Transform followObject;
    public float normalViewSize;

    public Transform computer;
    public float computerViewSize;
    public float timeZoomComputer;
    public float timeZoomNormal;

    private float currentViewSize;
    private Vector3 normalPosition;
    private bool isZoomed = false;
    private GameObject zoomButton;
    private float z;
    
    void Start()
    {
        Physics2D.queriesHitTriggers = true;

        currentViewSize = normalViewSize;
        cam.m_Lens.OrthographicSize = currentViewSize;
        
        cam.Follow = followObject;

        z = transform.localPosition.z;

        normalPosition = transform.localPosition;
    }

    public void ZoomToComputer(GameObject button)
    {
        button.SetActive(false);
        zoomButton = button;
        cam.Follow = null;

        StartCoroutine(computerZoom());
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (isZoomed)
        {
            isZoomed = false;
            StartCoroutine(unZoom());
        }
    }

    IEnumerator computerZoom()
    {
        float time = 0.0f;
        Vector3 position = transform.localPosition;


        while (time <= timeZoomComputer)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(currentViewSize, computerViewSize, Mathf.SmoothStep(0.0f, 1.0f, time / timeZoomComputer));

            Vector3 newPos = Vector3.Lerp(position, computer.position, Mathf.SmoothStep(0.0f, 1.0f, time / timeZoomComputer));
            newPos.z = z;
            transform.localPosition = newPos;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        cam.m_Lens.OrthographicSize = computerViewSize;

        Vector3 finalPos = computer.position;
        finalPos.z = z;
        transform.localPosition = finalPos;

        normalPosition = position;
        currentViewSize = computerViewSize;
        isZoomed = true;
    }

    IEnumerator unZoom()
    {
        float time = 0.0f;
        Vector3 position = transform.localPosition;

        while (time <= timeZoomComputer)
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(currentViewSize, normalViewSize, Mathf.SmoothStep(0.0f, 1.0f, time / timeZoomNormal));

            Vector3 newPos = Vector3.Lerp(position, normalPosition, Mathf.SmoothStep(0.0f, 1.0f, time / timeZoomNormal));
            newPos.z = z;
            transform.localPosition = newPos;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        cam.m_Lens.OrthographicSize = normalViewSize;

        Vector3 finalPos = normalPosition;
        finalPos.z = z;
        transform.localPosition = finalPos;

        zoomButton.SetActive(true);
        currentViewSize = normalViewSize;
    }
}
